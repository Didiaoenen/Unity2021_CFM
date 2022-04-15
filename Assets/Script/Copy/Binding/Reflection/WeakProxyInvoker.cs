using System;

namespace CFM.Framework.Binding.Reflection
{
    public class WeakProxyInvoker : IProxyInvoker
    {
        private WeakReference target;

        private IProxyMethodInfo proxyMethodInfo;

        public WeakProxyInvoker(WeakReference target, IProxyMethodInfo proxyMethodInfo)
        {
            this.target = target;
            this.proxyMethodInfo = proxyMethodInfo;
        }

        public virtual IProxyMethodInfo ProxyMethodInfo { get { return proxyMethodInfo; } }

        public object Invoke(params object[] args)
        {
            if (proxyMethodInfo.IsStatic)
                return proxyMethodInfo.Invoke(null, args);

            if (target == null || !target.IsAlive)
                return null;

            var obj = target.Target;
            if (obj == null)
                return null;

            return proxyMethodInfo.Invoke(obj, args);
        }
    }
}

