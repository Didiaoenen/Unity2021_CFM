using System;

namespace CFM.Framework.Binding.Proxy
{
    public interface INotifiable
    {
        event EventHandler ValueChanged;
    }
}

