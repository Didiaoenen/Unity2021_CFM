using System;
using System.Reflection;
using System.Linq.Expressions;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Reflection
{
    public class ProxyFieldInfo : IProxyFieldInfo
    {
        private readonly bool isValueType;

        private TypeCode typeCode;
        
        protected FieldInfo fieldInfo;

        public virtual bool IsValueType { get { return isValueType; } }

        public virtual Type ValueType { get { return fieldInfo.FieldType; } }

        public virtual Type DeclaringType { get { return fieldInfo.DeclaringType; } }

        public virtual string Name { get { return fieldInfo.Name; } }

        public virtual bool IsStatic { get { return fieldInfo.IsStatic; } }
        
        public TypeCode ValueTypeCode
        {
            get
            {
                if (typeCode == TypeCode.Empty)
                    typeCode = Type.GetTypeCode(ValueType);
            
                return typeCode;
            }
        }

        public ProxyFieldInfo(FieldInfo fieldInfo)
        {
            if (fieldInfo == null)
                throw new ArgumentNullException(nameof(fieldInfo));

            this.fieldInfo = fieldInfo;
            isValueType = fieldInfo.DeclaringType.IsValueType;
        }

        public virtual object GetValue(object target)
        {
            return fieldInfo.GetValue(target);
        }

        public virtual void SetValue(object target, object value)
        {
            if (fieldInfo.IsInitOnly)
                throw new MemberAccessException();

            if (IsValueType)
                throw new NotSupportedException();

            fieldInfo.SetValue(target, value);
        }
    }

    public class ProxyFieldInfo<T, TValue> : ProxyFieldInfo, IProxyFieldInfo<T, TValue>
    {
        private Func<T, TValue> getter;

        private Action<T, TValue> setter;

        public override Type DeclaringType { get { return typeof(T); } }

        public ProxyFieldInfo(string fieldName) : this(typeof(T).GetField(fieldName))
        {

        }

        public ProxyFieldInfo(FieldInfo fieldInfo) : base(fieldInfo)
        {
            if (!typeof(TValue).Equals(fieldInfo.FieldType) || !DeclaringType.IsAssignableFrom(typeof(T)))
                throw new ArgumentException();

            getter = MakeGetter(fieldInfo);
            setter = MakeSetter(fieldInfo);
        }

        private Action<T, TValue> MakeSetter(FieldInfo fieldInfo)
        {
            if (IsValueType)
                return null;

            if (fieldInfo.IsInitOnly)
                return null;

            try
            {
                bool expressionSupportRestricted = false;

#if EBABLE_IL2CPP
                expressionSupportRestricted = true;
#endif

                if (!expressionSupportRestricted || !(typeof(T).IsValueType || typeof(TValue).IsValueType))
                {
                    var targetExp = Expression.Parameter(typeof(T), "target");
                    var paramExp = Expression.Parameter(typeof(TValue), "value");
                    var fieldExp = Expression.Field(fieldInfo.IsStatic ? null : targetExp, fieldInfo);
                    var assignExp = Expression.Assign(fieldExp, paramExp);
                    var lambda = Expression.Lambda<Action<T, TValue>>(assignExp, targetExp, paramExp);
                    return lambda.Compile();
                }

            }
            catch (Exception e)
            {
            }
            return null;
        }

        private Func<T, TValue> MakeGetter(FieldInfo fieldInfo)
        {
            try
            {
                bool expressionSupportRestricted = false;

#if ENABLE_IL2CPP
                expressionSupportRestricted = true;
#endif

                if (!expressionSupportRestricted || !(typeof(T).IsValueType || typeof(TValue).IsValueType))
                {
                    var targetExp = Expression.Parameter(typeof(T), "target");
                    var fieldExp = Expression.Field(fieldInfo.IsStatic ? null : targetExp, fieldInfo);
                    var lambda = Expression.Lambda<Func<T, TValue>>(fieldExp, targetExp);
                    return lambda.Compile();
                }
            }
            catch (Exception e)
            {
            }
            return null;
        }

        public ProxyFieldInfo(string fieldName, Func<T, TValue> getter, Action<T, TValue> setter) : this(typeof(T).GetField(fieldName), getter, setter)
        {

        }

        public ProxyFieldInfo(FieldInfo fieldInfo, Func<T, TValue> getter, Action<T, TValue> setter) : base(fieldInfo)
        {
            if (!typeof(TValue).Equals(this.fieldInfo.FieldType) || !DeclaringType.IsAssignableFrom(typeof(T)))
                throw new ArgumentException();

            this.getter = getter;
            this.setter = setter;
        }

        public TValue GetValue(T target)
        {
            if (getter != null)
                return getter(target);
            return (TValue)fieldInfo.GetValue(target);
        }

        public override object GetValue(object target)
        {
            if (getter != null)
                return getter((T)target);
            return fieldInfo.GetValue(target);
        }

        TValue IProxyFieldInfo<TValue>.GetValue(object target)
        {
            return GetValue((T)target);
        }

        public void SetValue(T target, TValue value)
        {
            if (fieldInfo.IsInitOnly)
                throw new MemberAccessException();

            if (IsValueType)
                throw new NotSupportedException();

            if (setter != null)
            {
                setter(target, value);
                return;
            }

            fieldInfo.SetValue(target, value);
        }

        public override void SetValue(object target, object value)
        {
            if (fieldInfo.IsInitOnly)
                throw new MemberAccessException();

            if (IsValueType)
                throw new NotSupportedException();

            if (setter != null)
            {
                setter((T)target, (TValue)value);
                return;
            }

            fieldInfo.SetValue(target, value);
        }

        public void SetValue(object target, TValue value)
        {
            setter((T)target, value);
        }
    }
}

