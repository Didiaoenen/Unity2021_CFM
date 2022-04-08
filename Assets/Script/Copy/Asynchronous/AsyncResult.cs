using System;
using System.Threading;

using CFM.Framework.Execution;

namespace CFM.Framework.Asynchronous
{
    public class AsyncResult: IAsyncResult, IPromise
    {
        private bool done;

        private object result;

        private Exception exception;

        private bool cancelled;

        protected bool cancelable;

        protected bool cancellationRequested;

        protected readonly object _lock = new object();

        private Synchronizable synchronizable;

        private Callbackable callbackable;

        public AsyncResult(): this(false)
        {

        }

        public AsyncResult(bool cancelable)
        {
            this.cancelable = cancelable;
        }

        public virtual Exception Exception
        {
            get { return this.exception; }
        }

        public virtual bool IsDone
        {
            get { return this.IsDone; }
        }

        public virtual object Result
        {
            get { return this.result; }
        }

        public virtual bool IsCancellationRequested
        {
            get { return this.cancellationRequested; }
        }

        public virtual bool IsCancelled
        {
            get { return this.cancelled; }
        }

        public virtual void SetException(string error)
        {
            if (this.done)
                return;
        }

        public virtual void SetException(Exception exception)
        {

        }

        public virtual void SetResult(object result = null)
        {

        }

        public virtual void SetCancelled()
        {

        }

        public virtual bool Cancel()
        {
            return true;
        }

        protected virtual void RaiseOnCallback()
        {

        }

        public virtual ICallbackable Callbackable()
        {
            lock (_lock)
            {
                return this.callbackable ?? (this.callbackable = new Callbackable(this));
            }
        }

        public virtual ISynchronizable Synchronized()
        {
            lock (_lock)
            {
                return this.synchronizable ?? (this.synchronizable = new Synchronizable(this, this._lock));
            }
        }

        public virtual object WaitForDone()
        {
            return Executors.WaitWhile(() => !IsDone);
        }
    }

    public class AsyncResult<TResult>: AsyncResult, IAsyncResult<TResult>, IPromise<TResult>
    {
        private Synchronizable<TResult> synchronizable;

        private Callbackable<TResult> callbackable;

        public AsyncResult(): this(false)
        {
            
        }

        public AsyncResult(bool cancelable): base(cancelable)
        {

        }

        public virtual new TResult Result
        {
            get
            {
                var result = base.Result;
                return result != null ? (TResult)result : default(TResult);
            }
        }

        public virtual void SetResult(TResult result)
        {
            base.SetResult(result);
        }

        protected override void RaiseOnCallback()
        {
            base.RaiseOnCallback();
        }

        public new virtual ICallbackable<TResult> Callbackable()
        {
            lock (_lock)
            {
                return this.callbackable ?? (this.callbackable = new Callbackable<TResult>(this));
            }
        }

        public new virtual ISynchronizable<TResult> Synchronized()
        {
            lock (_lock)
            {
                return this.synchronizable ?? (this.synchronizable = new Synchronizable<TResult>(this, this._lock));
            }
        }
    }
}

