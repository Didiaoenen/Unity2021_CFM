using System.Collections.Generic;

using CFM.Framework.Binding.Contexts;

namespace CFM.Framework.Binding.Binders
{
    public interface IBinder
    {
        IBinding Bind(IBindingContext bindingContext, object source, object target, BindingDescription bindingDescription);

        IEnumerable<IBinding> Bind(IBindingContext bindingContext, object source, object target, IEnumerable<BindingDescription> bindingDescriptions);
    }
}

