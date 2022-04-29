using UnityEngine;
using Assembly_CSharp.Assets.Script.Simple.Binding.Binders;
using Assembly_CSharp.Assets.Script.Simple.Binding.Contexts;
using Assembly_CSharp.Assets.Script.Simple.Binding.Builder;

namespace Assembly_CSharp.Assets.Script.Simple.Binding
{
    public static class BehaviourBindingExtension
    {
        private static IBinder binder;

        public static IBinder Binder
        {
            get
            {
                if (binder == null)
                    binder = null;

                return binder;
            }
            set
            {
                binder = value;
            }
        }

        public static IBindingContext BindingContext(this Behaviour behaviour)
        {
            if (behaviour == null || behaviour.gameObject == null)
                return null;

            BindingContextLifecycle bindingContextLifecycle = behaviour.GetComponent<BindingContextLifecycle>();
            if (bindingContextLifecycle == null)
                bindingContextLifecycle = behaviour.gameObject.AddComponent<BindingContextLifecycle>();

            IBindingContext bindingContext = bindingContextLifecycle.BindingContext;
            if (bindingContext == null)
            {
                bindingContext = new BindingContext(behaviour, Binder);
                bindingContextLifecycle.BindingContext = bindingContext;
            }
            return bindingContext;
        }

        public static BindingSet<TBehaviour, TSource> CreateBindingSet<TBehaviour, TSource>(this TBehaviour behaviour) where TBehaviour : Behaviour
        {
            IBindingContext context = behaviour.BindingContext();
            return new BindingSet<TBehaviour, TSource>(context, behaviour);
        }

        public static BindingSet<TBehaviour, TSource> CreateBindingSet<TBehaviour, TSource>(this TBehaviour behaviour, TSource dataContext) where TBehaviour : Behaviour
        {
            IBindingContext context = behaviour.BindingContext();
            context.DataContext = dataContext;
            return new BindingSet<TBehaviour, TSource>(context, behaviour);
        }
    }
}

