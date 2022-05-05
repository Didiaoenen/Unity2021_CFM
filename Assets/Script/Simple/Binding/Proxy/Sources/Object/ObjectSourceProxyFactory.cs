using System;
using System.Collections.Generic;
using Assembly_CSharp.Assets.Script.Simple.Binding.Paths;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources.Object
{
    struct PriorityFactoryPair
    {
        public int priority;

        public INodeProxyFactory factory;

        public PriorityFactoryPair(INodeProxyFactory factory, int priority)
        {
            this.factory = factory;
            this.priority = priority;
        }
    }

    public class ObjectSourceProxyFactory : TypedSourceProxyFactory<ObjectSourceDescription>, INodeProxyFactory, INodeProxyFactoryRegister
    {
        private List<PriorityFactoryPair> factories = new List<PriorityFactoryPair>();

        public ISourceProxy Create(object source, PathToken token)
        {
            ISourceProxy proxy = null;
            foreach (var pair in factories)
            {
                var factory = pair.factory;
                if (factory == null)
                    continue;

                proxy = factory.Create(source, token);
                if (proxy != null)
                    return proxy;
            }
            return proxy;
        }

        protected override bool TryCreateProxy(object source, ObjectSourceDescription description, out ISourceProxy proxy)
        {
            proxy = null;
            var path = description.Path;
            if (path.Count <= 0)
                throw new Exception();

            PathToken token = path.AsPathToken();
            if (path.Count == 1)
            {
                proxy = Create(source, token);
                if (proxy != null)
                    return true;
                return false;
            }

            proxy = new ChainedObjectSourceProxy(source, token, this);
            return true;
        }

        public void Register(INodeProxyFactory factory, int priority = 100)
        {
            factories.Add(new PriorityFactoryPair(factory, priority));
            factories.Sort((x, y) => y.priority.CompareTo(x.priority));
        }

        public void Unregister(INodeProxyFactory factory)
        {
            factories.RemoveAll(pair => pair.factory == factory);
        }
    }
}

