#if !(UNITY_WSA || !NET_LEGACY)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

[Serializable()]
public class ObservableCollection<T> : Collection<T>, INotifyCollectionChanged, INotifyPropertyChanged
{
#region Constructors
    public ObservableCollection() : base() { }

    public ObservableCollection(List<T> list)
        : base((list != null) ? new List<T>(list.Count) : list)
    {
        IList<T> items = Items;
        if (list != null && items != null)
        {
            using (IEnumerator<T> enumerator = list.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    items.Add(enumerator.Current);
                }
            }
        }
    }

#endregion Constructors

#region Public Methods

    public void Move(int oldIndex, int newIndex)
    {
        MoveItem(oldIndex, newIndex);
    }

#endregion Public Methods

#region Public Events

#region INotifyPropertyChanged implementation

    event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
    {
        add
        {
            PropertyChanged += value;
        }
        remove
        {
            PropertyChanged -= value;
        }
    }
#endregion INotifyPropertyChanged implementation

    public virtual event NotifyCollectionChangedEventHandler CollectionChanged;

#endregion Public Events

#region Protected Methods

    protected override void ClearItems()
    {
        CheckReentrancy();
        base.ClearItems();
        OnPropertyChanged(CountString);
        OnPropertyChanged(IndexerName);
        OnCollectionReset();
    }

    protected override void RemoveItem(int index)
    {
        CheckReentrancy();
        T removedItem = this[index];

        base.RemoveItem(index);

        OnPropertyChanged(CountString);
        OnPropertyChanged(IndexerName);
        OnCollectionChanged(NotifyCollectionChangedAction.Remove, removedItem, index);
    }

    protected override void InsertItem(int index, T item)
    {
        CheckReentrancy();
        base.InsertItem(index, item);

        OnPropertyChanged(CountString);
        OnPropertyChanged(IndexerName);
        OnCollectionChanged(NotifyCollectionChangedAction.Add, item, index);
    }

    protected override void SetItem(int index, T item)
    {
        CheckReentrancy();
        T originalItem = this[index];
        base.SetItem(index, item);

        OnPropertyChanged(IndexerName);
        OnCollectionChanged(NotifyCollectionChangedAction.Replace, originalItem, item, index);
    }

    protected virtual void MoveItem(int oldIndex, int newIndex)
    {
        CheckReentrancy();

        T removedItem = this[oldIndex];

        base.RemoveItem(oldIndex);
        base.InsertItem(newIndex, removedItem);

        OnPropertyChanged(IndexerName);
        OnCollectionChanged(NotifyCollectionChangedAction.Move, removedItem, newIndex, oldIndex);
    }


    protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, e);
        }
    }

    protected virtual event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        if (CollectionChanged != null)
        {
            using (BlockReentrancy())
            {
                CollectionChanged(this, e);
            }
        }
    }

    protected IDisposable BlockReentrancy()
    {
        _monitor.Enter();
        return _monitor;
    }

    protected void CheckReentrancy()
    {
        if (_monitor.Busy)
        {
            // we can allow changes if there's only one listener - the problem
            // only arises if reentrant changes make the original event args
            // invalid for later listeners.  This keeps existing code working
            // (e.g. Selector.SelectedItems).
            if ((CollectionChanged != null) && (CollectionChanged.GetInvocationList().Length > 1))
                throw new InvalidOperationException("ObservableCollectionReentrancyNotAllowed");
        }
    }

#endregion Protected Methods

#region Private Methods
    private void OnPropertyChanged(string propertyName)
    {
        OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
    }

    private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index)
    {
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));
    }

    private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index, int oldIndex)
    {
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index, oldIndex));
    }

    private void OnCollectionChanged(NotifyCollectionChangedAction action, object oldItem, object newItem, int index)
    {
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index));
    }

    private void OnCollectionReset()
    {
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }
#endregion Private Methods

#region Private Types

    [Serializable()]
    private class SimpleMonitor : IDisposable
    {
        public void Enter()
        {
            ++_busyCount;
        }

        public void Dispose()
        {
            --_busyCount;
        }

        public bool Busy { get { return _busyCount > 0; } }

        int _busyCount;
    }

#endregion Private Types

#region Private Fields

    private const string CountString = "Count";

    private const string IndexerName = "Item[]";

    private SimpleMonitor _monitor = new SimpleMonitor();

#endregion Private Fields
}
#endif
