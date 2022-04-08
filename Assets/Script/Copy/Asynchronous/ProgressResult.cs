namespace CFM.Framework.Asynchronous
{
    public class ProgressResult<TProgress> : AsyncResult, IProgressResult<TProgress>, IProgressPromise<TProgress>
    {
        private ProgressCallbackable<TProgress> callbackable;

        protected TProgress _progress;

        public ProgressResult() : this(false)
        {

        }

        public ProgressResult(bool cancelable) : base(cancelable)
        {

        }

        public virtual TProgress Progress
        {
            get { return this._progress; }
        }

        protected override void RaiseOnCallback()
        {
            base.RaiseOnCallback();
        }

        protected virtual void RaiseOnProgressCallback(TProgress progress)
        {

        }

        public new virtual IProgressCallbackable<TProgress> Callbackable()
        {
            lock (_lock)
            {
                return this.callbackable ?? (this.callbackable = new ProgressCallbackable<TProgress>(this));
            }
        }

        public virtual void UpdateProgress(TProgress progress)
        {

        }

    }

    public class ProgressResult<TProgress, TResult> : ProgressResult<TProgress>, IProgressResult<TProgress, TResult>, IProgressPromise<TProgress, TResult>
    {
        private Callbackable<TResult> callbackable;

        private ProgressCallbackable<TProgress, TResult> progressCallbackable;

        private Synchronizable<TResult> synchronizable;

        public ProgressResult() : this(false)
        {

        }

        public ProgressResult(bool cancelable) : base(cancelable)
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

        }

        protected override void RaiseOnCallback()
        {
            base.RaiseOnCallback();
        }

        protected override void RaiseOnProgressCallback(TProgress progress)
        {
            base.RaiseOnProgressCallback(progress);
        }

        public new virtual IProgressCallbackable<TProgress, TResult> Callbackable()
        {
            lock (_lock)
            {
                return this.progressCallbackable ?? (this.progressCallbackable = new ProgressCallbackable<TProgress, TResult>(this));
            }
        }

        public new virtual ISynchronizable<TResult> Synchronized()
        {
            lock (_lock)
            {
                return this.synchronizable ?? (this.synchronizable = new Synchronizable<TResult>(this, this._lock));
            }
        }

        ICallbackable<TResult> IAsyncResult<TResult>.Callbackable()
        {
            lock (_lock)
            {
                return this.callbackable ?? (this.callbackable = new Callbackable<TResult>(this));
            }
        }
    }
}

