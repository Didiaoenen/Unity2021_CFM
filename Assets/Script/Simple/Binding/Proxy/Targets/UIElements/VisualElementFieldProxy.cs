using UnityEngine.UIElements;
using Assembly_CSharp.Assets.Script.Simple.Binding.Reflection;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Targets.Universal;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Targets.UIElements
{
    public class VisualElementFieldProxy<TValue> : FieldTargetProxy
    {
        private readonly INotifyValueChanged<TValue> notifyValueChanged;

        public override BindingMode DefaultMode { get { return BindingMode.TwoWay; } }

        public VisualElementFieldProxy(object target, IProxyFieldInfo fieldInfo) : base(target, fieldInfo)
        {
            if (target is INotifyValueChanged<TValue>)
                notifyValueChanged = (INotifyValueChanged<TValue>)target;
            else
                notifyValueChanged = null;
        }

        protected override void DoSubscribeForValueChange(object target)
        {
            if (notifyValueChanged == null || target == null)
                return;

            notifyValueChanged.RegisterValueChangedCallback(OnValueChanged);
        }

        protected override void DoUnsubscribeForValueChange(object target)
        {
            if (notifyValueChanged != null)
                notifyValueChanged.UnregisterValueChangedCallback(OnValueChanged);
        }

        private void OnValueChanged(ChangeEvent<TValue> e)
        {
            RaiseValueChanged();
        }
    }
}

