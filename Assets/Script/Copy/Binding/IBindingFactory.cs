using CFM.Framework.Binding.Contexts;

namespace CFM.Framework.Binding
{
    public interface IBindingFactory
    {
        IBinding Create(IBindingContext bindingContext, object source, object target, BindingDescription bindingDescription);
    }
}

