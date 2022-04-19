using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

using CFM.Framework.Binding.Reflection;

namespace CFM.Framework.Binding.Proxy.Sources.Object
{
    public abstract class ItemNodeProxy<T> : NotifiableSourceProxyBase, IObtainable, IModifiable, INotifiable
    {
        protected T key;

        protected IProxyItemInfo itemInfo;

        protected bool isList;

        protected Regex regex;

        public ItemNodeProxy(ICollection source, T key, IProxyItemInfo itemInfo) : base(source)
        {
            this.key = key;
            isList = (source is IList);

            this.itemInfo = itemInfo;

            if (this.source != null && this.source is INotifyCollectionChanged)
            {
                var sourceCollection = this.source as INotifyCollectionChanged;
                sourceCollection.CollectionChanged += OnCollectionChanged;
            }

            if (!isList)
            {
                regex = new Regex(@"\[" + this.key + ",", RegexOptions.IgnorePatternWhitespace);
            }
        }

        public override Type Type { get { return itemInfo.ValueType; } }

        public override TypeCode TypeCode { get { return itemInfo.ValueTypeCode; } }

        protected abstract void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e);


        public virtual object GetValue()
        {
            return itemInfo.GetValue(source, key);
        }

        public abstract TValue GetValue<TValue>();

        public virtual void SetValue(object value)
        {
            itemInfo.SetValue(source, key, value);
        }

        public abstract void SetValue<TValue>(TValue value);

        private bool disposedValue = false;

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (source != null && source is INotifyCollectionChanged)
                {
                    var sourceCollection = source as INotifyCollectionChanged;
                    sourceCollection.CollectionChanged -= OnCollectionChanged;
                }
                disposedValue = true;
                base.Dispose(disposing);
            }
        }
    }

    public class IntItemNodeProxy : ItemNodeProxy<int>
    {
        public IntItemNodeProxy(ICollection source, int key, IProxyItemInfo itemInfo) : base(source, key, itemInfo)
        {
        }

        protected override void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (isList)
            {
                //IList or Array
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Reset:
                        RaiseValueChanged();
                        break;
                    case NotifyCollectionChangedAction.Remove:
                    case NotifyCollectionChangedAction.Replace:
                        if (key == e.OldStartingIndex || key == e.NewStartingIndex)
                            RaiseValueChanged();
                        break;
                    case NotifyCollectionChangedAction.Move:
                        if (key == e.OldStartingIndex || key == e.NewStartingIndex)
                            RaiseValueChanged();
                        break;
                    case NotifyCollectionChangedAction.Add:
                        int endIndex = e.NewItems != null ? e.NewStartingIndex + e.NewItems.Count : e.NewStartingIndex + 1;
                        if (key >= e.NewStartingIndex && key < endIndex)
                            RaiseValueChanged();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                //IDictionary
                if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    RaiseValueChanged();
                    return;
                }

                if (e.NewItems != null && e.NewItems.Count > 0)
                {
                    foreach (var item in e.NewItems)
                    {
                        if (regex.IsMatch(item.ToString()))
                        {
                            RaiseValueChanged();
                            return;
                        }
                    }
                }

                if (e.OldItems != null && e.OldItems.Count > 0)
                {
                    foreach (var item in e.OldItems)
                    {
                        if (regex.IsMatch(item.ToString()))
                        {
                            RaiseValueChanged();
                            return;
                        }
                    }
                }
            }
        }

        public override TValue GetValue<TValue>()
        {
            if (!typeof(TValue).IsAssignableFrom(itemInfo.ValueType))
                throw new MemberAccessException();

            var proxy = itemInfo as IProxyItemInfo<int, TValue>;
            if (proxy != null)
                return proxy.GetValue(source, key);

            return (TValue)itemInfo.GetValue(source, key);
        }

        public override void SetValue<TValue>(TValue value)
        {
            var proxy = itemInfo as IProxyItemInfo<int, TValue>;
            if (proxy != null)
            {
                proxy.SetValue(source, key, value);
                return;
            }

            itemInfo.SetValue(source, key, value);
        }
    }

    public class StringItemNodeProxy : ItemNodeProxy<string>
    {
        public StringItemNodeProxy(ICollection source, string key, IProxyItemInfo itemInfo) : base(source, key, itemInfo)
        {
        }

        protected override void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //IDictionary
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                RaiseValueChanged();
                return;
            }

            if (e.NewItems != null && e.NewItems.Count > 0)
            {
                foreach (var item in e.NewItems)
                {
                    if (regex.IsMatch(item.ToString()))
                    {
                        RaiseValueChanged();
                        return;
                    }
                }
            }

            if (e.OldItems != null && e.OldItems.Count > 0)
            {
                foreach (var item in e.OldItems)
                {
                    if (regex.IsMatch(item.ToString()))
                    {
                        RaiseValueChanged();
                        return;
                    }
                }
            }
        }

        public override TValue GetValue<TValue>()
        {
            if (!typeof(TValue).IsAssignableFrom(itemInfo.ValueType))
                throw new InvalidCastException();

            var proxy = itemInfo as IProxyItemInfo<string, TValue>;
            if (proxy != null)
                return proxy.GetValue(source, key);

            return (TValue)itemInfo.GetValue(source, key);
        }

        public override void SetValue<TValue>(TValue value)
        {
            var proxy = itemInfo as IProxyItemInfo<string, TValue>;
            if (proxy != null)
            {
                proxy.SetValue(source, key, value);
                return;
            }

            itemInfo.SetValue(source, key, value);
        }
    }
}