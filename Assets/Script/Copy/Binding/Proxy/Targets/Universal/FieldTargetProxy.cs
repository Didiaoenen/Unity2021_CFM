using System;
using CFM.Framework.Binding.Reflection;

namespace CFM.Framework.Binding.Proxy.Targets.Universal
{
    public class FieldTargetProxy : ValueTargetProxyBase
    {
        protected readonly IProxyFieldInfo fieldInfo;

        public FieldTargetProxy(object target, IProxyFieldInfo fieldInfo) : base(target)
        {
            this.fieldInfo = fieldInfo;
        }

        public override Type Type { get { return fieldInfo.ValueType; } }

        public override TypeCode TypeCode { get { return fieldInfo.ValueTypeCode; } }

        public override BindingMode DefaultMode { get { return BindingMode.OneWay; } }

        public override object GetValue()
        {
            var target = Target;
            if (target == null)
                return null;

            return fieldInfo.GetValue(target);
        }

        public override TValue GetValue<TValue>()
        {
            var target = Target;
            if (target == null)
                return default(TValue);

            if (fieldInfo is IProxyFieldInfo<TValue>)
                return ((IProxyFieldInfo<TValue>)fieldInfo).GetValue(target);

            return (TValue)fieldInfo.GetValue(target);
        }

        public override void SetValue(object value)
        {
            var target = Target;
            if (target == null)
                return;

            fieldInfo.SetValue(target, value);
        }

        public override void SetValue<TValue>(TValue value)
        {
            var target = Target;
            if (target == null)
                return;

            if (fieldInfo is IProxyFieldInfo<TValue>)
            {
                ((IProxyFieldInfo<TValue>)fieldInfo).SetValue(target, value);
                return;
            }

            fieldInfo.SetValue(target, value);
        }
    }
}
