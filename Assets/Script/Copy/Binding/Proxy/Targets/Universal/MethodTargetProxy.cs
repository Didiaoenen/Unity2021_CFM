using System;
using UnityEngine;
using CFM.Framework.Binding.Reflection;

namespace CFM.Framework.Binding.Proxy.Targets.Universal
{
    public class MethodTargetProxy : TargetProxyBase, IObtainable, IProxyInvoker
    {
        protected readonly IProxyMethodInfo methodInfo;
        protected IProxyInvoker invoker;
        public MethodTargetProxy(object target, IProxyMethodInfo methodInfo) : base(target)
        {
            this.methodInfo = methodInfo;
            if (!methodInfo.ReturnType.Equals(typeof(void)))
                throw new ArgumentException("methodInfo");

            invoker = this;// new WeakProxyInvoker(new WeakReference(target, false), methodInfo);
        }

        public override BindingMode DefaultMode { get { return BindingMode.OneWayToSource; } }

        public override Type Type { get { return typeof(IProxyInvoker); } }

        public IProxyMethodInfo ProxyMethodInfo { get { return this.methodInfo; } }

        public object GetValue()
        {
            return invoker;
        }

        public TValue GetValue<TValue>()
        {
            return (TValue)invoker;
        }

        public object Invoke(params object[] args)
        {
            if (methodInfo.IsStatic)
                return methodInfo.Invoke(null, args);

            var obj = Target;
            if (obj == null)
                return null;

            if (obj is Behaviour behaviour && !behaviour.isActiveAndEnabled)
                return null;

            return methodInfo.Invoke(obj, args);
        }
    }
}
