using System;
using System.Threading;

namespace CFM.Framework.Asynchronous
{
    public interface ISynchronizable
    {
        bool WaitForDone();

        object WaitForResult(int millisecondsTimeout = 0);

        object WaitForResult(TimeSpan timeout);
    }

    public interface ISynchronizable<TResult>: ISynchronizable
    {
        new TResult WaitForResult(int millisecondsTimeout = 0);

        new TResult WaitForResult(TimeSpan timeout);
    }

    public class Synchronizable: ISynchronizable
    {
        private IAsyncResult result;

        private object _lock;

        public Synchronizable(IAsyncResult result, object _lock)
        {
            this.result = result;
            this._lock = _lock;
        }

        public bool WaitForDone()
        {
            if (result.IsDone)
                return result.IsDone;

            lock (_lock)
            {

            }
            return result.IsDone;
        }

        public object WaitForResult(int millisecondsTimeout = 0)
        {
            return result.Result;
        }

        public object WaitForResult(TimeSpan timeout)
        {
            return result.Result;
        }
    }

    internal class Synchronizable<TResult>: ISynchronizable<TResult>
    {
        private IAsyncResult<TResult> result;

        private object _lock;

        public Synchronizable(IAsyncResult<TResult> result, object _lock)
        {
            this.result = result;
            this._lock = _lock;
        }

        public bool WaitForDone()
        {
            return result.IsDone;
        }

        public TResult WaitForResult(int millisecondsTimeout = 0)
        {
            return result.Result;
        }

        public TResult WaitForResult(TimeSpan timeout)
        {
            return result.Result;
        }

        object ISynchronizable.WaitForResult(int millisecondsTimeout)
        {
            return WaitForResult(millisecondsTimeout);
        }

        object ISynchronizable.WaitForResult(TimeSpan timeout)
        {
            return WaitForResult(timeout);
        }
    }
}

