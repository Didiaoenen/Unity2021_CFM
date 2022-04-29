using System;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources;

namespace Assembly_CSharp.Assets.Script.Simple.Observables
{
    public class ObservableNodeProxy : NotifiableSourceProxyBase, IObtainable, IModifiable, INotifiable
    {
        protected IObservableProperty property;

        public override Type Type { get { return property.Type; } }

        public ObservableNodeProxy(IObservableProperty property) : this(null, property)
        {
        }

        public ObservableNodeProxy(object source, IObservableProperty property) : base(source)
        {
            this.property = property;
            this.property.ValueChanged += OnValueChanged;
        }

        protected virtual void OnValueChanged(object sender, EventArgs e)
        {
            RaiseValueChanged();
        }

        public object GetValue()
        {
            return property.Value;
        }

        public TValue GetValue<TValue>()
        {
            var observableProperty = property as IObservableProperty<TValue>;
            if (observableProperty != null)
                return observableProperty.Value;

            return (TValue)property.Value;
        }

        public void SetValue(object value)
        {
            property.Value = value;
        }

        public void SetValue<TValue>(TValue value)
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

