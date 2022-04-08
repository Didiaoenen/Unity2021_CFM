using CFM.Framework.Binding.Reflection;

namespace CFM.Framework.Binding.Proxy.Targets
{
    public class VisualElementProxyFactory : ITargetProxyFactory
    {
        public ITargetProxy CreateProxy(object target, BindingDescription description)
        {
            throw new System.NotImplementedException();
        }

        protected virtual ITargetProxy CreateValueChangedEventProxy(object target)
        {
            return null;
        }

        protected virtual ITargetProxy CreateVisualElementPropertyProxy(object target, IProxyPropertyInfo propertyInfo)
        {
            return null;
        }

        protected virtual ITargetProxy CreateVisualElementFieldProxy(object target, IProxyFieldInfo fieldInfo)
        {
            return null;
        }
    }
}

