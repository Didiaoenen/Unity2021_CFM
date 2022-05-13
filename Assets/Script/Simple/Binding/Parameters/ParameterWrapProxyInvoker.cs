using System;
using Assembly_CSharp.Assets.Script.Simple.Binding.Reflection;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Parameters
{
    public class ParameterWrapProxyInvoker : IInvoker
    {
        private readonly object commandParameter;

        private readonly IProxyInvoker invoker;

        public ParameterWrapProxyInvoker(IProxyInvoker invoker, object commandParameter)
        {
            if (invoker == null)
                throw new ArgumentNullException(nameof(invoker));
            if (commandParameter == null)
                throw new ArgumentNullException(nameof(commandParameter));

            this.invoker = invoker;
            this.commandParameter = commandParameter;

            if (!IsValid(invoker))
                throw new ArgumentException();
        }

        public object Invoke(params object[] args)
        {
            return invoker.Invoke(commandParameter);
        }

        protected bool IsValid(IProxyInvoker invoker)
        {
            IProxyMethodInfo info = invoker.ProxyMethodInfo;
            if (!info.ReturnType.Equals(typeof(void)))
                return false;

            var parameters = info.Parameters;
            if (parameters == null || parameters.Length != 1)
                return false;

            return parameters[0].ParameterType.IsAssignableFrom(commandParameter.GetType());
        }
    }
}

