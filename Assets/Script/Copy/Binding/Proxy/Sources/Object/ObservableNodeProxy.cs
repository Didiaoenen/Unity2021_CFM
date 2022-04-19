using System;

using CFM.Framework.Observables;

namespace CFM.Framework.Binding.Proxy.Sources.Object
{
    public class ObservableNodeProxy : NotifiableSourceProxyBase, IObtainable, IModifiable, INotifiable
    {
        protected IObservableProperty property;

        public ObservableNodeProxy(IObservableProperty property) : this(null, property)
        {
        }
        public ObservableNodeProxy(object source, IObservableProperty property) : base(source)
        {
            this.property = property;
            this.property.ValueChanged += OnValueChanged;
        }

        public override Type Type { get { return property.Type; } }

        protected virtual void OnValueChanged(object sender, EventArgs e)
        {
            RaiseValueChanged();
        }

        public virtual object GetValue()
        {
            return property.Value;
        }

        public virtual TValue GetValue<TValue>()
        {
            var observableProperty = property as IObservableProperty<TValue>;
            if (observableProperty != null)
                return observableProperty.Value;

            return (TValue)property.Value;
        }

        public virtual void SetValue(object value)
        {
            property.Value = value;
        }

        public virtual void SetValue<TValue>(TValue value)
        {
            var observableProperty = property as IObservableProperty<TValue>;
            if (observableProperty != null)
            {
                observableProperty.Value = value;
                return;
            }

            property.Value = value;
        }

        private bool disposedValue = false;

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (property != null)
                    property.ValueChanged -= OnValueChanged;

                disposedValue = true;
                base.Dispose(disposing);
            }
        }
    }
}