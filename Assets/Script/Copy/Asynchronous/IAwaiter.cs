#if NETFX_CORE || NET_STANDARD_2_0 || NET_4_6

using System.Runtime.CompilerServices;

namespace CFM.Framework.Asynchronous
{
    public interface IAwaiter: ICriticalNotifyCompletion
    {
        bool IsCompleted { get; }

        void GetResult();
    }

    public interface IAwaiter<T>: ICriticalNotifyCompletion
    {
        bool IsCompleted { get; }

        T GetResult();
    }
}

#endif