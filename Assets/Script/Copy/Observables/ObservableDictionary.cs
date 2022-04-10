using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace CFM.Framework.Observables
{
    [Serializable]
    public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IDictionary, INotifyCollectionChanged, INotifyPropertyChanged
    {

        private static readonly PropertyChangedEventArgs CountEventArgs = new PropertyChangedEventArgs("Count");

        private static readonly PropertyChangedEventArgs IndexerEventArgs = new PropertyChangedEventArgs("Item[]");

        private static readonly PropertyChangedEventArgs KeysEventArgs = new PropertyChangedEventArgs("Keys");

        private static readonly PropertyChangedEventArgs ValuesEventArgs = new PropertyChangedEventArgs("Values");

        private readonly object propertyChangedLock = new object();
        
        private readonly object collectionChangedLock = new object();
        
        private PropertyChangedEventHandler propertyChanged;
        
        private NotifyCollectionChangedEventHandler collectionChanged;

        protected Dictionary<TKey, TValue> dictinary;

        public event PropertyChangedEventHandler PropertyChanged
        {
            add { lock(propertyChangedLock) { propertyChanged += value; } }
            remove { lock(propertyChangedLock) { propertyChanged -= value; } }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { lock(collectionChangedLock) { collectionChanged += value; } }
            remove { lock(collectionChangedLock) { collectionChanged -= value; } }
        }

        public ObservableDictionary()
        {
            dictinary = new Dictionary<TKey, TValue>();
        }

        public ObservableDictionary(IDictionary<TKey, TValue> dictionary)
        {
            dictinary = new Dictionary<TKey, TValue>(dictionary);
        }

        public ObservableDictionary(IEqualityComparer<TKey> comparer)
        {
            dictinary = new Dictionary<TKey, TValue>(comparer);
        }

        public ObservableDictionary(int capacity)
        {
            dictinary = new Dictionary<TKey, TValue>(capacity);
        }

        public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            dictinary = new Dictionary<TKey, TValue>(dictionary, comparer);
        }

        public ObservableDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            dictinary = new Dictionary<TKey, TValue>(capacity, comparer);
        }

        public TValue this[TKey key]
        {
            get
            {
                if (!dictinary.ContainsKey(key))
                    return default(TValue);
                return dictinary[key];
            }
            set
            {
                Insert(key, value, false);
            }
        }

        public object this[object key] 
        { 
            get { return dictinary.Keys; }
        }

        public ICollection<TKey> Keys
        {
            get { return dictinary.Keys; }
        }

        public ICollection<TValue> Values
        {
            get { return dictinary.Values; }
        }

        public void Add(TKey key, TValue value)
        {
            Insert(key, value, true);
        }

        public bool Remove(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            TValue value;
            dictinary.TryGetValue(key, out value);
            var removed = dictinary.Remove(key);
            if (removed)
            {
                OnPropertyChanged(NotifyCollectionChangedAction.Remove);
                if (collectionChanged != null)
                    OnCollectionChanged(NotifyCollectionChangedAction.Remove, new KeyValuePair<TKey, TValue>(key, value));
            }

            return removed;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return dictinary.TryGetValue(key, out value);
        }

        public bool ContainsKey(TKey key)
        {
            return dictinary.ContainsKey(key);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Insert(item.Key, item.Value, true);
        }

        public void Clear()
        {
            if (dictinary.Count > 0)
            {
                dictinary.Clear();
                OnPropertyChanged(NotifyCollectionChangedAction.Reset);
                if (collectionChanged != null)
                    OnCollectionChanged();
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return dictinary.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((IDictionary)dictinary).CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return dictinary.Count; }
        }

        public bool IsReadOnly
        {
            get { return ((IDictionary)dictinary).IsReadOnly; }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return dictinary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)dictinary).GetEnumerator();
        }

        public void AddRange(IDictionary<TKey, TValue> items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            if (items.Count > 0)
            {
                if (dictinary.Count > 0)
                {
                    if (items.Keys.Any((key) => dictinary.ContainsKey(key)))
                        throw new ArgumentException("");
                    else
                    {
                        foreach (var item in items)
                            ((IDictionary<TKey, TValue>)dictinary).Add(item);
                    }
                }
                else
                {
                    dictinary = new Dictionary<TKey, TValue>(items);
                }

                OnPropertyChanged(NotifyCollectionChangedAction.Add);
                if (collectionChanged != null)
                    OnCollectionChanged(NotifyCollectionChangedAction.Add, items.ToArray());
            }
        }

        private void Insert(TKey key, TValue value, bool add)
        {
            if (key == null)
                throw new ArgumentNullException();

            TValue item;
            if (dictinary.TryGetValue(key, out item))
            {
                if (add)
                    throw new ArgumentException();

                if (Equals(item, value))
                    return;

                dictinary[key] = value;
                OnPropertyChanged(NotifyCollectionChangedAction.Replace);
                if (collectionChanged != null)
                    OnCollectionChanged(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(key, value));
            }
        }

        private void OnPropertyChanged(NotifyCollectionChangedAction action)
        {
            switch (action)
            {
                case NotifyCollectionChangedAction.Reset:
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                    {
                        OnPropertyChanged(CountEventArgs);
                        OnPropertyChanged(IndexerEventArgs);
                        OnPropertyChanged(KeysEventArgs);
                        OnPropertyChanged(ValuesEventArgs);
                        break;
                    }
                case NotifyCollectionChangedAction.Replace:
                    {
                        OnPropertyChanged(IndexerEventArgs);
                        OnPropertyChanged(ValuesEventArgs);
                        break;
                    }
                case NotifyCollectionChangedAction.Move:
                default:
                    {
                        OnPropertyChanged(CountEventArgs);
                        OnPropertyChanged(IndexerEventArgs);
                        OnPropertyChanged(KeysEventArgs);
                        OnPropertyChanged(ValuesEventArgs);
                        break;
                    }
            }
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            if (propertyChanged != null)
                propertyChanged(this, eventArgs);
        }

        private void OnCollectionChanged()
        {
            if (collectionChanged != null)
                collectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action, KeyValuePair<TKey, TValue>changedItem)
        {
            if (collectionChanged != null)
                collectionChanged(this, new NotifyCollectionChangedEventArgs(action, changedItem));
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action, KeyValuePair<TKey, TValue> newItem, KeyValuePair<TKey, TValue> oldItem)
        {
            if (collectionChanged != null)
                collectionChanged(this, new NotifyCollectionChangedEventArgs(action, newItem, oldItem));
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action, IList newItems)
        {
            if (collectionChanged != null)
                collectionChanged(this, new NotifyCollectionChangedEventArgs(action, newItems));
        }

        object IDictionary.this[object key]
        {
            get { return ((IDictionary)dictinary)[key]; }
            set { Insert((TKey)key, (TValue)value, false); }
        }

        ICollection IDictionary.Keys
        {
            get { return ((IDictionary)dictinary).Keys; }
        }

        ICollection IDictionary.Values
        {
            get { return ((IDictionary)dictinary).Values; }
        }


        public bool Contains(object key)
        {
            return ((IDictionary)dictinary).Contains(key);
        }

        public void Add(object key, object value)
        {
            Add((TKey)key, (TValue)value);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return ((IDictionary)dictinary).GetEnumerator();
        }

        public void Remove(object key)
        {
            Remove((TKey)key);
        }

        public bool IsFixedSize
        {
            get { return ((IDictionary)dictinary).IsFixedSize; }
        }

        public void CopyTo(Array array, int index)
        {
            ((IDictionary)dictinary).CopyTo(array, index);
        }

        public object SyncRoot
        {
            get { return ((IDictionary)dictinary).SyncRoot; }
        }

        public bool IsSynchronized
        {
            get { return ((IDictionary)dictinary).IsSynchronized; }
        }
    }
}

