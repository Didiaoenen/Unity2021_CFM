using System;

namespace CFM.Framework.ObjectPool
{
    public interface IObjectPool<T> : IDisposable where T : class
    {
        T Allocate();

        void Free(T obj);
    }
}

