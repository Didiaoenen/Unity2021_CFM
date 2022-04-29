using System;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Reflection
{
    public interface IProxyEventInfo : IProxyMemberInfo
    {
        Type HandlerType { get; }

        void Add(object target, Delegate handler);

        void Remove(object target, Delegate handler);
    }
}

