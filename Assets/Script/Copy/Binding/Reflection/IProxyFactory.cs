using System;

namespace CFM.Framework.Binding.Reflection
{
    public interface IProxyFactory
    {
        IProxyType Create(Type type);
    }
}

