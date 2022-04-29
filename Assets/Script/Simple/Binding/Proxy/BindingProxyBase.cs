using System;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy
{
    public abstract class BindingProxyBase : IBindingProxy
    {
        protected virtual void Dispose(bool disposing)
        {
        }

        ~BindingProxyBase()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

