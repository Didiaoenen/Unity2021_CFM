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
                bindingContextLifecycle = behaviour.gameObject.GetComponent<BindingContextLifecycle>();

            IBindingContext bindingContext = bindingContextLifecycle.BindingContext;
            if (bindingContext == null)
            {
                bindingContext = new BindingContext(behaviour, Binder);
                bindingContextLifecycle.BindingContext = bindingContext;
            }
            return bindingContext;
        }

        public static BindingSet<TBehaviour, TSource> CreateBindingSet<TBehaviour, TSource>(this TBehaviour behaviour) where TBehaviour: Behaviour
        {
            return null;
        }

        public static BindingSet<TBehaviour, TSource> CreateBindingSet<TBehaviour, TSource>(this TBehaviour behaviour, TSource dataContext) where TBehaviour: Behaviour
        {
            return null;
        }

        public static BindingSet<TBehaviour> CreateBindingSet<TBehaviour>(this TBehaviour behaviour) where TBehaviour: Behaviour
        {
            return null;
        }

        public static BindingSet CreateSimpleBindindSet(this Behaviour behaviour)
        {
            return null;
        }

        public static void SetDataContext(this Behaviour behaviour, object dataContext)
        {

        }

        public static object GetDataContext(this Behaviour behaviour)
        {
            return null;
        }

        public static void AddBinding(this Behaviour behaviour, IEquatable<BindingDescription> equatable)
        {

        }

        public static void AddBinding(this Behaviour behaviour, IBinding binding, object key = null)
        {

        }

        public static void AddBindings(this Behaviour behaviour, IEquatable<IBinding> binding, object key = null)
        {

        }

        public static void AddBinding(this Behaviour behaviour, object target, BindingDescription bindingDescription, object key = null)
        {

        }

        public static void AddBindings(this Behaviour behaviour, object target, IDictionary<object, IEnumerable<BindingDescription>> bindingDescriptions, object key = null)
        {

        }

        public static void AddBindings(this Behaviour behaviour, IDictionary<object, IEnumerable<BindingDescription>> bindingMap, object key = null)
        {

        }

        public static void ClearBindings(this Behaviour behaviour, object key)
        {

        }

        public static void ClearAllBindings(this Behaviour behaviour)
        {

        }
    }
}

