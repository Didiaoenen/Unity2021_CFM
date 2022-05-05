using System;
using System.Reflection;
using System.Collections;
using Assembly_CSharp.Assets.Script.Simple.Interactivity;
using Assembly_CSharp.Assets.Script.Simple.Observables;
using Assembly_CSharp.Assets.Script.Simple.Binding.Paths;
using Assembly_CSharp.Assets.Script.Simple.Binding.Reflection;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources.Object
{
    public class UniversalNodeProxyFactory : INodeProxyFactory
    {
        public ISourceProxy Create(object source, PathToken token)
        {
            IPathNode node = token.Current;
            if (source == null && !node.IsStatic)
                return null;

            if (node.IsStatic)
                return CreateStaticProxy(node);

            return CreateProxy(source, node);
        }

        protected virtual ISourceProxy CreateProxy(object source, IPathNode node)
        {
            IProxyType proxyType = source.GetType().AsProxy();
            if (node is IndexedNode)
            {
                if (!(source is ICollection))
                    throw new Exception();

                var itemInfo = proxyType.GetItem();
                if (itemInfo == null)
                    throw new MissingMemberException(proxyType.Type.FullName, "Item");

                var intIndexNode = node as IntegerIndexedNode;
                if (intIndexNode != null)
                    return new IntItemNodeProxy(source as ICollection, intIndexNode.Value, itemInfo);

                var stringIndexedNode = node as StringIndexNode;
                if (stringIndexedNode != null)
                    return new StringItemNodeProxy(source as ICollection, stringIndexedNode.Value, itemInfo);

                return null;
            }

            var memberNode = node as MemberNode;
            if (memberNode == null)
                return null;

            var memberInfo = memberNode.MemberInfo;
            if (memberInfo != null && !memberInfo.DeclaringType.IsAssignableFrom(source.GetType()))
                return null;

            if (memberInfo == null)
                memberInfo = source.GetType().FindFirstMemberInfo(memberNode.Name);

            if (memberInfo == null || memberInfo.IsStatic())
                throw new MissingMemberException(proxyType.Type.FullName, memberNode.Name);

            var propertyInfo = memberInfo as PropertyInfo;
            if (propertyInfo != null)
            {
                IProxyPropertyInfo proxyPropertyInfo = propertyInfo.AsProxy();
                var valueType = proxyPropertyInfo.ValueType;
                if (typeof(IObservableProperty).IsAssignableFrom(valueType))
                {
                    object observableValue = proxyPropertyInfo.GetValue(source);
                    if (observableValue == null)
                        return null;

                    return new ObservableNodeProxy(source, observableValue as IObservableProperty);
                }
                else if (typeof(IInteractionRequest).IsAssignableFrom(valueType))
                {
                    object request = proxyPropertyInfo.GetValue(source);
                    if (request == null)
                        return null;

                    return new InteractionNodeProxy(source, request as IInteractionRequest);
                }
                else
                {
                    return new PropertyNodeProxy(source, proxyPropertyInfo);
                }
            }

            var fieldInfo = memberInfo as FieldInfo;
            if (fieldInfo != null)
            {
                IProxyFieldInfo proxyFieldInfo = fieldInfo.AsProxy();
                var valueType = proxyFieldInfo.ValueType;
                if (typeof(IObservableProperty).IsAssignableFrom(valueType))
                {
                    object observableValue = proxyFieldInfo.GetValue(source);
                    if (observableValue == null)
                        return null;

                    return new ObservableNodeProxy(source, observableValue as IObservableProperty);
                }
                else if (typeof(IInteractionRequest).IsAssignableFrom(valueType))
                {
                    object request = proxyFieldInfo.GetValue(source);
                    if (request == null)
                        return null;

                    return new InteractionNodeProxy(source, request as IInteractionRequest);
                }
                else
                {
                    return new FieldNodeProxy(source, proxyFieldInfo);
                }
            }

            var methodInfo = memberInfo as MethodInfo;
            if (methodInfo != null && methodInfo.ReturnType.Equals(typeof(void)))
                return new MethodNodeProxy(source, methodInfo.AsProxy());

            var eventInfo = memberInfo as EventInfo;
            if (eventInfo != null)
                return new EventNodeProxy(source, eventInfo.AsProxy());

            return null;
        }

        protected virtual ISourceProxy CreateStaticProxy(IPathNode node)
        {
            var memberNode = node as MemberNode;
            if (memberNode == null)
                return null;

            Type type = memberNode.Type;
            var memberInfo = memberNode.MemberInfo;
            if (memberInfo == null)
                memberInfo = type.FindFirstMemberInfo(memberNode.Name, BindingFlags.Public | BindingFlags.Static);

            if (memberInfo == null)
                throw new MissingMemberException(type.FullName, memberNode.Name);

            var propertyInfo = memberInfo as PropertyInfo;
            if (propertyInfo != null)
            {
                var proxyPropertyInfo = propertyInfo.AsProxy();
                var valueType = proxyPropertyInfo.ValueType;
                if (typeof(IObservableProperty).IsAssignableFrom(valueType))
                {
                    object observableValue = proxyPropertyInfo.GetValue(null);
                    if (observableValue == null)
                        throw new NullReferenceException(string.Format("{0},{1}", propertyInfo.Name, type.Name));

                    return new ObservableNodeProxy(observableValue as IObservableProperty);
                }
                else if (typeof(IInteractionRequest).IsAssignableFrom(valueType))
                {
                    object request = proxyPropertyInfo.GetValue(null);
                    if (request == null)
                        throw new NullReferenceException(string.Format("{0}{1}", propertyInfo.Name, type.Name));

                    return new InteractionNodeProxy(request as IInteractionRequest);
                }
                else
                {
                    return new PropertyNodeProxy(proxyPropertyInfo);
                }
            }

            var methodInfo = memberInfo as MethodInfo;
            if (methodInfo != null && methodInfo.ReturnType.Equals(typeof(void)))
                return new MethodNodeProxy(methodInfo.AsProxy());

            var eventInfo = memberInfo as EventInfo;
            if (eventInfo != null)
                return new EventNodeProxy(eventInfo.AsProxy());
        
            return null;
        }
    }
}

