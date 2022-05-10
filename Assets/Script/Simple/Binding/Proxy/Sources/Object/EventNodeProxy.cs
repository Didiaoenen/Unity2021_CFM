using System;
using Assembly_CSharp.Assets.Script.Simple.Binding.Reflection;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources.Object
{
    public class EventNodeProxy : SourceProxyBase, ISourceProxy, IModifiable
    {
        protected readonly IProxyEventInfo eventInfo;

        private bool disposed = false;

        private bool isStatic = false;

        protected Delegate handler;

        public override Type Type { get { return eventInfo.HandlerType; } }

        public EventNodeProxy(IProxyEventInfo eventInfo) : this (null, eventInfo)
        {
        }

        public EventNodeProxy(object source, IProxyEventInfo eventInfo) : base(source)
        {
            this.eventInfo = eventInfo;
            isStatic = this.eventInfo.IsStatic;
        }

        public void SetValue(object value)
        {
            if (value != null && !value.GetType().Equals(Type))
                throw new ArgumentException("Binding delegate to event failed, mismatched delegate type", "value");

            Unbind(Source, handler);
            handler = value as Delegate;
            Bind(Source, handler);
        }

        public void SetValue<TValue>(TValue value)
        {
            SetValue(value as object);
        }

        protected virtual void Bind(object target, Delegate handler)
        {
            if (handler == null)
                return;

            if (eventInfo != null)
                eventInfo.Add(target, handler);
        }

        protected virtual void Unbind(object target, Delegate handler)
        {
            if (handler == null)
                return;

            if (eventInfo != null)
                eventInfo.Remove(target, handler);
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                var source = Source;
                if (isStatic || source != null)
                    Unbind(source, handler);

                handler = null;
                disposed = true;
                base.Dispose(disposing);
            }
        }
    }
}

