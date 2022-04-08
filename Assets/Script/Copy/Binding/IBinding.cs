using CFM.Framework.Binding.Contexts;

namespace CFM.Framework.Binding
{
    public interface IBinding
    {
        IBindingContext BindingContext { get; set; }

        object Target { get; }

        object DataContext { get; set; }
    }
}

