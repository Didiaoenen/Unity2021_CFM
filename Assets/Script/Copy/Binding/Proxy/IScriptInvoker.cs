namespace CFM.Framework.Binding.Proxy
{
    public interface IScriptInvoker : IInvoker
    {
        object Invoke(params object[] args);
    }
}

