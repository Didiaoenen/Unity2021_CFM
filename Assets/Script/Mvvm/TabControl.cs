using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

[AddComponentMenu("UI/Tabs/TabControl", 1)]
public class TabControl : Selector
{
    private GameObject _lastTab;

    protected override void OnItemsSourceChanged()
    {
        base.OnItemsSourceChanged();

    }

    protected override void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        base.CollectionChanged(sender, e);
    }

    private void Reselect()
    {
        if (_items.Count == 0)
        {
            
        }
    }

    protected override void OnSelectedChanged(bool fromProperty)
    {
        base.OnSelectedChanged(fromProperty);
    }

    private void SetTabState(GameObject tab)
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).gameObject;
            if (i == 0 && tab == null)
                SetTabSelected(child, true);
            else
                SetTabSelected(child, tab == child);
        }
    }

    private void SetTabSelected(GameObject tab, bool state)
    {
        if (tab == null) return;
        tab.GetComponent<TabItem>().SetSelected(state);
    }

    internal void SelectTab(TabItem item)
    {
        SetSelected(_items.FirstOrDefault(i => i.Control == item.gameObject));
        if (_items.Count == 0)
            SetTabState(item.gameObject);
    }
}
