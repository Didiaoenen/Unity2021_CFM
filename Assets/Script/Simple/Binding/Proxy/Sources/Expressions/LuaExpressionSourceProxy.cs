using XLua;
using System;
using System.Collections.Generic;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources.Expressions
{
    public class LuaExpressionSourceProxy : NotifiableSourceProxyBase, IExpressionSourceProxy
    {
        private bool disposed = false;

        private LuaFunction func;

        private List<ISourceProxy> inners = new List<ISourceProxy>();

        public override Type Type { get { return typeof(object); } }

        public LuaExpressionSourceProxy(object source, LuaFunction func, List<ISourceProxy> inners) : base(source)
        {
            this.func = func;
            this.inners = inners;

            foreach (ISourceProxy proxy in inners)
            {
                if (proxy is INotifiable)
                    (proxy as INotifiable).ValueChanged += OnValueChanged;
            }
        }

        public virtual object GetValue()
        {
            if (source == null)
                return null;

            object[] results = func.Call(source);
            if (results == null || results.Length <= 0)
                return null;

            return results[0];
        }

        public virtual TValue GetValue<TValue>()
        {
            return (TValue)GetValue();
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            RaiseValueChanged();
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (inners != null && inners.Count > 0)
                    {
                        foreach (ISourceProxy proxy in inners)
                        {
                            if (proxy is INotifiable)
                                (proxy as INotifiable).ValueChanged -= OnValueChanged;
                            proxy.Dispose();
                        }
                        inners.Clear();
                    }
                }
                disposed = true;
                base.Dispose(disposing);
            }
        }
    }
}

