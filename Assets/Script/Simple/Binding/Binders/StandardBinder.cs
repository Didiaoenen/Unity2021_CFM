using System.Linq;
using System.Collections.Generic;
using Assembly_CSharp.Assets.Script.Simple.Binding.Contexts;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Binders
{
    public class StandardBinder : IBinder
    {
        protected IBindingFactory factory;

        public StandardBinder(IBindingFactory factory)
        {
            this.factory = factory;
        }

        public IBinding Bind(IBindingContext bindingContext, object source, object target, BindingDescription bindingDescription)
        {
            return factory.Create(bindingContext, source, target, bindingDescription);
        }

        public IEnumerable<IBinding> Bind(IBindingContext bindingContext, object source, object target, IEnumerable<BindingDescription> bindingDescritions)
        {
            if (bindingDescritions == null)
                return new IBinding[0];

            return bindingDescritions.Select(description => Bind(bindingContext, source, target, description));
        }
    }
}

