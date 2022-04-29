using System;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy
{
    public interface INotifiable
    {
        event EventHandler ValueChanged;
    }
}

