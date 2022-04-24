using System;
using UnityEngine;
using CFM.Framework.Interactivity;

namespace CFM.Framework.Binding.Proxy.Targets.Universal
{
    public class InteractionTargetProxy : TargetProxyBase, IObtainable
    {
        protected readonly EventHandler<InteractionEventArgs> handler;

        public InteractionTargetProxy(object target, IInteractionAction interactionAction) : base(target)
        {
            handler = (sender, args) =>
            {
                if (target is Behaviour behaviour && !behaviour.isActiveAndEnabled)
                    return;

                interactionAction.OnRequest(sender, args);
            };
        }

        public override Type Type { get { return typeof(EventHandler<InteractionEventArgs>); } }

        public override BindingMode DefaultMode { get { return BindingMode.OneWayToSource; } }

        public object GetValue()
        {
            return handler;
        }

        public TValue GetValue<TValue>()
        {
            return (TValue)GetValue();
        }
    }
}
