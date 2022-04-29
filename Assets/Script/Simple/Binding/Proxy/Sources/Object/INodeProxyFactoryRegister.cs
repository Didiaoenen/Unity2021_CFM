namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources.Object
{
    public interface INodeProxyFactoryRegister
    {
        void Register(INodeProxyFactory factory, int priority = 100);

        void Unregister(INodeProxyFactory factory);
    }
}

