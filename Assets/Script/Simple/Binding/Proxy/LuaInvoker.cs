using XLua;
using System;
using UnityEngine;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy
{
    public class LuaInvoker : DisposableBase, IScriptInvoker
    {
        private readonly WeakReference target;

        private LuaFunction function;

        private bool disposed = false;

        public object Target { get { return target != null && target.IsAlive ? target.Target : null; } }

        public LuaInvoker(object target, LuaFunction function)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            this.target = new WeakReference(target);
            this.function = function;
        }

        public object Invoke(params object[] args)
        {
            try
            {
                var target = Target;
                if (target == null)
                    return null;

                if (target is Behaviour behaviour && !behaviour.isActiveAndEnabled)
                    return null;

                int length = args != null ? args.Length + 1 : 1;
                object[] parameters = new object[length];
                parameters[0] = target;
                if (args != null && args.Length > 0)
                    Array.Copy(args, 0, parameters, 1, args.Length);

                return function.Call(parameters);
            }
            catch (Exception e)
            {

            }
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (function != null)
                    {
                        function.Dispose();
                        function = null;
                    }
                }
                disposed = true;
                base.Dispose(disposing);
            }
        }
    }
}

