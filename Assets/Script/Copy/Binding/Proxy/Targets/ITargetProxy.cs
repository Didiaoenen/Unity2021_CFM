using System;

namespace CFM.Framework.Binding.Proxy.Targets
{
    public interface ITargetProxy
    {
        Type Type { get; }

        TypeCode TypeCode { get; }

        object Target { get; }

        BindingMode DefaultMode { get; }
    }
}

