using UnityEngine.Events;

using CFM.Framework.Binding.Reflection;
using System;

namespace CFM.Framework.Binding.Proxy.Targets
{
    public class UnityTargetProxyFactory : ITargetProxyFactory
    {
        public ITargetProxy CreateProxy(object target, BindingDescription description)
        {
            throw new System.NotImplementedException();
        }

        protected virtual ITargetProxy CreateUnityPropertyProxy(object target, IProxyPropertyInfo propertyInfo, UnityEventBase updateTrigger)
        {
            return null;
        }

        protected virtual ITargetProxy CreateUnityFieldProxy(object target, IProxyFieldInfo fieldInfo, UnityEventBase updateTrigger)
        {
            return null;
        }

        protected virtual ITargetProxy CreateUnityEventProxy(object target, UnityEventBase unityEvent, Type[] paramTypes)
        {
            return null;
        }

        protected Type[] GetUnityEventParametersType(Type type)
        {
            return null;
        }
    }
}

