using System.Linq;
using System.Collections.Generic;

using CFM.Framework.Binding.Contexts;

namespace CFM.Framework.Binding.Binders
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

        public IEnumerable<IBinding> Bind(IBindingContext bindingContext, object source, object target, IEnumerable<BindingDescription> BindingDirections)
        {
            return BindingDirections.Select(description => Bind(bindingContext, source, target, description));
        }
    }
}

