using System;
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
            return base.Convert(value);
        }

        public override object ConvertBack(object value)
        {
            return new NotSupportedException(); 
        }
    }
}

