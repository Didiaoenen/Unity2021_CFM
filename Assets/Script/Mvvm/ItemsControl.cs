using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ItemsControl : MonoBehaviour
{
    public class ItemInfo
    {
        public readonly object Item;
        public readonly GameObject Control;
        public readonly RectTransform Rect;

        public  ItemInfo(object item, GameObject control, RectTransform rect)
        {
            Item = item;
            Control = control;
            Rect = rect;
        }
    }

    private ICreateDestroyItems _createDetroyItemsBehavior;

    [SerializeField]
    [Tooltip("")]
    protected bool _reuseControlsForReset = false;

    [SerializeField]
    private GameObject _itemTemplate;

    public GameObject ItemTemplate
    {
        get {  return _itemTemplate; }
        set
        {
            if (_itemTemplate == value) return;
            _itemTemplate = value;
            ResetCollection(false);
        }
    }

    [SerializeField]
    private DataTemplateSelector _itemTemplateSelector;

    public DataTemplateSelector ItemTemplateSelector
    {
        get {  return _itemTemplateSelector; }
        set
        {
            if (_itemTemplateSelector == value) return;
            _itemTemplateSelector = value;
            ResetCollection(false);
        }
    }

    [SerializeField]
    private bool _destroyChildrenOnAwake = false;

    private IEnumerable _itemsSource;

    public IEnumerable ItemsSource
    {
        get {  return _itemsSource; }
        set
        {
            if (_itemsSource == value) return;
            ResetBindings(_itemsSource, value);
            _itemsSource = value;
            ResetCollection(true);
            OnItemsSourceChanged();
            ItemsSourceChanged.Invoke();
        }
    }

    protected readonly List<ItemInfo> _items = new List<ItemInfo>();

    [SerializeField]
    private UnityEvent _itemsSourceChanged = null;

    public UnityEvent ItemsSourceChanged { get { return _itemsSourceChanged; } }

    public bool HasItems { get { return _items.Count > 0; } }

    [SerializeField]
    private UnityEvent _hasItemsChanged = null;

    public UnityEvent HasItemsChanged { get { return _hasItemsChanged; } }

    [Tooltip("")]
    [SerializeField]
    private bool _cacheGameObjects = false;

    [Tooltip("")]
    [SerializeField]
    private int _cacheGameObjectPoolSize = 10;

    void Awake()
    {
        if (_destroyChildrenOnAwake)
        {
            for (var i = transform.childCount - 1; i >= 0; i--)
            {
                var cg = transform.GetChild(i).gameObject;
                if (cg == ItemTemplate)
                    cg.SetActive(false);
                else if (_items.All(c => c.Control != cg))
                    Destroy(cg);
            }
        }

        if (ItemTemplateSelector != null)
        {
            Debug.LogWarning("");
            _cacheGameObjects = false;
        }

        if (_cacheGameObjects == true)
        {
            _createDetroyItemsBehavior = new CachingCreateDestroyItemsBehavior(new DefaultCreateDestroyItemsBehavior(transform, _itemTemplate, _itemTemplateSelector), _cacheGameObjectPoolSize);
        }
        else
        {
            _createDetroyItemsBehavior = new DefaultCreateDestroyItemsBehavior(transform, _itemTemplate, _itemTemplateSelector);
        }
    }

    private void ResetBindings(IEnumerable oldvalue, IEnumerable newvalue)
    {
        if (oldvalue is INotifyCollectionChanged)
        {
            (oldvalue as INotifyCollectionChanged).CollectionChanged -= CollectionChanged;
        }
        if (newvalue is INotifyCollectionChanged)
        {
            (newvalue as INotifyCollectionChanged).CollectionChanged += CollectionChanged;
        }
    }

    protected virtual void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                AddItems(e.NewItems);
                break;
            case NotifyCollectionChangedAction.Remove:
                RemoveItems(e.OldItems);
                break;
            case NotifyCollectionChangedAction.Replace:
                RemoveItems(e.OldItems);
                AddItems(e.NewItems);
                break;
            case NotifyCollectionChangedAction.Move:
                MoveItem(e.OldStartingIndex, e.NewStartingIndex);
                break;
            default:
                ResetCollection(true);
                break;
        }
    }

    private void MoveItem(int oldIndex, int newIndex)
    {
        var item = _items[oldIndex];
        _items.RemoveAt(oldIndex);
        _items.Insert(newIndex, item);
        var rect = item.Rect;
        rect.SetSiblingIndex(newIndex);
    }

    private ItemInfo AddItem(object item)
    {
        GameObject control = _createDetroyItemsBehavior.CreateItemControl(item);
        RectTransform rect = control.GetComponent<RectTransform>();

        var info = new ItemInfo(item, control, rect);
        _items.Add(info);

        control.SetActive(true);

        var context = control.GetComponent<DataContext>();
        if (context != null)
            context.UpdateValue(item);

        OnItemAdded(info);
        return info;
    }

    private void AddItems(IEnumerable newItems)
    {
        foreach (var item in newItems)
        {
            AddItem(item);
        }

        HasItemsChanged.Invoke();
    }

    protected virtual void OnItemAdded(ItemInfo info) { }

    protected virtual void RemoveItems(IEnumerable oldItems)
    {
        foreach (var item in oldItems)
        {
            RemoveAt(_items.FindIndex(i => i.Item == item));
        }

        HasItemsChanged.Invoke();
    }

    protected virtual void OnItemRemoved(ItemInfo info) { }

    private void ResetCollection(bool allowControlReuse)
    {
        if (_reuseControlsForReset && allowControlReuse)
        {
            var source = new List<object>();
            foreach (var item in _itemsSource)
                source.Add(item);

            var oldItems = _items.ToArray();
            _items.Clear();
            var toRemove = new List<ItemInfo>(oldItems);

            var oldLookup = new Dictionary<object, int>(oldItems.Length);
            for (var i = 0; i < oldItems.Length; i++)
                oldLookup[oldItems[i].Item] = i;

            for (var i = 0; i < source.Count; i++)
            {
                int oldIdx;
                if (!oldLookup.TryGetValue(source[i], out oldIdx))
                {
                    var item = AddItem(source[i]);
                    item.Rect.SetSiblingIndex(i);
                }
                else
                {
                    oldLookup.Remove(source[i]);

                    var oldItem = oldItems[oldIdx];
                    toRemove.Remove(oldItem);
                    _items.Add(oldItem);
                    oldItem.Rect.SetSiblingIndex(i);
                }
            }

            foreach (var item in toRemove)
            {
                _createDetroyItemsBehavior.DestroyItemControl(item);
            }

            HasItemsChanged.Invoke();
        }
        else
        {
            for (var i = _items.Count - 1; i >= 0; i--)
            {
                RemoveAt(i);
            }
            AddItems(_itemsSource);
        }
    }

    protected virtual void OnItemsSourceChanged() { }

    void RemoveAt(int idx)
    {
        if (idx < 0) return;
        var item = _items[idx];
        _items.RemoveAt(idx);
        _createDetroyItemsBehavior.DestroyItemControl(item);
        OnItemRemoved(item);
    }

    void OnDestroy()
    {
        if (_itemsSource is INotifyCollectionChanged)
        {
            (_itemsSource as INotifyCollectionChanged).CollectionChanged -= CollectionChanged;
        }
    }
}
