using System;

using CFM.Log;

namespace CFM.Framework.Binding.Proxy.Sources
{
    public abstract class SourceProxyBase : BindingProxyBase, ISourceProxy
    {
        protected TypeCode typeCode = TypeCode.Empty;

        protected readonly object source;

        public SourceProxyBase(object source)
        {
            this.source = source;
        }

        public abstract Type Type { get; }

        public virtual TypeCode TypeCode
        {
            get
            {
                if (typeCode == TypeCode.Empty)
                {
                    typeCode = Type.GetTypeCode(Type);
                }
                return typeCode;
            }
        }

        public virtual object Source { get { return source; } }
    }

    public abstract class NotifiableSourceProxyBase : SourceProxyBase, INotifiable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NotifiableSourceProxyBase));

        protected readonly object _lock = new object();

        protected EventHandler valueChanged;

        public NotifiableSourceProxyBase(object source) : base(source)
        {
        }

        public virtual event EventHandler ValueChanged
        {
            add
            {
                lock (_lock) { valueChanged += value; }
            }

            remove
            {
                lock (_lock) { valueChanged -= value; }
            }
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
                if (log.IsWarnEnabled)
                    log.Warn(e);
            }
        }
    }
}
