namespace CFM.Framework.Binding.Reflection
{
    public interface IProxyRegistry
    {
        void Register(IProxyMemberInfo memberInfo);

        void Unregister(IProxyMemberInfo memberInfo);
    }
}

