using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class Selector : ItemsControl
{
    protected ItemInfo SelectedInfo {  get; private set; }

    public object Selected
    { 
        get { return SelectedInfo == null ? null : SelectedInfo.Item; } 
        set 
        {
            ItemInfo info;
            if (value == null)
                info= null;
            else
                info = _items.FirstOrDefault(i => ReferenceEquals(i.Item, value));

            InternalSetSelected(info, true);
        }
    }

    public GameObject SelectedControl
    {
        get {  return SelectedInfo == null ? null : SelectedInfo.Control; }
    }

    [SerializeField]
    private UnityEvent _selectedChanged = null;

    public UnityEvent SelectedChanged {  get {  return _selectedChanged; } }

    protected override void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        base.CollectionChanged(sender, e);
        ValidateSelected();
    }

    protected void ValidateSelected()
    {
        Selected = Selected;
    }

    protected void SetSelected(ItemInfo info)
    {
        InternalSetSelected(info, false);
    }

    void InternalSetSelected(ItemInfo info, bool fromProperty)
    {
        if (info == SelectedInfo) return;

        SelectedInfo = info;

        OnSelectedChanged(fromProperty);
        SelectedChanged.Invoke();
    }

    protected virtual void OnSelectedChanged(bool fromProperty) { }
}
