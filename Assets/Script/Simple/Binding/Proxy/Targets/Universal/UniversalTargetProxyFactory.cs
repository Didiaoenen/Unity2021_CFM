using Assembly_CSharp.Assets.Script.Simple.Binding.Reflection;
using Assembly_CSharp.Assets.Script.Simple.Interactivity;
using Assembly_CSharp.Assets.Script.Simple.Observables;
using System;
using System.Reflection;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Targets.Universal
{
    public class UniversalTargetProxyFactory : ITargetProxyFactory
    {
        public ITargetProxy CreateProxy(object target, BindingDescription description)
        {
            IProxyType type = target.GetType().AsProxy();
            IProxyMemberInfo memberInfo = type.GetMember(description.TargetName);
            if (memberInfo == null)
                memberInfo = type.GetMember(description.TargetName, BindingFlags.Instance | BindingFlags.NonPublic);

            if (memberInfo == null)
                throw new MissingMemberException();

            var propertyInfo = memberInfo as IProxyPropertyInfo;
            if (propertyInfo != null)
            {
                var valueType = propertyInfo.ValueType;
                if (typeof(IObservableProperty).IsAssignableFrom(valueType))
                {
                    object observaleValue = propertyInfo.GetValue(target);
                    if (observaleValue == null)
                        throw new NullReferenceException(string.Format("{0}{1}", propertyInfo.Name, propertyInfo.DeclaringType.Name));

                    return new ObservableTargetProxy(target, (IObservableProperty)observaleValue);
                }

                if (typeof(IInteractionAction).IsAssignableFrom(valueType))
                {
                    object interactionAction = propertyInfo.GetValue(target);
                    if (interactionAction == null)
                        return null;

                    return new InteractionTargetProxy(target, (IInteractionAction)interactionAction);
                }

                return new PropertyTargetProxy(target, propertyInfo);
            }

            var fieldInfo = memberInfo as IProxyFieldInfo;
            if (fieldInfo != null)
            {
                var valueType = fieldInfo.ValueType;
                if (typeof(IObservableProperty).IsAssignableFrom(valueType))
                {
                    object observableValue = fieldInfo.GetValue(target);
                    if (observableValue == null)
                        throw new NullReferenceException(string.Format("{0}{1}", fieldInfo.Name, fieldInfo.DeclaringType.Name));

                    return new ObservableTargetProxy(target, (IObservableProperty)observableValue);
                }

                return new FieldTargetProxy(target, fieldInfo);
            }

            var eventInfo = memberInfo as IProxyEventInfo;
            if (eventInfo != null)
                return new EventTargetProxy(target, eventInfo);

            var methodInfo = memberInfo as IProxyMethodInfo;
            if (methodInfo != null)
                return new MethodTargetProxy(target, methodInfo);

            return null;
        }
    }
}

