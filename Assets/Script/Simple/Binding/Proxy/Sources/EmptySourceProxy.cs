using System;
using System.Diagnostics;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources
{
    public class EmptySourceProxy : SourceProxyBase, IObtainable, IModifiable
    {
        private SourceDescription description;

        public override Type Type { get { return typeof(object); } }

        public EmptySourceProxy(SourceDescription description) : base(description)
        {
            this.description = description;
        }

        public object GetValue()
        {
            DebugWarning();
            return null;
        }

        public TValue GetValue<TValue>()
        {
            DebugWarning();
            return default(TValue);
        }

        public void SetValue(object value)
        {
            DebugWarning();
        }

        public void SetValue<TValue>(TValue value)
        {
            DebugWarning();
        }

        [Conditional("DEBUG")]
        private void DebugWarning()
        {
        }
    }
}

