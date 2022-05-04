using System;
using UnityEngine;
using UnityEngine.Events;
using Assembly_CSharp.Assets.Script.Simple.Execution;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy;
using Assembly_CSharp.Assets.Script.Simple.Binding.Contexts;
using Assembly_CSharp.Assets.Script.Simple.Binding.Converters;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Targets;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources;

namespace Assembly_CSharp.Assets.Script.Simple.Binding
{
    public class Binding : AbstractBinding
    {
        private readonly ISourceProxyFactory sourceProxyFactory;

        private readonly ITargetProxyFactory targetProxyFactory;
    
        private bool disposed = false;

        private BindingMode bindingMode = BindingMode.Default;

        private BindingDescription bindingDescription;

        private ISourceProxy sourceProxy;

        private ITargetProxy targetProxy;

        private EventHandler sourceValueChangedHandler;

        private EventHandler targetValueChangedHandler;

        private IConverter converter;

        private object _lock = new object();

        private bool isUpdatingSource;

        private bool isUpdatingTarget;

        private string targetTypeName;

        protected BindingMode BindingMode
        {
            get
            {
                if (bindingMode != BindingMode.Default)
                    return bindingMode;

                bindingMode = bindingDescription.Mode;
                if (bindingMode == BindingMode.Default)
                    bindingMode = targetProxy.DefaultMode;

                return bindingMode;
            }
        }

        public Binding(IBindingContext bindingContext, object source, object target, BindingDescription bindingDescription, ISourceProxyFactory sourceProxyFactory, ITargetProxyFactory targetProxyFactory) : base(bindingContext, source, target)
        {
            targetTypeName = target.GetType().Name;
            this.bindingDescription = bindingDescription;

            converter = bindingDescription.Converter;
            this.sourceProxyFactory = sourceProxyFactory;
            this.targetProxyFactory = targetProxyFactory;

            CreateTargetProxy(target, bindingDescription);
            CreateSourceProxy(DataContext, bindingDescription.Source);
            UpdateDataOnBind();
        }

        protected void CreateTargetProxy(object target, BindingDescription description)
        {
            DisposeTargetProxy();

            targetProxy = targetProxyFactory.CreateProxy(target, description);

            if (IsSubscribeTargetValueChanged(BindingMode) && targetProxy is INotifiable)
            {
                targetValueChangedHandler = (sender, args) => UpdateSourceFromTarget();
                (targetProxy as INotifiable).ValueChanged += targetValueChangedHandler;
            }
        }

        protected void DisposeTargetProxy()
        {
            try
            {
                if (targetProxy != null)
                {
                    if (targetValueChangedHandler != null)
                    {
                        (targetProxy as INotifiable).ValueChanged -= targetValueChangedHandler;
                        targetValueChangedHandler = null;
                    }
                    targetProxy.Dispose();
                    targetProxy = null;
                }
            }
            catch (Exception) { }
        }

        protected void CreateSourceProxy(object source, SourceDescription description)
        {
            DisposeSourceProxy();

            sourceProxy = sourceProxyFactory.CreateProxy(description.IsStatic ? null : source, description);

            if (IsSubscribeSourceValueChanged(BindingMode) && sourceProxy is INotifiable)
            {
                sourceValueChangedHandler = (sender, args) => UpdateTargetFromSource();
                (sourceProxy as INotifiable).ValueChanged += sourceValueChangedHandler;
            }
        }

        protected void DisposeSourceProxy()
        {
            try
            {
                if (sourceProxy != null)
                {
                    if (sourceValueChangedHandler!= null)
                    {
                        (sourceProxy as INotifiable).ValueChanged -= sourceValueChangedHandler;
                        sourceValueChangedHandler = null;
                    }
                    sourceProxy.Dispose();
                    sourceProxy = null;
                }
            }
            catch (Exception) { }
        }

        protected void UpdateDataOnBind()
        {
            try
            {
                if (UpdateTargetOnFirstBind(BindingMode) && sourceProxy != null)
                {
                    UpdateTargetFromSource();
                }

                if (UpdateSourceOnFirstBind(BindingMode) && targetProxy != null && targetProxy is IObtainable)
                {
                    UpdateSourceFromTarget();
                }
            }
            catch (Exception e)
            {

            }
        }

        protected bool UpdateTargetOnFirstBind(BindingMode bindingMode)
        {
            switch (bindingMode)
            {
                case BindingMode.Default:
                    return true;

                case BindingMode.OneWay:
                case BindingMode.OneTime:
                case BindingMode.TwoWay:
                    return true;

                case BindingMode.OneWayToSource:
                    return false;

                default:
                    throw new Exception();
            }
        }

        protected bool UpdateSourceOnFirstBind(BindingMode bindingMode)
        {
            switch (bindingMode)
            {
                case BindingMode.OneWayToSource:
                    return true;

                case BindingMode.Default:
                    return false;

                case BindingMode.OneWay:
                case BindingMode.OneTime:
                case BindingMode.TwoWay:
                    return false;

                default:
                    throw new Exception();
            }
        }

        protected bool IsSubscribeSourceValueChanged(BindingMode bindingMode)
        {
            switch (bindingMode)
            {
                case BindingMode.Default:
                    return true;

                case BindingMode.OneWay:
                case BindingMode.TwoWay:
                    return true;

                case BindingMode.OneTime:
                case BindingMode.OneWayToSource:
                    return false;

                default:
                    throw new Exception();
            }
        }

        protected bool IsSubscribeTargetValueChanged(BindingMode bindingMode)
        {
            switch (bindingMode)
            {
                case BindingMode.Default:
                    return true;

                case BindingMode.OneWay:
                case BindingMode.OneTime:
                    return false;

                case BindingMode.TwoWay:
                case BindingMode.OneWayToSource:
                    return true;

                default:
                    throw new Exception();
            }
        }

        protected virtual void UpdateTargetFromSource()
        {
            lock (_lock)
            {
                //Run on the main thread
                Executors.RunOnMainThread(() =>
                {
                    try
                    {
                        if (isUpdatingSource)
                            return;

                        isUpdatingTarget = true;

                        IObtainable obtainable = sourceProxy as IObtainable;
                        if (obtainable == null)
                            return;

                        IModifiable modifier = targetProxy as IModifiable;
                        if (modifier == null)
                            return;

                        TypeCode typeCode = sourceProxy.TypeCode;
                        switch (typeCode)
                        {
                            case TypeCode.Boolean:
                                {
                                    var value = obtainable.GetValue<bool>();
                                    SetTargetValue(modifier, value);
                                    break;
                                }
                            case TypeCode.Byte:
                                {
                                    var value = obtainable.GetValue<byte>();
                                    SetTargetValue(modifier, value);
                                    break;
                                }
                            case TypeCode.Char:
                                {
                                    var value = obtainable.GetValue<char>();
                                    SetTargetValue(modifier, value);
                                    break;
                                }
                            case TypeCode.DateTime:
                                {
                                    var value = obtainable.GetValue<DateTime>();
                                    SetTargetValue(modifier, value);
                                    break;
                                }
                            case TypeCode.Decimal:
                                {
                                    var value = obtainable.GetValue<decimal>();
                                    SetTargetValue(modifier, value);
                                    break;
                                }
                            case TypeCode.Double:
                                {
                                    var value = obtainable.GetValue<double>();
                                    SetTargetValue(modifier, value);
                                    break;
                                }
                            case TypeCode.Int16:
                                {
                                    var value = obtainable.GetValue<short>();
                                    SetTargetValue(modifier, value);
                                    break;
                                }
                            case TypeCode.Int32:
                                {
                                    var value = obtainable.GetValue<int>();
                                    SetTargetValue(modifier, value);
                                    break;
                                }
                            case TypeCode.Int64:
                                {
                                    var value = obtainable.GetValue<long>();
                                    SetTargetValue(modifier, value);
                                    break;
                                }
                            case TypeCode.SByte:
                                {
                                    var value = obtainable.GetValue<sbyte>();
                                    SetTargetValue(modifier, value);
                                    break;
                                }
                            case TypeCode.Single:
                                {
                                    var value = obtainable.GetValue<float>();
                                    SetTargetValue(modifier, value);
                                    break;
                                }
                            case TypeCode.String:
                                {
                                    var value = obtainable.GetValue<string>();
                                    SetTargetValue(modifier, value);
                                    break;
                                }
                            case TypeCode.UInt16:
                                {
                                    var value = obtainable.GetValue<ushort>();
                                    SetTargetValue(modifier, value);
                                    break;
                                }
                            case TypeCode.UInt32:
                                {
                                    var value = obtainable.GetValue<uint>();
                                    SetTargetValue(modifier, value);
                                    break;
                                }
                            case TypeCode.UInt64:
                                {
                                    var value = obtainable.GetValue<ulong>();
                                    SetTargetValue(modifier, value);
                                    break;
                                }
                            case TypeCode.Object:
                                {
                                    Type valueType = sourceProxy.Type;
                                    if (valueType.Equals(typeof(Vector2)))
                                    {
                                        var value = obtainable.GetValue<Vector2>();
                                        SetTargetValue(modifier, value);
                                    }
                                    else if (valueType.Equals(typeof(Vector3)))
                                    {
                                        var value = obtainable.GetValue<Vector3>();
                                        SetTargetValue(modifier, value);
                                    }
                                    else if (valueType.Equals(typeof(Vector4)))
                                    {
                                        var value = obtainable.GetValue<Vector4>();
                                        SetTargetValue(modifier, value);
                                    }
                                    else if (valueType.Equals(typeof(Color)))
                                    {
                                        var value = obtainable.GetValue<Color>();
                                        SetTargetValue(modifier, value);
                                    }
                                    else if (valueType.Equals(typeof(Rect)))
                                    {
                                        var value = obtainable.GetValue<Rect>();
                                        SetTargetValue(modifier, value);
                                    }
                                    else if (valueType.Equals(typeof(Quaternion)))
                                    {
                                        var value = obtainable.GetValue<Quaternion>();
                                        SetTargetValue(modifier, value);
                                    }
                                    else if (valueType.Equals(typeof(Version)))
                                    {
                                        var value = obtainable.GetValue<Version>();
                                        SetTargetValue(modifier, value);
                                    }
                                    else
                                    {
                                        var value = obtainable.GetValue();
                                        SetTargetValue(modifier, value);
                                    }
                                    break;
                                }
                            default:
                                {
                                    var value = obtainable.GetValue();
                                    SetTargetValue(modifier, value);
                                    break;
                                }
                        }
                    }
                    catch (Exception e)
                    {
                    }
                    finally
                    {
                        isUpdatingTarget = false;
                    }
                });
            }
        }

        protected virtual void UpdateSourceFromTarget()
        {
            try
            {
                if (isUpdatingTarget)
                    return;

                isUpdatingSource = true;


                IObtainable obtainable = targetProxy as IObtainable;
                if (obtainable == null)
                    return;

                IModifiable modifier = sourceProxy as IModifiable;
                if (modifier == null)
                    return;

                TypeCode typeCode = targetProxy.TypeCode;
                switch (typeCode)
                {
                    case TypeCode.Boolean:
                        {
                            var value = obtainable.GetValue<bool>();
                            SetSourceValue(modifier, value);
                            break;
                        }
                    case TypeCode.Byte:
                        {
                            var value = obtainable.GetValue<byte>();
                            SetSourceValue(modifier, value);
                            break;
                        }
                    case TypeCode.Char:
                        {
                            var value = obtainable.GetValue<char>();
                            SetSourceValue(modifier, value);
                            break;
                        }
                    case TypeCode.DateTime:
                        {
                            var value = obtainable.GetValue<DateTime>();
                            SetSourceValue(modifier, value);
                            break;
                        }
                    case TypeCode.Decimal:
                        {
                            var value = obtainable.GetValue<decimal>();
                            SetSourceValue(modifier, value);
                            break;
                        }
                    case TypeCode.Double:
                        {
                            var value = obtainable.GetValue<double>();
                            SetSourceValue(modifier, value);
                            break;
                        }
                    case TypeCode.Int16:
                        {
                            var value = obtainable.GetValue<short>();
                            SetSourceValue(modifier, value);
                            break;
                        }
                    case TypeCode.Int32:
                        {
                            var value = obtainable.GetValue<int>();
                            SetSourceValue(modifier, value);
                            break;
                        }
                    case TypeCode.Int64:
                        {
                            var value = obtainable.GetValue<long>();
                            SetSourceValue(modifier, value);
                            break;
                        }
                    case TypeCode.SByte:
                        {
                            var value = obtainable.GetValue<sbyte>();
                            SetSourceValue(modifier, value);
                            break;
                        }
                    case TypeCode.Single:
                        {
                            var value = obtainable.GetValue<float>();
                            SetSourceValue(modifier, value);
                            break;
                        }
                    case TypeCode.String:
                        {
                            var value = obtainable.GetValue<string>();
                            SetSourceValue(modifier, value);
                            break;
                        }
                    case TypeCode.UInt16:
                        {
                            var value = obtainable.GetValue<ushort>();
                            SetSourceValue(modifier, value);
                            break;
                        }
                    case TypeCode.UInt32:
                        {
                            var value = obtainable.GetValue<uint>();
                            SetSourceValue(modifier, value);
                            break;
                        }
                    case TypeCode.UInt64:
                        {
                            var value = obtainable.GetValue<ulong>();
                            SetSourceValue(modifier, value);
                            break;
                        }
                    case TypeCode.Object:
                        {
                            Type valueType = targetProxy.Type;
                            if (valueType.Equals(typeof(Vector2)))
                            {
                                var value = obtainable.GetValue<Vector2>();
                                SetSourceValue(modifier, value);
                            }
                            else if (valueType.Equals(typeof(Vector3)))
                            {
                                var value = obtainable.GetValue<Vector3>();
                                SetSourceValue(modifier, value);
                            }
                            else if (valueType.Equals(typeof(Vector4)))
                            {
                                var value = obtainable.GetValue<Vector4>();
                                SetSourceValue(modifier, value);
                            }
                            else if (valueType.Equals(typeof(Color)))
                            {
                                var value = obtainable.GetValue<Color>();
                                SetSourceValue(modifier, value);
                            }
                            else if (valueType.Equals(typeof(Rect)))
                            {
                                var value = obtainable.GetValue<Rect>();
                                SetSourceValue(modifier, value);
                            }
                            else if (valueType.Equals(typeof(Quaternion)))
                            {
                                var value = obtainable.GetValue<Quaternion>();
                                SetSourceValue(modifier, value);
                            }
                            else if (valueType.Equals(typeof(Version)))
                            {
                                var value = obtainable.GetValue<Version>();
                                SetSourceValue(modifier, value);
                            }
                            else
                            {
                                var value = obtainable.GetValue();
                                SetSourceValue(modifier, value);
                            }
                            break;
                        }
                    default:
                        {
                            var value = obtainable.GetValue();
                            SetSourceValue(modifier, value);
                            break;
                        }
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                isUpdatingSource = false;
            }
        }

        protected void SetTargetValue<T>(IModifiable modifier, T value)
        {
            if (converter == null && typeof(T).Equals(targetProxy.Type))
            {
                modifier.SetValue(value);
                return;
            }

            object safeValue = value;
            if (converter != null)
                safeValue = converter.Convert(value);

            if (!typeof(UnityEventBase).IsAssignableFrom(targetProxy.Type))
                safeValue = targetProxy.Type.ToSafe(safeValue);

            modifier.SetValue(safeValue);
        }

        protected void SetSourceValue<T>(IModifiable modifier, T value)
        {
            if (converter == null && typeof(T).Equals(sourceProxy.Type))
            {
                modifier.SetValue(value);
                return;
            }

            object safeValue = value;
            if (converter != null)
                safeValue = converter.ConvertBack(safeValue);

            safeValue = sourceProxy.Type.ToSafe(safeValue);
            modifier.SetValue(safeValue);
        }

        protected override void OnDataContextChanged()
        {
            if (bindingDescription.Source.IsStatic)
                return;

            CreateSourceProxy(DataContext, bindingDescription.Source);
            UpdateDataOnBind();
        }
    }
}

