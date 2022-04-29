using System;
using System.Collections.Generic;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources
{
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

    public class SourceProxyFactory : ISourceProxyFactory, ISourceProxyFactoryRegistry
    {
        private List<PriorityFactoryPair> factories = new List<PriorityFactoryPair>();

        public ISourceProxy CreateProxy(object source, SourceDescription description)
        {
            try
            {
                if (!description.IsStatic && source == null)
                    return new EmptySourceProxy(description);

                ISourceProxy proxy = null;
                if (TryCreateProxy(source, description, out proxy))
                    return proxy;

                throw new NotSupportedException();
            }
            catch (Exception e)
            {
                throw new Exception();
            }
        }

        protected virtual bool TryCreateProxy(object source, SourceDescription description, out ISourceProxy proxy)
        {
            proxy = null;
            foreach (var pair in factories)
            {
                var factory = pair.factory;
                if (factory == null)
                    continue;

                try
                {
                    proxy = factory.CreateProxy(source, description);
                    if (proxy != null)
                        return true;
                }
                catch (MissingMemberException e)
                {
                    throw e;
                }
                catch (NullReferenceException e)
                {
                    throw e;
                }
                catch (Exception e)
                {

                }
            }

            proxy = null;
            return false;
        }

        public void Register(ISourceProxyFactory factory, int priority = 100)
        {
            factories.Add(new PriorityFactoryPair(factory, priority));
            factories.Sort((x, y) => y.priority.CompareTo(x.priority));
        }

        public void Unregister(ISourceProxyFactory factory)
        {
            factories.RemoveAll(pair => pair.factory == factory);
        }
    }
}

