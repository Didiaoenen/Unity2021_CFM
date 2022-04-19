using System;

namespace CFM.Framework.Binding.Proxy
{
    public class BindingProxyBase
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

