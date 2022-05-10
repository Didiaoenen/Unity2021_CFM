using System;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources
{
    public interface ISourceProxy : IDisposable
    {
        Type Type { get; }

        TypeCode TypeCode { get; }

        object Source { get; }
    }
}

