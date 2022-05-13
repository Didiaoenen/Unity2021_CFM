using XLua;
using System;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources.Object
{
    public class LuaMethodNodeProxy : SourceProxyBase, IObtainable
    {
        private bool disposed = false;

        private IScriptInvoker invoker;

        public override Type Type { get { return typeof(IScriptInvoker); } }

        public LuaMethodNodeProxy(LuaTable source, LuaFunction function) : base(source)
        {
            if (source == null)
                throw new ArgumentException(nameof(source));

            if (function == null)
                throw new ArgumentException(nameof(function));

            invoker = new LuaInvoker(source, function);
        }

        public object GetValue()
        {
            return invoker;
        }

        public TValue GetValue<TValue>()
        {
            return (TValue)invoker;
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (invoker != null && invoker is IDisposable)
                    {
                        (invoker as IDisposable).Dispose();
                        invoker = null;
                    }
                }
                disposed = true;
                base.Dispose(disposing);
            }
        }
    }
}

