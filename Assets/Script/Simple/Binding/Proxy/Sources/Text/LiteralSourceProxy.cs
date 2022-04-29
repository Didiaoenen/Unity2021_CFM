using System;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources.Text
{
    public class LiteralSourceProxy : SourceProxyBase, ISourceProxy, IObtainable
    {
        public override Type Type { get { return source != null ? source.GetType() : typeof(object); } }

        public LiteralSourceProxy(object source) : base(source)
        {
        }

        public object GetValue()
        {
            return source;
        }

        public TValue GetValue<TValue>()
        {
            return (TValue)Convert.ChangeType(source, typeof(TValue));
        }
    }
}

