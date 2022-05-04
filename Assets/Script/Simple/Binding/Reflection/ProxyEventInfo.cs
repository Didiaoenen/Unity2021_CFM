using System;
using System.Reflection;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Reflection
{
    public class ProxyEventInfo : IProxyEventInfo
    {
        protected EventInfo eventInfo;

        public Type DeclaringType { get { return eventInfo.DeclaringType; } }

        public string Name { get { return eventInfo.Name; } }

        public bool IsStatic { get { return eventInfo.IsStatic(); } }

        public Type HandlerType { get { return eventInfo.EventHandlerType; } }

        public ProxyEventInfo(EventInfo eventInfo)
        {
            this.eventInfo = eventInfo;
        }

        public void Add(object target, Delegate handler)
        {
            eventInfo.AddEventHandler(target, handler);
        }

        public void Remove(object target, Delegate handler)
        {
            eventInfo.RemoveEventHandler(target, handler);
        }
    }
}

