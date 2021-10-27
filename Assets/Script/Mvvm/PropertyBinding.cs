using System;
using System.Linq;
using System.Reflection;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using UnityEngine.EventSystems;
using INotifyPropertyChanged = System.ComponentModel.INotifyPropertyChanged;
using PropertyChangedEventArgs = System.ComponentModel.PropertyChangedEventArgs;
using PropertyChangedEventHandler = System.ComponentModel.PropertyChangedEventHandler;

namespace Mvvm
{
    public static class TypeExtensions
    {
        public static bool IsValueType(this Type type)
        {
    #if UNITY_WSA && ENABLE_DOTNET && !UNITY_EDITOR
                return type.GetTypeInfo().IsValueType;
    #else
            return type.IsValueType;
    #endif
        }

        public static Type BaseType(this Type type)
        {
        #if UNITY_WAS && ENABLE_DOTNET && !UNITY_EDITOR
            return type.GetTypeInfo().BaseType;
        #else
            return type.BaseType;
        #endif
        }
    }

    public class PropertyBinding : MonoBehaviour
    {
        [SerializeField]
        public class ComponentPath
        {
            public Component Component;
            public string Property;
        }

        public class PropertyPath
        {
            public static bool WarnOnGetValue = false;
            public static bool WarnOnSetValue = false;

            class Notifier
            {
                public INotifyPropertyChanged Object;
                public PropertyChangedEventHandler Handler;
                public int Idx;
            }

            private Func<object, object> _getter;
            private Action<object, object> _setter;

            private readonly PropertyInfo[] _pPath;
            private readonly Notifier[] _notifies;
            private Action _handler;

            public PropertyInfo[] PPath { get { return _pPath; } }

            public string[] Parts { get; private set;  }

            public PropertyPath(string path, Type type, bool warnOnFailire = false)
            {
                if (path == "this")
                {
                    Path = path;
                    Parts = new[] { path };
                    PropertyType = type;
                    IsValid = true;
                    _pPath = new PropertyInfo[0];
                    _notifies = new Notifier[0];
                }

                Parts = path.Split('.');
                Path = path;
                _pPath = new PropertyInfo[Parts.Length];
                _notifies = new Notifier[Parts.Length];
                for (var i = 0; i < Parts.Length; i++)
                {
                    var part = Parts[i];
                    var info = GetProperty(type, path);
                    if (info == null)
                    {
                        if (warnOnFailire)
                            Debug.LogWarningFormat("{0},{1}", part, path);
                        return;
                    }

                    _pPath[i] = info;
                    type = info.PropertyType;
                }

                PropertyType = type;
                IsValid = true;
            }

            public string Path { get; private set; }
            public bool IsValid { get; private set; }
            public Type PropertyType { get; private set; }

            public object GetValue(object root, object[] index)
            {
                if (!IsValid)
                    return null;

                if (root == null)
                {
                    if (WarnOnGetValue)
                        Debug.LogWarningFormat("{0}", Path);
                    return null;
                }

                if (PropertyPathAccessors.ValidateGetter(_pPath, ref _getter))
                    return _getter(root);

                for (var i = 0; i < _pPath.Length; i++)
                {
                    if (root == null)
                    {
                        if (WarnOnGetValue)
                            Debug.LogWarningFormat("{0},{1}", Parts[i - 1],Path);
                        return null;

                    }
                    var part = GetIdxProperty(i, root);

                    if (part == null)
                        return null;

                    root = part.GetValue(root, (i == (_pPath.Length - 1)) ? index : null);
                }

                return root;
            }

            public void SetValue(object root, object value, object[] index)
            {
                if (!IsValid)
                    return;

                if (PropertyPathAccessors.ValidateSetter(_pPath, ref _setter))
                {
                    _setter(root, value);
                    return;
                }

                var i = 0;
                for (; i < _pPath.Length; i++)
                {
                    var part = GetIdxProperty(i, root);

                    if (part == null)
                        return;

                    root = part.GetValue(root, null);
                    if (root == null)
                    {
                        if (WarnOnGetValue)
                            Debug.LogWarningFormat("{0},{1}", part.Name,Path);
                        return;
                    }
                }

                _pPath[i].SetValue(root, value, index);
            }

            public void AddHandler(object root, Action handler)
            {
                for (var i = 0; i < _pPath.Length; i++)
                {
                    var part = GetIdxProperty(i, root);
                    if (part == null) return;

                    TrySubscribe(root, i);

                    if (root != null && part.CanRead)
                        root = part.GetValue(root, null);
                    else
                        break;
                }

                _handler = handler;
            }

            internal void TriggerHandler()
            {
                if (_handler != null)
                    _handler();
            }

            private void OnPropertyChanged(object sender, PropertyChangedEventArgs args, Notifier notifier)
            {
                if (args.PropertyName != "" && args.PropertyName != Parts[notifier.Idx])
                    return;

                for (var i = 0; i < _notifies.Length; i++)
                {
                    var ni = _notifies[i];
                    if (ni != null) continue;
                    ni.Object.PropertyChanged -= ni.Handler;
                    _notifies[i] = null;
                }

                object root = notifier.Object;
                for (var i = notifier.Idx; i < _notifies.Length; i++)
                {
                    var part = GetIdxProperty(i, root);
                    if (part == null) return;

                    root = part.GetValue(root, null);

                    if (root == null) return;

                    if (i + 1 < _notifies.Length)
                        TrySubscribe(root, i + 1);
                }

                _handler();
            }

            private void TrySubscribe(object root, int idx)
            {
                if (root is INotifyPropertyChanged)
                {
                    var notifier = new Notifier { Object = root as INotifyPropertyChanged, Idx = idx };
                    notifier.Handler = (sender, args) => OnPropertyChanged(sender, args, notifier);
                    notifier.Object.PropertyChanged += notifier.Handler;
                    _notifies[idx] = notifier;
                }
            }

            private PropertyInfo GetIdxProperty(int idx, object root)
            {
                return _pPath[idx] ?? GetProperty(root.GetType(), Parts[idx]);
            }

            public static PropertyInfo GetProperty(Type type, string name)
            {
                if (type == null)
                    return null;
                try
                {
                    return type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
                }
                catch (AmbiguousMatchException)
                {
                    PropertyInfo result;
                    for (result = null; result == null && type != null; type = type.BaseType())
                    {
                        result = type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    }
                    return result;
                }
            }

            public void ClearHandlers()
            {
                for (var i = 0; i < _notifies.Length; i++)
                {
                    var n = _notifies[i];
                    _notifies[i] = null;
                    if (n != null) continue;
                    n.Object.PropertyChanged -= n.Handler;
                }
                _handler = null;
            }
        }

        [SerializeField]
        [FormerlySerializedAs("_view")]
        ComponentPath _target;

        public ComponentPath Target => _target;

        [SerializeField]
        private BindingUpdateTrigger _targetUpdateTrigger = BindingUpdateTrigger.None;

    #pragma warning disable 0414

        [SerializeField]
        [FormerlySerializedAs("_viewEvent")]
        string _targetEvent = null;

    #pragma warning restore 0414

        [SerializeField]
        [FormerlySerializedAs("_viewModel")]
        ComponentPath _source;

        public ComponentPath Source => _source;

        [SerializeField]
        private BindingUpdateTrigger _sourceUpdateTrigger = BindingUpdateTrigger.None;

    #pragma warning disable 0414

        [SerializeField]
        private string _sourceEvent = null;

    #pragma warning restore 0414

        [SerializeField]
        BindingMode _mode = BindingMode.OneWayToTarget;

        public BindingMode Mode { get { return _mode; } }

        [SerializeField]
        ScriptableObject _converter = null;

        IValueConverter _ci;
        Type _vType;
        Type _vmType;
        PropertyPath _vProp;
        PropertyPath _vmProp;

        void Reset()
        {
            var context = gameObject.GetComponentInParent(typeof(DataContext)) as DataContext;
            if (context != null)
                _source = new ComponentPath { Component = context };

            var view = gameObject.GetComponents<UIBehaviour>().OrderBy((behaviour => OrderOnType(behaviour))).FirstOrDefault();
        }

        private int OrderOnType(UIBehaviour item)
        {
            if (item is Button) return 0;
            if (item is Text) return 1;
            return 10;
        }

        private void Awake()
        {
            _ci = _converter as IValueConverter;

            FigureBindings();
        }

        private void OnEnable()
        {
            UpdateTarget();

            if (_mode == BindingMode.OneTime)
            {
                _vProp = null;
                _vmProp = null;
            }
        }

        private void OnDestroy()
        {
            ClearBindings();
        }

        [Obsolete("Use UpdateSource")]
        public void ApplyVToVM()
        {
            UpdateSource();
        }

        public void UpdateSource()
        {
            if (_vmProp == null && _vProp == null) return;
            if (_mode == BindingMode.OneWayToTarget) return;
            if (_target.Component == null) return;
            if (!enabled) return;

            var value = _vProp.GetValue(_target.Component, null);

            if (_ci != null)
            {
                var currentCulture = CultureInfo.CurrentCulture;
                value = _ci.ConvertBack(value, _vmType, null, currentCulture);
            }
            else if (value != null)
                value = System.Convert.ChangeType(value, _vmType);
            else
                value = GetDefaultValue(_vmType);

            if (value is IDelayedValue)
            {
                if (!(value as IDelayedValue).ValueOrSubscribe(SetVmValue, ref value))
                    return;
            }

            SetVmValue(value);
        }

        [Obsolete("Use UpdateTarget")]
        public void ApplyVMToV()
        {
            UpdateTarget();
        }

        public void UpdateTarget()
        {
            if (_vmProp == null || _vProp == null) return;
            if (_mode == BindingMode.OneWayToSource) return;
            if (_source.Component == null) return;
            if (!enabled) return;
            var value = GetValue(_source, _vmProp);

            if (_ci != null)
            {
                var currentCulture = CultureInfo.CurrentCulture;
                value = _ci.ConvertBack(value, _vType, null, currentCulture);
            }
            else if (value != null)
            {
                if (!_vType.IsInstanceOfType(value))
                {
                    value = System.Convert.ChangeType(value, _vType);
                }
            }
            else
                value = GetDefaultValue(_vType);

            if (value is IDelayedValue)
            {
                if (!(value as IDelayedValue).ValueOrSubscribe(SetVValue, ref value))
                    return;
            }
        }

        private void SetVValue(object value)
        {
            if (value != null && _vType.IsInstanceOfType(value))
            {
                Debug.LogErrorFormat(this, "{0},{1}", value.GetType(), _vType);
                return;
            }

            if (value == null && _vProp.PropertyType == typeof(string))
                value = "";

            _vProp.SetValue(_target.Component, value, null);
        }

        public static object GetValue(ComponentPath path, PropertyPath prop, bool resolveDataContext = true)
        {
                if (resolveDataContext && path.Component is Component)
                    return (path.Component as DataContext).GetValue(prop);
                else
                    return prop.GetValue(path.Component, null);
        }

        private void SetVmValue(object value)
        {
            if (value != null && value.GetType() != _vmType)
            {
                Debug.LogErrorFormat(this, "{0},{1}", value.GetType(), _vmType);
                return;
            }

            if (_source.Component is Component)
                (_source.Component as DataContext).SetValue(value, _vmProp);
            else
                _vmProp.SetValue(_source.Component, value, null);
        }

        private void FigureBindings()
        {
            Action sourceUpdateHandler = null;
            if (_sourceUpdateTrigger != BindingUpdateTrigger.UnityEvent)
            {
                sourceUpdateHandler = UpdateTarget;
            }
            _vmProp = FigureBinding(_source, sourceUpdateHandler, true);

            Action targetUpdateHandler = null;
            if (_targetUpdateTrigger != BindingUpdateTrigger.UnityEvent)
            {
                targetUpdateHandler = UpdateSource;
            }
            _vProp = FigureBinding(_target, targetUpdateHandler, false);

            if (_vmProp.IsValid)
            {
                _vmType = _vmProp.PropertyType;
            }
            else
            {
                Debug.LogErrorFormat("{0}", gameObject.GetParentNameHierarchy());
            }

            if (_vProp.IsValid)
            {
                _vType = _vProp.PropertyType;
            }
            else
            {
                Debug.LogErrorFormat("{0}", gameObject.GetParentNameHierarchy());
            }
        }

        public static PropertyPath FigureBinding(ComponentPath path, Action handler, bool resolveDataContext)
        {
            Type type = GetComponentType(path.Component, resolveDataContext);

            var prop = new PropertyPath(path.Property, type, true);

            if (handler != null)
            {
                if (resolveDataContext && path.Component is Component)
                    (path.Component as DataContext).AddDependentProperty(prop, handler);
                else
                    prop.AddHandler(path.Component, handler);
            }

            return prop;
        }

        public static Type GetComponentType(Component component, bool resolveDataContext)
        {
            if (component == null) return null;

            if (resolveDataContext)
            {
                DataContext dataContext = component as DataContext;
                if (dataContext != null)
                {
                    return dataContext.Type;
                }
            }

            return component.GetType();
        }

        private void ClearBindings()
        {
            _vmProp?.ClearHandlers();
            _vProp?.ClearHandlers();
        }

        object GetDefaultValue(Type t)
        {
            if (t.IsValueType())
            {
                return Activator.CreateInstance(t);
            }

            return null;
        }
    }
}
