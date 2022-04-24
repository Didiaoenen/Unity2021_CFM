using System;

using CFM.Framework.Binding.Contexts;

namespace CFM.Framework.Binding
{
    public interface IBinding : IDisposable
    {
        IBindingContext BindingContext { get; set; }

        object Target { get; }

        object DataContext { get; set; }
    }
}

