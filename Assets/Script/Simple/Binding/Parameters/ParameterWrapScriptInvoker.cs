using System;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Parameters
{
    public class ParameterWrapScriptInvoker : IInvoker
    {
        private readonly object commandParameter;

        private readonly IScriptInvoker invoker;
        
        public ParameterWrapScriptInvoker(IScriptInvoker invoker, object commandParameter)
        {
            if (invoker == null)
                throw new ArgumentNullException(nameof(invoker));
            if (commandParameter == null)
                throw new ArgumentNullException(nameof(commandParameter));

            this.invoker = invoker;
            this.commandParameter = commandParameter;
        }

        public object Invoke(params object[] args)
        {
            return invoker.Invoke(commandParameter);
        }
    }
}

