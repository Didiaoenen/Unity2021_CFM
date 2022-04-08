using System;
using System.Threading;

namespace CFM.Framework.Messaging
{
    public interface ISubscription<T>: IDisposable
    {
        ISubscription<T> ObserveOn(SynchronizationContext context);
    }
}

