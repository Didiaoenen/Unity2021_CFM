namespace CFM.Framework.Binding.Reflection
{
    public class ProxyInvoker : IProxyInvoker
    {
        private object target;

        private IProxyMethodInfo proxyMethodInfo;

        public ProxyInvoker(object target, IProxyMethodInfo proxyMethodInfo)
        {
            this.target = target;
            this.proxyMethodInfo = proxyMethodInfo;
        }

        public virtual IProxyMethodInfo ProxyMethodInfo { get { return proxyMethodInfo; } }

        public object Invoke(params object[] args)
        {
            return proxyMethodInfo.Invoke(target, args);
        }
    }
}

