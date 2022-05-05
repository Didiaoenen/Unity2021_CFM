using System;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources
{
    public abstract class SourceProxyBase : BindingProxyBase, ISourceProxy
    {
        protected TypeCode typeCode = TypeCode.Empty;

        protected readonly object source;

        public abstract Type Type { get; }

        public virtual TypeCode TypeCode
        {
            get
            {
                if (typeCode == TypeCode.Empty)
                    typeCode = Type.GetTypeCode(Type);
                return typeCode;
            }
        }

        public object Source { get { return source; } }

        public SourceProxyBase(object source)
        {
            this.source = source;
        }
    }

    public abstract class NotifiableSourceProxyBase : SourceProxyBase, INotifiable
    {
        protected readonly object _lock = new object();

        protected EventHandler valueChanged;

        public virtual event EventHandler ValueChanged
        {
            add { lock (_lock) { valueChanged += value; } }
            remove { lock (_lock) { valueChanged -= value; } }
        }

        public NotifiableSourceProxyBase(object source) : base(source)
        {

        }

        protected virtual void RaiseValueChanged()
        {
            try
            {
                if (valueChanged != null)
                    valueChanged(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
            }
        }
    }
}

