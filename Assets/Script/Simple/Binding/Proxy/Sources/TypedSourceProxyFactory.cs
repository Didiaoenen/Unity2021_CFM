namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources
{
    public abstract class TypedSourceProxyFactory<T> : ISourceProxyFactory where T : SourceDescription
    {
        public virtual bool IsSupported(SourceDescription description)
        {
            if (description is T)
                return true;
            return false;
        }

        public ISourceProxy CreateProxy(object source, SourceDescription description)
        {
            if (!IsSupported(description))
                return null;

            ISourceProxy proxy = null;
            if (TryCreateProxy(source, description as T, out proxy))
                return proxy;

            return proxy;
        }

        protected abstract bool TryCreateProxy(object source, T description, out ISourceProxy proxy);
    }
}

