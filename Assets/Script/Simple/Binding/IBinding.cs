using System;
using Assembly_CSharp.Assets.Script.Simple.Binding.Contexts;

namespace Assembly_CSharp.Assets.Script.Simple.Binding
{
    public interface IBinding : IDisposable
    {
        IBindingContext BindingContext { get; set; }

        object Target { get; }

        object DataContext { get; set; }
    }
}

