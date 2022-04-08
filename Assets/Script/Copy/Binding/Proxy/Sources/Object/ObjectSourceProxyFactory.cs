using System.Collections.Generic;

using CFM.Framework.Binding.Paths;

namespace CFM.Framework.Binding.Proxy.Sources.Object
{
    public class ObjectSourceProxyFactory: TypedSourceProxyFactory<ObjectSourceDescription>, INodeProxyFactory, INodeProxyFactoryRegister
    {
        private List<PriorityFactoryPair> factorise = new List<PriorityFactoryPair>();

        protected override bool TryCreateProxy(object source, ObjectSourceDescription description, out ISourceProxy proxy)
        {
            proxy = null;
            return true;
        }

        public virtual ISourceProxy Create(object source, PathToken token)
        {
            return null;
        }

        public virtual void Register(INodeProxyFactory factory, int priority = 100)
        {

        }

        public virtual void Unregister(INodeProxyFactory factory)
        {

        }

        struct PriorityFactoryPair
        {
            public int priority;
            public INodeProxyFactory Factory;

            public PriorityFactoryPair(INodeProxyFactory factory, int priority)
            {
                this.Factory = factory;
                this.priority = priority;
            }
        }
    }
}

