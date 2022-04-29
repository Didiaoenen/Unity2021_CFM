using Assembly_CSharp.Assets.Script.Simple.Binding.Registry;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Converters
{
    public interface IConverterRegistry : IKeyValueRegistry<string, IConverter>
    {
    }
}

