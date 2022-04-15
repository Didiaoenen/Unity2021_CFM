using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace CFM.Framework.Binding.Reflection
{
    public class ProxyItemInfo : IProxyItemInfo
    {
        private readonly bool isValueType;

        private TypeCode typeCode;

        protected PropertyInfo propertyInfo;

        protected MethodInfo getMethod;

        protected MethodInfo setMethod;

        public ProxyItemInfo(PropertyInfo propertyInfo)
        {

        }

        public bool IsValueType {  get { return isValueType; } }

        public Type ValueType { get { return this.propertyInfo.PropertyType; } }

        public TypeCode ValueTypeCode
        {
            get
            {
                if (typeCode == TypeCode.Empty)
                    typeCode = Type.GetTypeCode(ValueType);
                return typeCode;
            }
        }

        public Type DeclaringType {  get { return propertyInfo.DeclaringType; } }

        public string Name { get { return propertyInfo.Name; } }

        public bool IsStatic { get { return propertyInfo.IsStatic(); } }

        public object GetValue(object target, object key)
        {
            if (target is IList)
            {
                int index = (int)key;
                IList list = target as IList;

                if (index < 0 || index >= list.Count)
                    throw new ArgumentOutOfRangeException("key", string.Format("The index is out of range, the key value is {0}, it is not between 0 and {1}", index, list.Count));

                return list[index];
            }

            if (target is IDictionary)
            {
                IDictionary dict = target as IDictionary;
                if (!dict.Contains(key))
                    return null;

                return dict[key];
            }

            if (getMethod == null)
                throw new MemberAccessException();

            return getMethod.Invoke(target, new object[] { key });
        }

        public void SetValue(object target, object key, object value)
        {
            if (target is IList)
            {
                int index = (int)key;
                IList list = target as IList;

                if (index < 0 || index >= list.Count)
                    throw new ArgumentOutOfRangeException("key", string.Format("The index is out of range, the key value is {0}, it is not between 0 and {1}", index, list.Count));

                list[index] = value;
                return;
            }

            if (target is IDictionary)
            {
                ((IDictionary)target)[key] = value;
                return;
            }

            if (setMethod == null)
                throw new MemberAccessException();

            setMethod.Invoke(target, new object[] { key, value });
        }
    }

    public class ListProxyItemInfo<T, TValue> : ProxyItemInfo, IProxyItemInfo<T, int, TValue> where T : IList<TValue>
    {
        public ListProxyItemInfo(PropertyInfo propertyInfo) : base(propertyInfo)
        {
            if (!typeof(TValue).Equals(this.propertyInfo.PropertyType) || !typeof(IList<TValue>).IsAssignableFrom(propertyInfo.DeclaringType))
                throw new ArgumentException("The property types do not match!");
        }

        public TValue GetValue(T target, int key)
        {
            if (key < 0 || key >= target.Count)
                throw new ArgumentOutOfRangeException("key", string.Format("The index is out of range, the key value is {0}, it is not between 0 and {1}", key, target.Count));

            return target[key];
        }

        public TValue GetValue(object target, int key)
        {
            return GetValue((T)target, key);
        }

        public void SetValue(T target, int key, TValue value)
        {
            if (key < 0 || key >= target.Count)
                throw new ArgumentOutOfRangeException("key", string.Format("The index is out of range, the key value is {0}, it is not between 0 and {1}", key, target.Count));

            target[key] = value;
        }

        public void SetValue(object target, int key, TValue value)
        {
            SetValue((T)target, key, value);
        }
    }

    public class DictionaryProxyItemInfo<T, TKey, TValue> : ProxyItemInfo, IProxyItemInfo<T, TKey, TValue> where T : IDictionary<TKey, TValue>
    {
        public DictionaryProxyItemInfo(PropertyInfo propertyInfo) : base(propertyInfo)
        {
            if (!typeof(TValue).Equals(this.propertyInfo.PropertyType) || !typeof(IDictionary<TKey, TValue>).IsAssignableFrom(propertyInfo.DeclaringType))
                throw new ArgumentException("The property types do not match!");
        }

        public TValue GetValue(T target, TKey key)
        {
            if (!target.ContainsKey(key))
                return default(TValue);

            return target[key];
        }

        public TValue GetValue(object target, TKey key)
        {
            return GetValue((T)target, key);
        }

        public void SetValue(T target, TKey key, TValue value)
        {
            target[key] = value;
        }

        public void SetValue(object target, TKey key, TValue value)
        {
            SetValue((T)target, key, value);
        }
    }

    public class ArrayProxyItemInfo : IProxyItemInfo
    {
        private TypeCode typeCode;

        protected readonly Type type;

        public ArrayProxyItemInfo(Type type)
        {
            this.type = type;
            if (this.type == null || !this.type.IsArray)
                throw new ArgumentException();
        }

        public Type ValueType { get { return type.HasElementType ? type.GetElementType() : typeof(object); } }

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

        public Type DeclaringType { get { return type; } }

        public string Name { get { return "Item"; } }

        public bool IsStatic { get { return false; } }

        public virtual object GetValue(object target, object key)
        {
            int index = (int)key;
            Array array = target as Array;
            if (index < 0 || index >= array.Length)
                throw new ArgumentOutOfRangeException("key", string.Format("The index is out of range, the key value is {0}, it is not between 0 and {1}", index, array.Length));

            return array.GetValue(index);
        }

        public virtual void SetValue(object target, object key, object value)
        {
            int index = (int)key;
            Array array = target as Array;
            if (index < 0 || index >= array.Length)
                throw new ArgumentOutOfRangeException("key", string.Format("The index is out of range, the key value is {0}, it is not between 0 and {1}", index, array.Length));

            array.SetValue(value, index);
        }
    }

    public class ArrayProxyItemInfo<T, TValue> : ArrayProxyItemInfo, IProxyItemInfo<T, int, TValue> where T : IList<TValue>
    {
        public ArrayProxyItemInfo() : base(typeof(T))
        {
        }

        public TValue GetValue(T target, int key)
        {
            if (key < 0 || key >= target.Count)
                throw new ArgumentOutOfRangeException("key", string.Format("The index is out of range, the key value is {0}, it is not between 0 and {1}", key, target.Count));

            return target[key];
        }

        public TValue GetValue(object target, int key)
        {
            return GetValue((T)target, key);
        }

        public void SetValue(T target, int key, TValue value)
        {
            if (key < 0 || key >= target.Count)
                throw new ArgumentOutOfRangeException("key", string.Format("The index is out of range, the key value is {0}, it is not between 0 and {1}", key, target.Count));

            target[key] = value;
        }

        public void SetValue(object target, int key, TValue value)
        {
            SetValue((T)target, key, value);
        }
    }
}

