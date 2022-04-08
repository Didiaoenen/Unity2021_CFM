using System;
using System.Collections.Generic;

using CFM.Log;

namespace CFM.Framework.Binding.Proxy.Targets
{
    public class TargetProxyFactory : ITargetProxyFactory, ITargetProxyFactoryRegister
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TargetProxyFactory));

        private List<PriorityFactoryPair> factories = new List<PriorityFactoryPair>();

        public ITargetProxy CreateProxy(object target, BindingDescription description)
        {
            ITargetProxy proxy = null;
            return proxy;
        }

        protected virtual bool TryCreateProxy(object target, BindingDescription description, out ITargetProxy proxy)
        {
            proxy = null;
            return false;
        }

        public void Register(ITargetProxyFactory factory, int priority = 100)
        {

        }

        public void Unregister(ITargetProxyFactory factory)
        {

        }

        struct PriorityFactoryPair
        {
            public int priority;

            public ITargetProxyFactory factory;

            public PriorityFactoryPair(ITargetProxyFactory factory, int priority)
            {
                this.factory = factory;
                this.priority = priority;
            }
        }
    }
}

