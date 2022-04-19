using System;
using System.Collections.Generic;

using CFM.Framework.Contexts;
using CFM.Framework.Binding.Builder;
using CFM.Framework.Binding.Binders;
using CFM.Framework.Binding.Contexts;

using UnityEngine;

namespace CFM.Framework.Binding
{
    public static class BehaviourBindingExtension
    {
        private static IBinder binder;

        public static IBinder Binder
        {
            get
            {
                if (binder == null)
                    binder = ContextManager.GetApplicationContext().GetService<IBinder>();

                if (binder == null)
                    throw new Exception("");

                return binder;
            }
        }

        public static IBindingContext BindingContext(this Behaviour behaviour)
        {
            if (behaviour == null || behaviour.gameObject == null)
                return null;

            BindingContextLifecycle bindingContextLifecycle = behaviour.gameObject.GetComponent<BindingContextLifecycle>();
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

        public static BindingSet<TBehaviour> CreateBindingSet<TBehaviour>(this TBehaviour behaviour) where TBehaviour : Behaviour
        {
            IBindingContext context = behaviour.BindingContext();
            return new BindingSet<TBehaviour>(context, behaviour);
        }

        public static BindingSet CreateSimpleBindindSet(this Behaviour behaviour)
        {
            IBindingContext context = behaviour.BindingContext();
            return new BindingSet(context, behaviour);
        }

        public static void SetDataContext(this Behaviour behaviour, object dataContext)
        {
            behaviour.BindingContext().DataContext = dataContext;
        }

        public static object GetDataContext(this Behaviour behaviour)
        {
            return behaviour.BindingContext().DataContext;
        }

        public static void AddBinding(this Behaviour behaviour, BindingDescription bindingDescription)
        {
            behaviour.BindingContext().Add(behaviour, bindingDescription);
        }

        public static void AddBindings(this Behaviour behaviour, IEnumerable<BindingDescription> bindingDescriptions)
        {
            behaviour.BindingContext().Add(behaviour, bindingDescriptions);
        }

        public static void AddBinding(this Behaviour behaviour, IBinding binding)
        {
            behaviour.BindingContext().Add(binding);
        }

        public static void AddBinding(this Behaviour behaviour, IBinding binding, object key = null)
        {
            behaviour.BindingContext().Add(binding, key);
        }

        public static void AddBindings(this Behaviour behaviour, IEnumerable<IBinding> bindings, object key = null)
        {
            if (bindings == null)
                return;

            behaviour.BindingContext().Add(bindings, key);
        }

        public static void AddBinding(this Behaviour behaviour, object target, BindingDescription bindingDescription, object key = null)
        {
            behaviour.BindingContext().Add(target, bindingDescription, key);
        }

        public static void AddBindings(this Behaviour behaviour, object target, IEnumerable<BindingDescription> bindingDescriptions, object key = null)
        {
            behaviour.BindingContext().Add(target, bindingDescriptions, key);
        }

        public static void AddBindings(this Behaviour behaviour, IDictionary<object, IEnumerable<BindingDescription>> bindingMap, object key = null)
        {
            if (bindingMap == null)
                return;

            IBindingContext context = behaviour.BindingContext();
            foreach (var kvp in bindingMap)
            {
                context.Add(kvp.Key, kvp.Value, key);
            }
        }

        public static void ClearBindings(this Behaviour behaviour, object key)
        {
            behaviour.BindingContext().Clear(key);
        }

        public static void ClearAllBindings(this Behaviour behaviour)
        {
            behaviour.BindingContext().Clear();
        }
    }
}

