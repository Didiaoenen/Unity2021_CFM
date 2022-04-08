using System;
using System.Collections.Generic;

using CFM.Log;

namespace CFM.Framework.Binding.Proxy.Sources
{
    public class SourceProxyFactory : ISourceProxyFactory, ISourceProxyFactoryRegistry
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SourceProxyFactory));

        private List<PriorityFactoryPair> factories = new List<PriorityFactoryPair>();

        public ISourceProxy CreateProxy(object source, SourceDescription description)
        {
            return null;
        }

        protected virtual bool TryCreateProxy(object source, SourceDescription description, out ISourceProxy proxy)
        {
            proxy = null;
            return false;
        }

        public void Register(ISourceProxyFactory factory, int priority = 100)
        {

        }

        public void Unregister(ISourceProxyFactory factory)
        {

        }

        struct PriorityFactoryPair
        {
            public int priority;

            public ISourceProxyFactory factory;

            public PriorityFactoryPair(ISourceProxyFactory factory, int priority)
            {
                this.factory = factory;
                this.priority = priority;
            }
        }
    }
}

