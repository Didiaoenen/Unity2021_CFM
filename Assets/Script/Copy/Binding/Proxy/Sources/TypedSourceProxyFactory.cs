namespace CFM.Framework.Binding.Proxy.Sources
{
    public abstract class TypedSourceProxyFactory<T>: ISourceProxyFactory where T: SourceDescription
    {
        public virtual bool IsSupported(SourceDescription description)
        {
            return true;
        }

        public ISourceProxy CreateProxy(object source, SourceDescription description)
        {
            return null;
        }

        protected abstract bool TryCreateProxy(object source, T description, out ISourceProxy proxy);
    }
}

