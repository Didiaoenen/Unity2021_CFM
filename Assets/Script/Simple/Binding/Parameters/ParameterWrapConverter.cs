using System;
using Assembly_CSharp.Assets.Script.Simple.Commands;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy;
using Assembly_CSharp.Assets.Script.Simple.Binding.Reflection;
using Assembly_CSharp.Assets.Script.Simple.Binding.Converters;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Parameters
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
        
            return new NotSupportedException(string.Format("{0}", value.GetType()));
        }

        public override object ConvertBack(object value)
        {
            return new NotSupportedException(); 
        }
    }
}

