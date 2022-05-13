using System.Linq;
using System.Collections.Generic;
using Assembly_CSharp.Assets.Script.Simple.Binding.Contexts;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Targets;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Binders
{
    public class StandardBinder : IBinder
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

        public StandardBinder(ISourceProxyFactory sourceProxyFactory, ITargetProxyFactory targetProxyFactory)
        {
            this.sourceProxyFactory = sourceProxyFactory;
            this.targetProxyFactory = targetProxyFactory;
        }

        public IBinding Bind(IBindingContext bindingContext, object source, object target, BindingDescription bindingDescription)
        {
            return new Binding(bindingContext, source, target, bindingDescription, sourceProxyFactory, targetProxyFactory);
        }

        public IEnumerable<IBinding> Bind(IBindingContext bindingContext, object source, object target, IEnumerable<BindingDescription> bindingDescritions)
        {
            if (bindingDescritions == null)
                return new IBinding[0];

            return bindingDescritions.Select(description => Bind(bindingContext, source, target, description));
        }
    }
}

