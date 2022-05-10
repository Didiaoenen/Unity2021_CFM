using System;
using Assembly_CSharp.Assets.Script.Simple.Binding.Reflection;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Targets.Universal
{
    public class FieldTargetProxy : ValueTargetProxyBase
    {
        protected readonly IProxyFieldInfo fieldInfo;

        public override Type Type { get { return fieldInfo.ValueType; } }

        public override TypeCode TypeCode { get { return fieldInfo.ValueTypeCode; } }

        public override BindingMode DefaultMode { get { return BindingMode.OneWay; } }

        public FieldTargetProxy(object target, IProxyFieldInfo fieldInfo) : base(target)
        {
            this.fieldInfo = fieldInfo;
        }

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
                return (fieldInfo as IProxyFieldInfo<TValue>).GetValue(target);

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
                (fieldInfo as IProxyFieldInfo<TValue>).SetValue(target, value);
                return;
            }

            fieldInfo.SetValue(target, value);
        }
    }
}

