using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemsControl;

public interface ICreateDestroyItems
{
    GameObject CreateItemControl(object item);
    void DestroyItemControl(ItemInfo item);
    GameObject InstantiateItem();
    void SetItemTemplate(GameObject itemTemplate);
    void SetItemTemplate(DataTemplateSelector dataTemplateSelector);
}
