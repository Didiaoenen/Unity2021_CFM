using System;
using System.ComponentModel;
using Assembly_CSharp.Assets.Script.Simple.Binding.Reflection;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Targets.Universal
{
    public class PropertyTargetProxy : ValueTargetProxyBase
    {
        protected readonly IProxyPropertyInfo propertyInfo;

        public override Type Type { get { return propertyInfo.ValueType; } }

        public override TypeCode TypeCode { get { return propertyInfo.ValueTypeCode; } }

        public override BindingMode DefaultMode { get { return BindingMode.TwoWay; } }

        public PropertyTargetProxy(object target, IProxyPropertyInfo propertyInfo) : base(target)
        {
            this.propertyInfo = propertyInfo;
        }

        public override object GetValue()
        {
            var target = Target;
            if (target == null)
                return null;

            return propertyInfo.GetValue(target);
        }

        public override TValue GetValue<TValue>()
        {
            var target = Target;
            if (target == null)
                return default(TValue);

            if (propertyInfo is IProxyPropertyInfo<TValue>)
                return ((IProxyPropertyInfo<TValue>)propertyInfo).GetValue(target);

            return (TValue)propertyInfo.GetValue(target);
        }

        public override void SetValue(object value)
        {
            var target = Target;
            if (target == null)
                return;

            propertyInfo.SetValue(target, value);
        }

        public override void SetValue<TValue>(TValue value)
        {
            var target = Target;
            if (target == null)
                return;

            if (propertyInfo is IProxyPropertyInfo<TValue>)
            {
                ((IProxyPropertyInfo<TValue>)propertyInfo).SetValue(target, value);
                return;
            }

            propertyInfo.SetValue(target, value);
        }

        protected override void DoSubscribeForValueChange(object target)
        {
            if (target is INotifyPropertyChanged)
            {
                var targetNotify = target as INotifyPropertyChanged;
                targetNotify.PropertyChanged += OnPropertyChanged;
            }
        }

        protected override void DoUnsubscribeForValueChange(object target)
        {
            if (target is INotifyPropertyChanged)
            {
                var targetNotify = target as INotifyPropertyChanged;
                targetNotify.PropertyChanged -= OnPropertyChanged;
            }
        }

        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var name = e.PropertyName;
            if (string.IsNullOrEmpty(name) || name.Equals(this.propertyInfo.Name))
            {
                var target = Target;
                if (target == null)
                    return;

                RaiseValueChanged();
            }
        }
    }
}

