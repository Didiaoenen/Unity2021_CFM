using UnityEngine.Events;
using Assembly_CSharp.Assets.Script.Simple.Binding.Reflection;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Targets.Universal;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Targets.UGUI
{
    public class UnityPropertyProxy<TValue> : PropertyTargetProxy
    {
        private UnityEvent<TValue> unityEvent;

        public override BindingMode DefaultMode { get { return BindingMode.TwoWay; } }

        public UnityPropertyProxy(object target, IProxyPropertyInfo propertyInfo, UnityEvent<TValue> unityEvent) : base(target, propertyInfo)
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

