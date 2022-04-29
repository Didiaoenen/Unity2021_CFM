using System;
using Assembly_CSharp.Assets.Script.Simple.Binding.Reflection;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources.Object
{
    public class FieldNodeProxy : SourceProxyBase, IObtainable, IModifiable
    {
        protected IProxyFieldInfo fieldInfo;

        public override Type Type { get { return fieldInfo.ValueType; } }

        public override TypeCode TypeCode { get { return fieldInfo.ValueTypeCode; } }


        public FieldNodeProxy(IProxyFieldInfo fieldInfo) : this(null, fieldInfo)
        {
        }

        public FieldNodeProxy(object source, IProxyFieldInfo fieldInfo) : base(source)
        {
            this.fieldInfo = fieldInfo;
        }

        public virtual object GetValue()
        {
            return fieldInfo.GetValue(source);
        }

        public virtual TValue GetValue<TValue>()
        {
            var proxy = fieldInfo as IProxyFieldInfo<TValue>;
            if (proxy != null)
                return proxy.GetValue(source);

            return (TValue)fieldInfo.GetValue(source);
        }

        public void SetValue(object value)
        {
            fieldInfo.SetValue(source, value);
        }

        public void SetValue<TValue>(TValue value)
        {
            var proxy = fieldInfo as IProxyFieldInfo<TValue>;
            if (proxy != null)
            {
                proxy.SetValue(source, value);
                return;
            }

            fieldInfo.SetValue(source, value);
        }
    }
}

