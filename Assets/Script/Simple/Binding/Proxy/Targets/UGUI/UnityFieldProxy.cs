using UnityEngine.Events;
using Assembly_CSharp.Assets.Script.Simple.Binding.Reflection;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Targets.Universal;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Targets.UGUI
{
    public class UnityFieldProxy<TValue> : FieldTargetProxy
    {
        private UnityEvent<TValue> unityEvent;

        public override BindingMode DefaultMode { get { return BindingMode.TwoWay; } }

        public UnityFieldProxy(object target, IProxyFieldInfo fieldInfo, UnityEvent<TValue> unityEvent) : base(target, fieldInfo)
        {
            this.unityEvent = unityEvent;
        }

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

