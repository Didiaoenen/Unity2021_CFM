using CFM.Framework.Binding.Registry;

namespace CFM.Framework.Binding.Converters
{
    public interface IConverterRegistry : IKeyValueRegistry<string, IConverter>
    {
    }
}

