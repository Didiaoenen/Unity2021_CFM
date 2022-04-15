using System;

using CFM.Framework.Commands;
using CFM.Framework.Binding.Proxy;
using CFM.Framework.Binding.Converters;
using CFM.Framework.Binding.Reflection;

namespace CFM.Framework.Binding.Parameters
{
    public class ParameterWrapConverter : AbstractConverter
    {
        private readonly object commandParameter;
        public ParameterWrapConverter(object commandParameter)
        {
            if (commandParameter == null)
                throw new ArgumentNullException("commandParameter");

            this.commandParameter = commandParameter;
        }

        public override object Convert(object value)
        {
            if (value == null)
                return null;

            if (value is Delegate)
                return new ParameterWrapDelegateInvoker(value as Delegate, commandParameter);

            if (value is ICommand)
                return new ParameterWrapCommand(value as ICommand, commandParameter);

            if (value is IScriptInvoker)
                return new ParameterWrapScriptInvoker(value as IScriptInvoker, commandParameter);

            if (value is IProxyInvoker)
                return new ParameterWrapProxyInvoker(value as IProxyInvoker, commandParameter);

            throw new NotSupportedException(string.Format("Unsupported type \"{0}\".", value.GetType()));
        }

        public override object ConvertBack(object value)
        {
            throw new NotSupportedException();
        }
    }
}

