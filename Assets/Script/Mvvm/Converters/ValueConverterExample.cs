using UnityEngine;

namespace mvvm.converters
{
    [SerializeField]
    public class ItemMappingImpl : ItemMapping<int, string>
    {
    }

    public class ValueConverterExample : ValueConverter<ItemMappingImpl, int, string>
    {
    }
}
