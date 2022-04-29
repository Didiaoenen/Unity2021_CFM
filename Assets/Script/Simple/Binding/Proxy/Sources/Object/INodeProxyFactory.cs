using Assembly_CSharp.Assets.Script.Simple.Binding.Paths;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources.Object
{
    public interface INodeProxyFactory
    {
        ISourceProxy Create(object source, PathToken token);
    }
}

