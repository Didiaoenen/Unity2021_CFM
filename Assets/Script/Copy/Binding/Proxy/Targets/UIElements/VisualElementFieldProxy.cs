#if UNITY_2019_1_OR_NEWER
using UnityEngine.UIElements;
using CFM.Framework.Binding.Reflection;
using CFM.Framework.Binding.Proxy.Targets.Universal;

namespace CFM.Framework.Binding.Proxy.Targets.UIElements
{
    public class VisualElementFieldProxy<TValue> : FieldTargetProxy
    {
        private readonly INotifyValueChanged<TValue> notifyValueChanged;
        
        public VisualElementFieldProxy(object target, IProxyFieldInfo fieldInfo) : base(target, fieldInfo)
        {
            //�������е����Զ��Զ��󶨣����ܰ󶨵�����ֵ�����¼���ƥ���ֵ
            if (target is INotifyValueChanged<TValue>)
                notifyValueChanged = (INotifyValueChanged<TValue>)target;
            else
                notifyValueChanged = null;
        }

        public override BindingMode DefaultMode { get { return BindingMode.TwoWay; } }

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
#endif
