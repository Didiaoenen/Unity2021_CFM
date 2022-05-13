using System;
using System.Reflection;
using System.Collections.Generic;
using Assembly_CSharp.Assets.Script.Simple.Binding.Reflection;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Parameters
{
    public class ParameterWrapDelegateInvoker : IInvoker
    {
        private readonly object commandParameter;

        private readonly Delegate handler;

        public ParameterWrapDelegateInvoker(Delegate handler, object commandParameter)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));
            if (commandParameter == null)
                throw new ArgumentNullException(nameof(commandParameter));

            this.handler = handler;
            this.commandParameter = commandParameter;

            if (!IsValid(handler))
                throw new ArgumentException();
        }

        public object Invoke(params object[] args)
        {
            return handler.DynamicInvoke(commandParameter);
        }

        protected virtual bool IsValid(Delegate handler)
        {
            MethodInfo info = handler.Method;
            if (!info.ReturnType.Equals(typeof(void)))
                return false;

            List<Type> parameterTypes = info.GetParameterTypes();
            if (parameterTypes.Count != 1)
                return false;

            return parameterTypes[0].IsAssignableFrom(commandParameter.GetType());
        }
    }
}

