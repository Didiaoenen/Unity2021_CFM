namespace CFM.Framework.Binding.Reflection
{
    public interface IProxyInvoker : IInvoker
    {
        IProxyMethodInfo ProxyMethodInfo { get; }
    }
}

