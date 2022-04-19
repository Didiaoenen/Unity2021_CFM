using System;

namespace CFM.Framework.Binding.Proxy.Sources
{
    public interface ISourceProxy : IBindingProxy
    {
        Type Type { get; }

        TypeCode TypeCode { get; }

        object Source { get; }
    }
}

