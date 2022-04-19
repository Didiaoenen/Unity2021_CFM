using System;
using System.Threading;

namespace CFM.Framework.ObjectPool
{
    public class ObjectPool<T> : IObjectPool<T> where T : class
    {
        private readonly Entry[] entries = null;

        private int maxSize;

        private int initialSize;

        protected readonly IObjectFactory<T> factory;

        public ObjectPool(IObjectFactory<T> factory) : this(factory, 0, Environment.ProcessorCount * 2)
        {
        }

        public ObjectPool(IObjectFactory<T> factory, int maxSize) : this(factory, 0, maxSize)
        {
        }

        public ObjectPool(IObjectFactory<T> factory, int initialSize, int maxSize)
        {
            this.factory = factory;
            this.initialSize = initialSize;
            this.maxSize = maxSize;
            entries = new Entry[maxSize];

            if (maxSize < initialSize)
                throw new ArgumentException("the maxSize must be greater than or equal to the initialSize");

            for (int i = 0; i < initialSize; i++)
            {
                entries[i].value = factory.Create(this);
            }
        }

        public int MaxSize { get { return maxSize; } }

        public int InitialSize { get { return initialSize; } }

        public virtual T Allocate()
        {
            if (disposed)
                throw new ObjectDisposedException(GetType().Name);

            T value = default(T);
            for (var i = 0; i < entries.Length; i++)
            {
                value = entries[i].value;
                if (value == null)
                    continue;
                if (Interlocked.CompareExchange(ref entries[i].value, null, value) == value)
                    return value;
            }

            return factory.Create(this);
        }

        public virtual void Free(T obj)
        {
            if (obj == null)
                return;

            if (disposed || !factory.Validate(obj))
            {
                factory.Destroy(obj);
                return;
            }

            factory.Reset(obj);
            for (var i = 0; i < entries.Length; i++)
            {
                if (Interlocked.CompareExchange(ref entries[i].value, obj, null) == null)
                    return;
            }

            factory.Destroy(obj);
        }

        protected virtual void Clear()
        {
            for (var i = 0; i < entries.Length; i++)
            {
                var value = Interlocked.Exchange(ref entries[i].value, null);
                if (value != null)
                    factory.Destroy(value);
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                Clear();
                disposed = true;
            }
        }

        ~ObjectPool()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private struct Entry
        {
            public T value;
        }
    }
}
