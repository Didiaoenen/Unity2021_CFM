using System;
using System.Collections.Generic;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources.Expressions
{
    public class ExpressionSourceProxy : NotifiableSourceProxyBase, IExpressionSourceProxy
    {
        private bool disposed = false;
        
        private readonly Type type;
        
        private readonly Func<object[], object> func;
        
        private readonly List<ISourceProxy> inners = new List<ISourceProxy>();
        
        private readonly object[] args;

        public override Type Type { get { return type; } }

        public ExpressionSourceProxy(object source, Func<object[], object> func, Type type, List<ISourceProxy> inners) : base(source)
        {
            this.type = type;
            this.func = func;
            this.inners = inners;

            if (source != null)
                args = new object[] { source };
            else
                args = null;

            if (this.inners == null || this.inners.Count <= 0)
                return;

            foreach (ISourceProxy proxy in this.inners)
            {
                if (proxy is INotifiable)
                    ((INotifiable)proxy).ValueChanged += OnValueChanged;
            }
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            RaiseValueChanged();
        }


        public object GetValue()
        {
            return func(args);
        }

        public TValue GetValue<TValue>()
        {
            return (TValue)GetValue();
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
                                ((INotifiable)proxy).ValueChanged -= OnValueChanged;
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

    public class ExpressionSourceProxy<T, TResult> : NotifiableSourceProxyBase, IExpressionSourceProxy
    {
        private bool disposed = false;

        private readonly Func<T, TResult> func;

        private readonly List<ISourceProxy> inners;

        public override Type Type { get { return typeof(TResult); } }

        public ExpressionSourceProxy(T source, Func<T, TResult> func, List<ISourceProxy> inners) : base(source)
        {
            this.func = func;
            this.inners = inners;

            if (this.inners == null || this.inners.Count <= 0)
                return;

            foreach (ISourceProxy proxy in this.inners)
            {
                if (proxy is INotifiable)
                    ((INotifiable)proxy).ValueChanged += OnValueChanged;
            }
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            RaiseValueChanged();
        }

        public object GetValue()
        {
            return func((T)source);
        }

        public TValue GetValue<TValue>()
        {
            return (TValue)GetValue();
        }
    }

    public class ExpressionSourceProxy<TResult> : NotifiableSourceProxyBase, IExpressionSourceProxy
    {
        private bool disposed = false;

        private readonly Func<TResult> func;

        private readonly List<ISourceProxy> inners;

        public override Type Type { get { return typeof(TResult); } }

        public ExpressionSourceProxy(Func<TResult> func, List<ISourceProxy> inners) : base(null)
        {
            this.func = func;
            this.inners = inners;

            if (this.inners == null || this.inners.Count <= 0)
                return;

            foreach (ISourceProxy proxy in this.inners)
            {
                if (proxy is INotifiable)
                    ((INotifiable)proxy).ValueChanged += OnValueChanged;
            }
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            RaiseValueChanged();
        }

        public object GetValue()
        {
            return func();
        }

        public TValue GetValue<TValue>()
        {
            return (TValue)GetValue();
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
                                ((INotifiable)proxy).ValueChanged -= OnValueChanged;

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

