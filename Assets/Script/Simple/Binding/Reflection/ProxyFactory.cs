using System;
using System.Collections.Generic;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Reflection
{
    public class ProxyFactory
    {
        public static readonly ProxyFactory Default = new ProxyFactory();

        private readonly object _lock = new object();

        private readonly Dictionary<Type, ProxyType> types = new Dictionary<Type, ProxyType>();

        public IProxyType Get(Type type)
        {
            return GetType(type);
        }

        protected virtual ProxyType GetType(Type type)
        {
            ProxyType ret;
            if (types.TryGetValue(type, out ret) && ret != null)
                return ret;

            return Create(type);
        }

        protected ProxyType Create(Type type)
        {
            lock (_lock)
            {
                ProxyType proxyType = new ProxyType(type, this);
                types.Add(type, proxyType);
                return proxyType;
            }
        }

        public void Register(IProxyMemberInfo proxyMemberInfo)
        {
            if (proxyMemberInfo == null)
                return;

            ProxyType proxyType = GetType(proxyMemberInfo.DeclaringType);
            proxyType.Register(proxyMemberInfo);
        }

        public void Unregister(IProxyMemberInfo proxyMemberInfo)
        {
            if (proxyMemberInfo == null)
                return;

            ProxyType proxyType = GetType(proxyMemberInfo.DeclaringType);
            proxyType.Unregister(proxyMemberInfo);
        }
    }
}

