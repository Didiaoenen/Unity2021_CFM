using CFM.Framework.Binding.Paths;

namespace CFM.Framework.Binding.Proxy.Sources.Object
{
    public interface INodeProxyFactory
    {
        ISourceProxy Create(object source, PathToken token);
    }
}

