using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using mvvm;
using System;
using System.ComponentModel;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if UNITY_WSA || !NET_LEGACY
using System.Windows.Input;
#endif
using Component = UnityEngine.Component;

namespace mvvm
{
    public class CommandBinding : MonoBehaviour
    {
        [SerializeField]
        [FormerlySerializedAs("_view")]
        Component _target;

    #pragma warning disable 0169
        [SerializeField]
        [FormerlySerializedAs("_viewEvent")]
        string _targetEvent;

    #if !UNITY_EDITOR
    #pragma warning restore 0169
    #endif

        [SerializeField]
        [FormerlySerializedAs("_viewModel")]
        PropertyBinding.ComponentPath _source = null;

        [SerializeField]
        private BindingParameter _parameter = null;

        public object Parameter
        {
            get 
            {  
                return _runtimeParameter == null ? _parameter.GetValue() : _runtimeParameter; 
            }
            set
            {
                if (_runtimeParameter != null)
                {
                    _runtimeParameter = value;

                }
            }
        }

        private object _runtimeParameter = null;

        PropertyBinding.PropertyPath _vmProp;
        private ICommand _command;

        [Obsolete("")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public PropertyBinding.ComponentPath ViewModel
        {
            get
            {
                return Source;
            }
        }

        public PropertyBinding.ComponentPath Source
        {
            get
            {
                return _source;
            }
        }

        void Reset()
        {
            var context = gameObject.GetComponentInParent(typeof(DataContext)) as DataContext;
            if (context != null)
                _source = new PropertyBinding.ComponentPath { Component = context };

            _target = gameObject.GetComponent<UIBehaviour>();
        }

        void Awake()
        {
                
        }

        void OnDestroy()
        {
            
        }

        private void ClearBindings()
        {
            if (_source.Component is INotifyPropertyChanged)
                (_source.Component as INotifyPropertyChanged).PropertyChanged -= OnPropertyChanged;
            if (_command != null)
                _command.CanExecuteChanged -= CommandOnCanExecuteChanged;
        }

        private void FigureBindings()
        {
            Type vmtype;
            if (_source.Component is DataContext)
                vmtype = (_source.Component as DataContext).Type;
            else
                vmtype = _source.Component.GetType();

            _vmProp = new PropertyBinding.PropertyPath(_source.Property, vmtype, true);

            if (!_vmProp.IsValid)
            {
                Debug.LogErrorFormat(this, "{0}", gameObject.GetParentNameHierarchy());
            }

            if (!typeof(ICommand).IsAssignableFrom(_vmProp.PropertyType))
                _vmProp = null;

            if (_vmProp == null)
            {
                Debug.LogWarningFormat(this, "{0},{1}", _source.Property, vmtype);
            }

            if (_vmProp != null && _vmProp.IsValid)
            {
                if (_source.Component is INotifyPropertyChanged)
                    (_source.Component as INotifyPropertyChanged).PropertyChanged += OnPropertyChanged;

                BindCommand();
            }
        }

        private object GetVmValue()
        {
            if (_source.Component is DataContext)
                return (_source.Component as DataContext).GetValue(_vmProp);
            return _vmProp.GetValue(_source.Component, null);
        }

        private void BindCommand()
        {
            var ncommand = GetVmValue() as ICommand;

            if (_command != null)
                _command.CanExecuteChanged -= CommandOnCanExecuteChanged;

            _command = ncommand;

            if (_command != null)
            {
                _command.CanExecuteChanged += CommandOnCanExecuteChanged;
            }
            CommandOnCanExecuteChanged(this, new EventArgs());
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName != "" && propertyChangedEventArgs.PropertyName != _source.Property)
                return;

            BindCommand();
        }

        private void CommandOnCanExecuteChanged(object sender, EventArgs eventArgs)
        {
            SetViewEnabledState(_command == null || _command.CanExecute(Parameter));
        }

        private void SetViewEnabledState(bool state)
        {
            if (_target is Selectable)
                (_target as Selectable).interactable = state;
        }

        public void ExecuteCommand()
        {
            if (_command == null) return;
            _command.Execute(Parameter);
        }
    }

    public class BindingParameter
    {
        public BindingParameterType Type;
        public UnityEngine.Object ObjectReference;
        public string String;
        public int Int;
        public float Float;
        public bool Bool;

        public object GetValue()
        {
            switch (Type)
            {
                case BindingParameterType.String:
                    return String;
                case BindingParameterType.Int:  
                    return Int;
                case BindingParameterType.Float:
                    return Float;
                case BindingParameterType.Bool:
                    return Bool;
                case BindingParameterType.ObjectReference:
                    return ObjectReference;
                default:
                    return null;
            }
        }
    }

    public enum BindingParameterType
    {
        None,
        ObjectReference,
        String,
        Int,
        Float,
        Bool,
    }
}
