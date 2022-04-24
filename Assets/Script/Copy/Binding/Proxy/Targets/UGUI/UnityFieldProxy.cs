using UnityEngine.Events;
using CFM.Framework.Binding.Reflection;
using CFM.Framework.Binding.Proxy.Targets.Universal;

namespace CFM.Framework.Binding.Proxy.Targets.UGUI
{
    public class UnityFieldProxy<TValue> : FieldTargetProxy
    {
        private UnityEvent<TValue> unityEvent;
        public UnityFieldProxy(object target, IProxyFieldInfo fieldInfo, UnityEvent<TValue> unityEvent) : base(target, fieldInfo)
        {
            this.unityEvent = unityEvent;
        }

        public override BindingMode DefaultMode { get { return BindingMode.TwoWay; } }

        protected override void DoSubscribeForValueChange(object target)
        {
            if (unityEvent == null || target == null)
                return;

            unityEvent.AddListener(OnValueChanged);
        }

        protected override void DoUnsubscribeForValueChange(object target)
        {
            if (unityEvent != null)
                unityEvent.RemoveListener(OnValueChanged);
        }

        private void OnValueChanged(TValue value)
        {
            RaiseValueChanged();
        }
    }
}
