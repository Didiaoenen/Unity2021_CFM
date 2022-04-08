#if NETFX_CORE || NET_STANDARD_2_0 || NET_4_6
using System;
using System.Runtime.ExceptionServices;
using System.Runtime.CompilerServices;

using UnityEngine;

namespace CFM.Framework.Asynchronous
{
    public class CoroutineAwaiter: IAwaiter, ICriticalNotifyCompletion
    {
        protected object _lock = new object();

        protected bool done = false;

        protected Exception exception;

        protected Action continuation;

        public bool IsCompleted {  get { return done; } }

        public void GetResult()
        {
            lock (_lock)
            {
                if (!done)
                    throw new Exception("");
            }

            if (exception != null)
                ExceptionDispatchInfo.Capture(exception).Throw();
        }

        public void SetResult(Exception exception)
        {
            lock (_lock)
            {
                if (done)
                    return;

                this.exception = exception;
                this.done = true;
                try
                {
                    if (this.continuation != null)
                        this.continuation();
                }
                catch (Exception) { }
                finally
                {
                    this.continuation = null;
                }
            }
        }

        public void OnCompleted(Action continuation)
        {
            UnsafeOnCompleted(continuation);
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            if (continuation == null)
                throw new ArgumentNullException("");

            lock (_lock)
            {
                if (done)
                {
                    continuation();
                }
                else
                {
                    this.continuation += continuation;
                }
            }
        }
    }

    public class CoroutineAwaiter<T> : CoroutineAwaiter, IAwaiter<T>, ICriticalNotifyCompletion
    {
        protected T result;

        public CoroutineAwaiter()
        {
            
        }

        public new T GetResult()
        {
            lock (_lock)
            {
                if (!done)
                    throw new Exception("");
            }

            if (exception != null)
                ExceptionDispatchInfo.Capture (exception).Throw();

            return result;
        }

        public void SetResult(T result, Exception exception)
        {
            lock (_lock)
            {
                if (done)
                    return;

                this.result = result;
                this.exception = exception;
                this.done = true;
                try
                {
                    if (this.continuation != null)
                        this.continuation();
                }
                catch (Exception) { }
                finally
                {
                    this.continuation = null;
                }
            }
        }
    }

    public struct AsyncOperationAwaiter: IAwaiter, ICriticalNotifyCompletion
    {
        private AsyncOperation asyncOpration;

        private Action<AsyncOperation> continuationAction;

        public AsyncOperationAwaiter(AsyncOperation asyncOperation)
        {
            this.asyncOpration = asyncOperation;
            this.continuationAction = null;
        }

        public bool IsCompleted => asyncOpration.isDone;

        public void GetResult()
        {
            if (!IsCompleted)
                throw new Exception("");

            if (continuationAction != null)
                asyncOpration.completed -= continuationAction;
            continuationAction = null;
        }

        public void OnCompleted(Action continuation)
        {
            UnsafeOnCompleted(continuation);
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            if (continuation == null)
                throw new ArgumentNullException("");

            if (asyncOpration.isDone)
            {
                continuation();
            }
            else
            {
                continuationAction = (ao) => { continuation(); };
                asyncOpration.completed += continuationAction;
            }
        }
    }

    public struct AsyncOperationAwaiter<T, TResult> : IAwaiter<TResult>, ICriticalNotifyCompletion where T : AsyncOperation
    {
        private T asyncOperation;

        private Func<T, TResult> getter;

        private Action<AsyncOperation> continuationAction;

        public AsyncOperationAwaiter(T asyncOperation, Func<T, TResult> getter)
        {
            this.asyncOperation = asyncOperation ?? throw new ArgumentNullException("");
            this.getter = getter ?? throw new ArgumentNullException("");
            this.continuationAction = null;
        }

        public bool IsCompleted => asyncOperation.isDone;

        public TResult GetResult()
        {
            if (!IsCompleted)
                throw new Exception("");

            if (continuationAction != null)
            {
                asyncOperation.completed -= continuationAction;
                continuationAction = null;
            }
            return getter(asyncOperation);
        }

        public void OnCompleted(Action continuation)
        {
            UnsafeOnCompleted(continuation);
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            if (continuation == null)
                throw new ArgumentNullException("continuation");

            if (asyncOperation.isDone)
            {
                continuation();
            }
            else
            {
                continuationAction = (ao) => { continuation(); };
                asyncOperation.completed += continuationAction;
            }
        }
    }

    public struct AsyncResultAwaiter<T> : IAwaiter, ICriticalNotifyCompletion where T : IAsyncResult
    {
        private T asyncResult;

        public AsyncResultAwaiter(T asyncResult)
        {
            if (asyncResult == null)
                throw new ArgumentException("");
            this.asyncResult = asyncResult;
        }

        public bool IsCompleted => asyncResult.IsDone;

        public void GetResult()
        {
            if (!IsCompleted)
                throw new Exception("The task is not finished yet");

            if (asyncResult.Exception != null)
                ExceptionDispatchInfo.Capture(asyncResult.Exception).Throw();
        }

        public void OnCompleted(Action continuation)
        {
            UnsafeOnCompleted(continuation);
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            if (continuation == null)
                throw new ArgumentNullException("continuation");
            asyncResult.Callbackable().OnCallback((ar) => { continuation(); });
        }
    }

    public struct AsyncResultAwaiter<T, TResult> : IAwaiter<TResult>, ICriticalNotifyCompletion where T : IAsyncResult<TResult>
    {
        private T asyncResult;

        public AsyncResultAwaiter(T asyncResult)
        {
            if (asyncResult == null)
                throw new ArgumentException("");
            this.asyncResult = asyncResult;
        }

        public bool IsCompleted => asyncResult.IsDone;

        public TResult GetResult()
        {
            if (!IsCompleted)
                throw new Exception("The task is not finished yet");

            if (asyncResult.Exception != null)
                ExceptionDispatchInfo.Capture(asyncResult.Exception).Throw();

            return this.asyncResult.Result;
        }

        public void OnCompleted(Action continuation)
        {
            UnsafeOnCompleted(continuation);
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            if (continuation == null)
                throw new ArgumentNullException("continuation");
            asyncResult.Callbackable().OnCallback((ar) => { continuation(); });
        }
    }
}
#endif
