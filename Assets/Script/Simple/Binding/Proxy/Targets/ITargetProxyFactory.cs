namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Targets
{
    public interface ITargetProxyFactory
    {
        ITargetProxy CreateProxy(object target, BindingDescription description);
    }
}

