namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources
{
    public interface ISourceProxyFactory
    {
        ISourceProxy CreateProxy(object source, SourceDescription description);
    }
}

