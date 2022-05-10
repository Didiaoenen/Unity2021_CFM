using System;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Targets
{
    public interface ITargetProxy : IDisposable
    {
        Type Type { get; }

        TypeCode TypeCode { get; }

        object Target { get; }

        BindingMode DefaultMode { get; }
    }
}

