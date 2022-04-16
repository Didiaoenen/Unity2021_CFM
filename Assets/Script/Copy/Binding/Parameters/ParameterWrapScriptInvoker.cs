using System;

using CFM.Framework.Binding.Proxy;

namespace CFM.Framework.Binding.Parameters
{
    public class ParameterWrapScriptInvoker
    {
        private readonly object commandParameter;

        private readonly IScriptInvoker invoker;

        public ParameterWrapScriptInvoker(IScriptInvoker invoker, object commandParameter)
        {
            if (invoker == null)
                throw new ArgumentNullException("invoker");
            if (commandParameter == null)
                throw new ArgumentNullException("commandParameter");

            this.invoker = invoker;
            this.commandParameter = commandParameter;
        }

        public object Invoke(params object[] args)
        {
            return invoker.Invoke(commandParameter);
        }
    }
}

