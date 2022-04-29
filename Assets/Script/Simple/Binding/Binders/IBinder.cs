using System.Collections.Generic;
using Assembly_CSharp.Assets.Script.Simple.Binding.Contexts;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Binders
{
    public interface IBinder
    {
        IBinding Bind(IBindingContext bindingContext, object source, object target, BindingDescription bindingDescription);

        IEnumerable<IBinding> Bind(IBindingContext bindingContext, object source, object target, IEnumerable<BindingDescription> bindingDescritions);
    }
}

