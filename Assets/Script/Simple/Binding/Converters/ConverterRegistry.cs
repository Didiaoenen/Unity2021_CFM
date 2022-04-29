using Assembly_CSharp.Assets.Script.Simple.Binding.Registry;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Converters
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

