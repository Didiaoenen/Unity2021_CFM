using CFM.Log;
using System;
using System.Reflection;

namespace CFM.Framework.Binding.Reflection
{
    public class ProxyPropertyInfo : IProxyPropertyInfo
    {
        private readonly bool isValueType;

        private TypeCode typeCode;

        protected PropertyInfo propertyInfo;

        protected MethodInfo getMethod;

        protected MethodInfo setMethod;

        public ProxyPropertyInfo(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");

            this.propertyInfo = propertyInfo;
            isValueType = this.propertyInfo.DeclaringType.IsValueType;

            if (this.propertyInfo.CanRead)
                getMethod = propertyInfo.GetGetMethod();

            if (this.propertyInfo.CanWrite && !isValueType)
                setMethod = propertyInfo.GetSetMethod();
        }

        public virtual bool IsValueType { get { return isValueType; } }

        public virtual Type ValueType { get { return propertyInfo.PropertyType; } }

        public TypeCode ValueTypeCode
        {
            get
            {
                if (typeCode == TypeCode.Empty)
                {
                    typeCode = Type.GetTypeCode(ValueType);
                }
                return typeCode;
            }
        }

        public virtual Type DeclaringType { get { return propertyInfo.DeclaringType; } }

        public virtual string Name { get { return propertyInfo.Name; } }

        public virtual bool IsStatic { get { return propertyInfo.IsStatic(); } }

        public virtual object GetValue(object target)
        {
            if (getMethod == null)
                throw new MemberAccessException();

            return getMethod.Invoke(target, null);
        }

        public virtual void SetValue(object target, object value)
        {
            if (!propertyInfo.CanWrite)
                throw new MemberAccessException("The value is read-only.");

            if (IsValueType)
                throw new NotSupportedException("Assignments of Value type are not supported.");

            if (setMethod == null)
                throw new MemberAccessException("The value is read-only.");

            setMethod.Invoke(target, new object[] { value });
        }
    }

    public class ProxyPropertyInfo<T, TValue> : ProxyPropertyInfo, IProxyPropertyInfo<T, TValue>
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ProxyPropertyInfo<T, TValue>));

        private Func<T, TValue> getter;

        private Action<T, TValue> setter;

        public ProxyPropertyInfo(string propertyName) : this(typeof(T).GetProperty(propertyName))
        {
        }

        public ProxyPropertyInfo(PropertyInfo propertyInfo) : base(propertyInfo)
        {
            if (!typeof(TValue).Equals(this.propertyInfo.PropertyType) || !propertyInfo.DeclaringType.IsAssignableFrom(typeof(T)))
                throw new ArgumentException("The property types do not match!");

            if (IsStatic)
                throw new ArgumentException("The property is static!");

            getter = MakeGetter(propertyInfo);
            setter = MakeSetter(propertyInfo);
        }

        public ProxyPropertyInfo(string propertyName, Func<T, TValue> getter, Action<T, TValue> setter) : this(typeof(T).GetProperty(propertyName), getter, setter)
        {
        }

        public ProxyPropertyInfo(PropertyInfo propertyInfo, Func<T, TValue> getter, Action<T, TValue> setter) : base(propertyInfo)
        {
            if (!typeof(TValue).Equals(this.propertyInfo.PropertyType) || !propertyInfo.DeclaringType.IsAssignableFrom(typeof(T)))
                throw new ArgumentException("The property types do not match!");

            if (IsStatic)
                throw new ArgumentException("The property is static!");

            this.getter = getter;
            this.setter = setter;
        }

        public override Type DeclaringType { get { return typeof(T); } }

        private Action<T, TValue> MakeSetter(PropertyInfo propertyInfo)
        {
            try
            {
                if (IsValueType)
                    return null;

                var setMethod = propertyInfo.GetSetMethod();
                if (setMethod == null)
                    return null;

                return (Action<T, TValue>)setMethod.CreateDelegate(typeof(Action<T, TValue>));
            }
            catch (Exception e)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("{0}", e);
            }

            return null;
        }

        private Func<T, TValue> MakeGetter(PropertyInfo propertyInfo)
        {
            try
            {
                if (IsValueType)
                    return null;

                var getMethod = propertyInfo.GetGetMethod();
                if (getMethod == null)
                    return null;

                return (Func<T, TValue>)getMethod.CreateDelegate(typeof(Func<T, TValue>));
            }
            catch (Exception e)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("{0}", e);
            }
            return null;
        }

        public TValue GetValue(T target)
        {
            if (getter != null)
                return getter(target);

            return (TValue)base.GetValue(target);
        }

        TValue IProxyPropertyInfo<TValue>.GetValue(object target)
        {
            return GetValue((T)target);
        }

        public override object GetValue(object target)
        {
            if (getter != null)
                return getter((T)target);

            return base.GetValue(target);
        }

        public void SetValue(T target, TValue value)
        {
            if (IsValueType)
                throw new NotSupportedException("Assignments of Value type are not supported.");

            if (setter != null)
            {
                setter(target, value);
                return;
            }

            base.SetValue(target, value);
        }

        public void SetValue(object target, TValue value)
        {
            SetValue((T)target, value);
        }

        public override void SetValue(object target, object value)
        {
            if (IsValueType)
                throw new NotSupportedException("Assignments of Value type are not supported.");

            if (setter != null)
            {
                setter((T)target, (TValue)value);
                return;
            }

            base.SetValue(target, value);
        }
    }
}

