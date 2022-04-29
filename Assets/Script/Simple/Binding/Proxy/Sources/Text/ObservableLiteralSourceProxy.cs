using System;
using Assembly_CSharp.Assets.Script.Simple.Observables;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources.Text
{
    public class ObservableLiteralSourceProxy : NotifiableSourceProxyBase, ISourceProxy, IObtainable
    {
        private IObservableProperty observableProperty;

        public override Type Type { get { return observableProperty.Type; } }

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

        public object GetValue()
        {
            return observableProperty.Value;
        }

        public TValue GetValue<TValue>()
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

