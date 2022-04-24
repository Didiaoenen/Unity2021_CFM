using System;
using CFM.Framework.Observables;

namespace CFM.Framework.Binding.Proxy.Sources.Text
{
    public class ObservableLiteralSourceProxy : NotifiableSourceProxyBase, ISourceProxy, IObtainable
    {
        private IObservableProperty observableProperty;

        public ObservableLiteralSourceProxy(IObservableProperty source) : base(source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            observableProperty = source;
            observableProperty.ValueChanged += OnValueChanged;
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            RaiseValueChanged();
        }

        public override Type Type { get { return observableProperty.Type; } }

        public virtual object GetValue()
        {
            return observableProperty.Value;
        }

        public virtual TValue GetValue<TValue>()
        {
            return (TValue)Convert.ChangeType(observableProperty.Value, typeof(TValue));
        }

        private bool disposedValue = false;

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (observableProperty != null)
                    observableProperty.ValueChanged -= OnValueChanged;

                disposedValue = true;
                base.Dispose(disposing);
            }
        }
    }
}

