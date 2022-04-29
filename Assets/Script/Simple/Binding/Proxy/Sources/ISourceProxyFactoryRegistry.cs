namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources
{
    public interface ISourceProxyFactoryRegistry
    {
        void Register(ISourceProxyFactory factory, int priority = 100);

        void Unregister(ISourceProxyFactory factory);
    }
}

