using System;
using System.Reflection;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Reflection
{
    public class ProxyFieldInfo : IProxyFieldInfo
    {
        private readonly bool isValueType;

        private TypeCode typeCode;
        
        protected FieldInfo fieldInfo;

        public ProxyFieldInfo(FieldInfo fieldInfo)
        {
        }

        public virtual bool IsValueType { get { return isValueType; } }

        public Type ValueType => throw new NotImplementedException();

        public TypeCode ValueTypeCode => throw new NotImplementedException();

        public Type DeclaringType => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public bool IsStatic => throw new NotImplementedException();

        public object GetValue(object target)
        {
            throw new NotImplementedException();
        }

        public void SetValue(object target, object value)
        {
            throw new NotImplementedException();
        }
    }

    public class ProxyFieldInfo<T, TValue> : ProxyFieldInfo, IProxyFieldInfo<T, TValue>
    {
        private Func<T, TValue> getter;

        private Action<T, TValue> setter;

        public ProxyFieldInfo(FieldInfo fieldInfo) : base(fieldInfo)
        {
        }

        public TValue GetValue(T target)
        {
            throw new NotImplementedException();
        }

        public void SetValue(T target, TValue value)
        {
            throw new NotImplementedException();
        }

        public void SetValue(object target, TValue value)
        {
            throw new NotImplementedException();
        }

        TValue IProxyFieldInfo<TValue>.GetValue(object target)
        {
            throw new NotImplementedException();
        }
    }
}

