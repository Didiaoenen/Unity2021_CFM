using System;
using System.Reflection;
using System.Collections.Generic;
using CFM.Framework.Binding.Reflection;

namespace CFM.Framework.Binding.Parameters
{
    public class ParameterWrapDelegateInvoker : IInvoker
    {
        private readonly object commandParameter;

        private readonly Delegate handler;

        public ParameterWrapDelegateInvoker(Delegate handler, object commandParameter)
        {
            if (handler == null)
                throw new ArgumentNullException("handler");
            if (commandParameter == null)
                throw new ArgumentNullException("commandParameter");

            this.handler = handler;
            this.commandParameter = commandParameter;

            if (!IsValid(handler))
                throw new ArgumentException("Bind methed failed, the parameter types do not match.");
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

