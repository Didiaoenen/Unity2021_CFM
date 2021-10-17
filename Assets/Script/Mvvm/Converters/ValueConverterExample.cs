using UnityEngine;

namespace Mvvm.converters
{
    [SerializeField]
    public class ItemMappingImpl : ItemMapping<int, string>
    {
    }

    public class ValueConverterExample : ValueConverter<ItemMappingImpl, int, string>
    {
    }
}
