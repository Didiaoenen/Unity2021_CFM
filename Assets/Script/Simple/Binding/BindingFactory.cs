using Assembly_CSharp.Assets.Script.Simple.Binding.Contexts;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Targets;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources;

namespace Assembly_CSharp.Assets.Script.Simple.Binding
{
    public class BindingFactory : IBindingFactory
    {
        private ISourceProxyFactory sourceProxyFactory;

        private ITargetProxyFactory targetProxyFactory;

        public ISourceProxyFactory SourceProxyFactory
        {
            get { return sourceProxyFactory; }
            set { sourceProxyFactory = value; }
        }

        public ITargetProxyFactory TargetProxyFactory
        {
            get { return targetProxyFactory; }
            set { targetProxyFactory = value; }
        }

        public BindingFactory(ISourceProxyFactory sourceProxyFactory, ITargetProxyFactory targetProxyFactory)
        {
            this.sourceProxyFactory = sourceProxyFactory;
            this.targetProxyFactory = targetProxyFactory;
        }

        public IBinding Create(IBindingContext bindingContext, object source, object target, BindingDescription bindingDescription)
        {
            return new Binding(bindingContext, source, target, bindingDescription, sourceProxyFactory, targetProxyFactory);
        }
    }
}

