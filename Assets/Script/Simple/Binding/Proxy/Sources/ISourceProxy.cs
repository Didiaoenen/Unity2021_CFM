using System;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources
{
    public interface ISourceProxy : IBindingProxy
    {
        Type Type { get; }

        TypeCode TypeCode { get; }

        object Source { get; }
    }
}

