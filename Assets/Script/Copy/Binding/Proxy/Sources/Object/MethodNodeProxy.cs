using System;

using CFM.Framework.Binding.Reflection;

namespace CFM.Framework.Binding.Proxy.Sources.Object
{
    public class MethodNodeProxy : SourceProxyBase, IObtainable
    {
        protected IProxyMethodInfo methodInfo;

        protected IProxyInvoker invoker;

        public MethodNodeProxy(IProxyMethodInfo methodInfo) : this(null, methodInfo)
        {
        }

        public MethodNodeProxy(object source, IProxyMethodInfo methodInfo) : base(source)
        {
            this.methodInfo = methodInfo;
            invoker = new ProxyInvoker(this.source, this.methodInfo);
        }

        public override Type Type { get { return typeof(IProxyInvoker); } }

        public object GetValue()
        {
            return invoker;
        }

        public TValue GetValue<TValue>()
        {
            return (TValue)invoker;
        }
    }
}
