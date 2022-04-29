using Assembly_CSharp.Assets.Script.Simple.Binding.Contexts;

namespace Assembly_CSharp.Assets.Script.Simple.Binding
{
    public interface IBindingFactory
    {
        IBinding Create(IBindingContext bindingContext, object source, object target, BindingDescription bindingDescription);
    }
}

