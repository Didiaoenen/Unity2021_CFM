using System;
using CFM.Framework.Observables;

namespace CFM.Framework.Binding.Proxy.Targets.Universal
{
    public class ObservableTargetProxy : ValueTargetProxyBase
    {
        protected readonly IObservableProperty observableProperty;

        public ObservableTargetProxy(object target, IObservableProperty observableProperty) : base(target)
        {
            this.observableProperty = observableProperty;
        }

        public override Type Type { get { return observableProperty.Type; } }

        public override BindingMode DefaultMode { get { return BindingMode.TwoWay; } }

        public override object GetValue()
        {
            return observableProperty.Value;
        }

        public override TValue GetValue<TValue>()
        {
            if (observableProperty is IObservableProperty<TValue>)
                return ((IObservableProperty<TValue>)observableProperty).Value;

            return (TValue)observableProperty.Value;
        }

        public override void SetValue(object value)
        {
            observableProperty.Value = value;
        }

        public override void SetValue<TValue>(TValue value)
        {
            if (observableProperty is IObservableProperty<TValue>)
            {
                ((IObservableProperty<TValue>)observableProperty).Value = value;
                return;
            }

            observableProperty.Value = value;
        }

        protected override void DoSubscribeForValueChange(object target)
        {
            observableProperty.ValueChanged += OnValueChanged;
        }

        protected override void DoUnsubscribeForValueChange(object target)
        {
            observableProperty.ValueChanged -= OnValueChanged;
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            RaiseValueChanged();
        }
    }
}
