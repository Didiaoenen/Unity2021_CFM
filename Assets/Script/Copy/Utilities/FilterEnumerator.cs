using System;
using System.Collections;
using System.Collections.Generic;

namespace CFM.Framework.Utilities
{
    public class FilterEnumerator
    {
        private IEnumerator enumerator;

        private Predicate<object> match;

        public FilterEnumerator(IEnumerator enumerator, Predicate<object> match)
        {
            this.enumerator = enumerator;
            this.match = match;
        }

        public object Current { get; private set; }

        public bool MoveNext()
        {
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;
                if (!match(current))
                    continue;

                Current = current;
                return true;
            }
            return false;
        }

        public void Reset()
        {
            enumerator.Reset();
        }
    }

    public class FilterEnumerator<T> : IEnumerator<T>
    {
        private IEnumerator<T> enumerator;

        private Predicate<T> match;

        public FilterEnumerator(IEnumerator<T> enumerator, Predicate<T> match)
        {
            Current = default(T);
            this.enumerator = enumerator;
            this.match = match;
        }

        public T Current { get; private set; }

        object IEnumerator.Current { get { return this.Current; } }

        public bool MoveNext()
        {
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;
                if (!match(current))
                    continue;

                Current = current;
                return true;
            }
            return false;
        }

        public void Reset()
        {
            enumerator.Reset();
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                Reset();
                enumerator = null;
                match = null;
                disposedValue = true;
            }
        }

        ~FilterEnumerator()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

