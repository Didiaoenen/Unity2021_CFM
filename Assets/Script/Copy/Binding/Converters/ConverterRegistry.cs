using CFM.Framework.Binding.Registry;

namespace CFM.Framework.Binding.Converters
{
    public class ConverterRegistry : KeyValueRegistry<string, IConverter>, IConverterRegistry
    {
        public ConverterRegistry()
        {
            Init();
        }

        protected virtual void Init()
        {
            
        }
    }
}

