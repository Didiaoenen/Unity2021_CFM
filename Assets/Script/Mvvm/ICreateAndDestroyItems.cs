using UnityEngine;
using static Mvvm.ItemsControl;

namespace Mvvm
{
    public interface ICreateDestroyItems
    {
        GameObject CreateItemControl(object item);
        void DestroyItemControl(ItemInfo item);
        GameObject InstantiateItem();
        void SetItemTemplate(GameObject itemTemplate);
        void SetItemTemplate(DataTemplateSelector dataTemplateSelector);
    }
}
