using System;
using XLua;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Targets.Lua
{
    public class LuaMethodTargetProxy : TargetProxyBase, IObtainable
    {
        private bool disposed = false;

        private IScriptInvoker invoker;

        public override BindingMode DefaultMode { get { return BindingMode.OneWayToSource; } }

        public override Type Type { get { return typeof(LuaFunction); } }

        public LuaMethodTargetProxy(object target, LuaFunction function) : base(target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            invoker = new LuaInvoker(target, function);
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

