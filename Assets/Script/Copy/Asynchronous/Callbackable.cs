using System;
using System.Threading;
using System.Diagnostics;

using CFM.Log;
using CFM.Framework.Execution;
using CFM.Framework.Asynchronous;

namespace CFM.Framework.Asynchronous
{
    public interface ICallbackable
    {
        void OnCallback(Action<IAsyncResult> callback);
    }

    public interface ICallbackable<TResult>
    {
        void OnCallback(Action<IAsyncResult<TResult>> callback);
    }

    public interface IProgressCallbackable<TProgress>
    {
        void OnCallback(Action<IProgressResult<TProgress>> callback);

        void OnProgressCallback(Action<TProgress> callback);
    }

    public interface IProgressCallbackable<TProgress, TResult>
    {
        void OnCallback(Action<IProgressResult<TProgress, TResult>> callback);

        void OnProgressCallback(Action<TProgress> callback);
    }

    public class Callbackable : ICallbackable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Callbackable));

        private IAsyncResult result;

        private readonly object _lock = new object();

        private Action<IAsyncResult> callback;

        public Callbackable(IAsyncResult result)
        {
            this.result = result;
        }

        public void RaiseOnCallback()
        {
            lock (_lock)
            {

            }
        }

        public void OnCallback(Action<IAsyncResult> callback)
        {
            lock (_lock)
            {

            }
        }


    }

    internal class Callbackable<TResult> : ICallbackable<TResult>
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Callbackable<TResult>));

        private IAsyncResult<TResult> result;

        private readonly object _lock = new object();

        private Action<IAsyncResult<TResult>> callback;

        public Callbackable(IAsyncResult<TResult> result)
        {
            this.result = result;
        }

        public void RaiseOnCallback()
        {
            lock (_lock)
            {

            }
        }

        public void OnCallback(Action<IAsyncResult<TResult>> callback)
        {
            lock (_lock)
            {

            }
        }
    }

    internal class ProgressCallbackable<TProgress> : IProgressCallbackable<TProgress>
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ProgressCallbackable<TProgress>));

        private IProgressResult<TProgress> result;

        private readonly object _lock = new object();

        private Action<IProgressResult<TProgress>> callback;

        private Action<TProgress> progressCallback;

        public ProgressCallbackable(IProgressResult<TProgress> result)
        {
            this.result = result;
        }

        public void RaiseOnCallback()
        {
            lock (_lock)
            {

            }
        }

        public void RaiseOnProgressCallback(TProgress progress)
        {
            lock (_lock)
            {

            }
        }

        public void OnCallback(Action<IProgressResult<TProgress>> callback)
        {
            lock (_lock)
            {

            }
        }

        public void OnProgressCallback(Action<TProgress> callback)
        {
            lock (_lock)
            {

            }
        }
    }
    
    internal class ProgressCallbackable<TProgress, TResult> : IProgressCallbackable<TProgress, TResult>
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ProgressCallbackable<TProgress, TResult>));

        private IProgressResult<TProgress, TResult> result;

        private readonly object _lock = new object();

        private Action<IProgressResult<TProgress, TResult>> callback;

        private Action<TProgress> progressCallback;

        public ProgressCallbackable(IProgressResult<TProgress, TResult> result)
        {
            this.result = result;
        }

        public void RaiseOnCallback()
        {
            lock (_lock)
            {

            }
        }

        public void RaiseOnProgressCallback(TProgress progress)
        {
            lock (_lock)
            {

            }
        }

        public void OnCallback(Action<IProgressResult<TProgress, TResult>> callback)
        {
            lock (_lock)
            {

            }
        }

        public void OnProgressCallback(Action<TProgress> callback)
        {
            lock (_lock)
            {

            }
        }
    }
}

