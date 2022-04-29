namespace Assembly_CSharp.Assets.Script.Simple.Binding.Reflection
{
    public interface IProxyInvoker : IInvoker
    {
        IProxyMethodInfo ProxyMethodInfo { get; }
    }
}

