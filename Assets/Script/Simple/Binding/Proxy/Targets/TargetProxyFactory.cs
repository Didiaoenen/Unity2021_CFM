using System;
using System.Collections.Generic;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Targets
{
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

    public class TargetProxyFactory : ITargetProxyFactory, ITargetProxyFactoryRegister
    {
        private List<PriorityFactoryPair> factories = new List<PriorityFactoryPair>();

        public ITargetProxy CreateProxy(object target, BindingDescription description)
        {
            try
            {
                ITargetProxy proxy = null;
                if (TryCreateProxy(target, description, out proxy))
                    return proxy;

                throw new NotSupportedException("");
            }
            catch (Exception e)
            {
                throw new Exception();
            }
        }

        protected virtual bool TryCreateProxy(object target, BindingDescription description, out ITargetProxy proxy)
        {
            proxy = null;
            foreach (var pair in factories)
            {
                var factory = pair.factory;
                if (factory == null)
                    continue;

                try
                {
                    proxy = factory.CreateProxy(target, description);
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

            return false;
        }

        public void Register(ITargetProxyFactory factory, int priority = 100)
        {
            factories.Add(new PriorityFactoryPair(factory, priority));
            factories.Sort((x, y) => y.priority.CompareTo(x.priority));
        }

        public void Unregister(ITargetProxyFactory factory)
        {
            factories.RemoveAll(pair => pair.factory == factory);
        }
    }
}

