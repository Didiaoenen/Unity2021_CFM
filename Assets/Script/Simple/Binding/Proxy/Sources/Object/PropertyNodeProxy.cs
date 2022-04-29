using System;
using System.ComponentModel;
using Assembly_CSharp.Assets.Script.Simple.Binding.Reflection;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources.Object
{
    public class PropertyNodeProxy : NotifiableSourceProxyBase, IObtainable, IModifiable, INotifiable
    {
        protected IProxyPropertyInfo propertyInfo;

        public override Type Type { get { return propertyInfo.ValueType; } }

        public override TypeCode TypeCode { get { return propertyInfo.ValueTypeCode; } }

        public PropertyNodeProxy(IProxyPropertyInfo propertyInfo) : this(null, propertyInfo)
        {
        }

        public PropertyNodeProxy(object source, IProxyPropertyInfo propertyInfo) : base(source)
        {
            this.propertyInfo = propertyInfo;

            if (this.source == null)
                return;

            if (this.source is INotifyPropertyChanged)
            {
                var sourceNotify = this.source as INotifyPropertyChanged;
                sourceNotify.PropertyChanged += OnPropertyChanged;
            }
            else
            {
            }
        }

        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var name = e.PropertyName;
            if (string.IsNullOrEmpty(name) || name.Equals(propertyInfo.Name))
                RaiseValueChanged();
        }

        public object GetValue()
        {
            return propertyInfo.GetValue(source);
        }

        public TValue GetValue<TValue>()
        {
            var proxy = propertyInfo as IProxyPropertyInfo<TValue>;
            if (proxy != null)
                return proxy.GetValue(source);

            return (TValue)propertyInfo.GetValue(source);
        }

        public void SetValue(object value)
        {
            propertyInfo.SetValue(source, value);
        }

        public void SetValue<TValue>(TValue value)
        {
            var proxy = propertyInfo as IProxyPropertyInfo<TValue>;
            if (proxy != null)
            {
                proxy.SetValue(source, value);
                return;
            }

            propertyInfo.SetValue(source, value);
        }

        private bool disposedValue = false;

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (source != null && source is INotifyPropertyChanged)
                {
                    var sourceNotify = source as INotifyPropertyChanged;
                    sourceNotify.PropertyChanged -= OnPropertyChanged;
                }
                disposedValue = true;
                base.Dispose(disposing);
            }
        }
    }
}

