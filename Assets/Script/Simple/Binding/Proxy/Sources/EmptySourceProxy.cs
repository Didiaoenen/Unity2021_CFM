using System;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources
{
    public class EmptySourceProxy : SourceProxyBase, IObtainable, IModifiable
    {
        public EmptySourceProxy(SourceDescription description) : base(description)
        {
        }

        public override Type Type => throw new NotImplementedException();

        public object GetValue()
        {
            throw new System.NotImplementedException();
        }

        public TValue GetValue<TValue>()
        {
            throw new System.NotImplementedException();
        }

        public void SetValue(object value)
        {
            throw new System.NotImplementedException();
        }

        public void SetValue<TValue>(TValue value)
        {
            throw new System.NotImplementedException();
        }
    }
}

