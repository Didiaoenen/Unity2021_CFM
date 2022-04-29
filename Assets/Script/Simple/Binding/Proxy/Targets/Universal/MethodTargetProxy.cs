using System;
using UnityEngine;
using Assembly_CSharp.Assets.Script.Simple.Binding.Reflection;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Targets.Universal
{
    public class MethodTargetProxy : TargetProxyBase, IObtainable, IProxyInvoker
    {
        protected readonly IProxyMethodInfo methodInfo;

        protected IProxyInvoker invoker;

        public MethodTargetProxy(object target, IProxyMethodInfo methodInfo) : base(target)
        {
            this.methodInfo = methodInfo;
            if (!methodInfo.ReturnType.Equals(typeof(void)))
                throw new ArgumentException(nameof(methodInfo));

            invoker = this;
        }

        public IProxyMethodInfo ProxyMethodInfo { get { return methodInfo; } }

        public override Type Type { get { return typeof(IProxyInvoker); } }

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

