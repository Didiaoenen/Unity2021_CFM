using System;

namespace Mvvm
{
    public interface IDelayedValue
    {
        bool ValueOrSubscribe(Action<object> whenReady, ref object readyValue);
    }
}
