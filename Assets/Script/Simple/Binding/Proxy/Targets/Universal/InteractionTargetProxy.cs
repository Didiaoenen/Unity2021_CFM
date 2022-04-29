using System;
using UnityEngine;
using Assembly_CSharp.Assets.Script.Simple.Interactivity;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Targets.Universal
{
    public class InteractionTargetProxy : TargetProxyBase, IObtainable
    {
        protected readonly EventHandler<InteractionEventArgs> handler;

        public override Type Type { get { return typeof(EventHandler<InteractionEventArgs>); } }

        public override BindingMode DefaultMode { get { return BindingMode.OneWayToSource; } }

        public InteractionTargetProxy(object target, IInteractionAction interactionAction) : base(target)
        {
            handler = (sender, args) =>
            {
                if (target is Behaviour behaviour && !behaviour.isActiveAndEnabled)
                    return;

                interactionAction.OnRequest(sender, args);
            };
        }

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

