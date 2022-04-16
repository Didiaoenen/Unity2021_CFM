using System;
using CFM.Framework.Binding.Reflection;

namespace CFM.Framework.Binding.Parameters
{
    public class ParameterWrapProxyInvoker
    {
        private readonly object commandParameter;

        private readonly IProxyInvoker invoker;

        public ParameterWrapProxyInvoker(IProxyInvoker invoker, object commandParameter)
        {
            if (invoker == null)
                throw new ArgumentNullException("invoker");
            if (commandParameter == null)
                throw new ArgumentNullException("commandParameter");

            this.invoker = invoker;
            this.commandParameter = commandParameter;

            if (!IsValid(invoker))
                throw new ArgumentException("Bind method failed.the parameter types do not match.");
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

