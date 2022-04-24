using System;
using System.Collections.Generic;

namespace CFM.Framework.Binding.Reflection
{
    public class ProxyFactory : IProxyFactory
    {
        public static readonly ProxyFactory Default = new ProxyFactory();

        private readonly object _lock = new object();

        private readonly Dictionary<Type, ProxyType> types = new Dictionary<Type, ProxyType>();

        public IProxyType Get(Type type)
        {
            return GetType(type);
        }

        protected virtual IProxyType GetType(Type type)
        {
            ProxyType ret;
            if (types.TryGetValue(type, out ret) && ret != null)
                return ret;

            return Create(type);
        }

        public void Register(IProxyMemberInfo proxyMemberInfo)
        {
            if (proxyMemberInfo == null)
                return;

            ProxyType proxyType = (ProxyType)GetType(proxyMemberInfo.DeclaringType);
            proxyType.Register(proxyMemberInfo);
        }

        public void Unregister(IProxyMemberInfo proxyMemberInfo)
        {
            if (proxyMemberInfo == null)
                return;

            ProxyType proxyType = (ProxyType)GetType(proxyMemberInfo.DeclaringType);
            proxyType.Unregister(proxyMemberInfo);
        }

        public IProxyType Create(Type type)
        {
            lock (_lock)
            {
                ProxyType proxyType = new ProxyType(type, this);
                types.Add(type, proxyType);
                return proxyType;
            }
        }
    }
}

