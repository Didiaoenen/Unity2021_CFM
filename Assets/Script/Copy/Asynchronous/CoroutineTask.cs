using System;
using System.Collections;

using UnityEngine;

using CFM.Framework.Execution;

namespace CFM.Framework.Asynchronous
{
    [Flags]
    public enum CoroutineTaskContinuationOptions
    {
        None = 0,
        OnCompleted = 1,
        OnCanceled = 2,
        OnFaulted = 4
    }

    public class CoroutineTask
    {
        private static IEnumerator DoDelay(float secondsDelay)
        {
            yield return new WaitForSecondsRealtime(secondsDelay);
        }

        public static CoroutineTask Delay(int millisecondsDelay)
        {
            return Delay(millisecondsDelay / 1000.0f);
        }

        public static CoroutineTask Delay(float secondsDelay)
        {
            return new CoroutineTask(DoDelay(secondsDelay));
        }

        public static CoroutineTask Run(Action action)
        {
            return new CoroutineTask(action);
        }

        public static CoroutineTask Run(Action<object> action, object state)
        {
            return new CoroutineTask(action, state);
        }

        public static CoroutineTask Run(IEnumerator routine)
        {
            return new CoroutineTask(routine);
        }

        public static CoroutineTask<TResult> Run<TResult>(Func<TResult> function)
        {
            return new CoroutineTask<TResult>(function);
        }

        public static CoroutineTask<TResult> Run<TResult>(Func<object, TResult> function, object state)
        {
            return new CoroutineTask<TResult>(function, state);
        }

        public static CoroutineTask<TResult> Run<TResult>(Func<IPromise<TResult>, IEnumerator> function)
        {
            return new CoroutineTask<TResult>(function);
        }

        public static CoroutineTask<TResult> Run<TResult>(Func<object, IPromise<TResult>, IEnumerator> function, object state)
        {
            return new CoroutineTask<TResult>(function, state);
        }

        public static CoroutineTask WhenAll(params CoroutineTask[] tasks)
        {
            return null;
        }

        public static CoroutineTask<TResult[]> WhenAll<TResult>(params CoroutineTask<TResult>[] tasks)
        {
            return null;
        }

        public static CoroutineTask<CoroutineTask> WhenAny(params CoroutineTask[] tasks)
        {
            return null;
        }

        public static CoroutineTask<CoroutineTask<TResult>> WhenAny<TResult>(params CoroutineTask<TResult>[] tasks)
        {
            return null;
        }

        private AsyncResult asyncResult;

        public Exception Exception { get { return asyncResult.Exception; } }

        public bool IsCompleted {  get { return asyncResult.IsDone && asyncResult.Exception == null; } }

        public bool IsCancelled { get { return asyncResult.IsDone && asyncResult.IsCancelled; } }

        public bool IsFaulted {  get { return asyncResult.IsDone && !asyncResult.IsCancelled && asyncResult.Exception != null; } }

        public bool IsDone { get { return asyncResult.IsDone; } }

        protected internal CoroutineTask(AsyncResult asyncResult)
        {
            this.asyncResult = asyncResult;
        }

        public CoroutineTask(Action action) : this(new AsyncResult())
        {
            Executors.RunOnMainThread(() =>
            {
                try
                {
                    action();
                    asyncResult.SetResult();
                }
                catch (Exception e)
                {
                    asyncResult.SetException(e);
                }
            });
        }

        public CoroutineTask(Action<object> action, object state) : this(new AsyncResult())
        {
            Executors.RunOnMainThread(() =>
            {
                try
                {
                    action(state);
                    asyncResult.SetResult();
                }
                catch (Exception e)
                {
                    asyncResult.SetException(e);
                }
            });
        }

        public CoroutineTask(IEnumerator routine) : this(new AsyncResult(true))
        {
            try
            {
                if (routine == null)
                    throw new ArgumentNullException("routine");

                Executors.RunOnCoroutine(routine, this.asyncResult);
            }
            catch (Exception e)
            {
                this.asyncResult.SetException(e);
            }
        }

        public object WaitForDone()
        {
            return asyncResult.WaitForDone();
        }

#if NET_STANDARD_2_0 || NET_4_6
        public virtual IAwaiter GetAwaiter()
        {
            return new AsyncResultAwaiter<AsyncResult>(asyncResult);
        }
#endif

        protected bool IsExecutable(IAsyncResult ar, CoroutineTaskContinuationOptions continuationOptions)
        {
            bool executable = (continuationOptions == CoroutineTaskContinuationOptions.None);
            if (!executable)
                executable = (ar.Exception == null && (continuationOptions & CoroutineTaskContinuationOptions.OnCompleted) > 0);
            if (!executable)
                executable = (ar.IsCancelled && (continuationOptions & CoroutineTaskContinuationOptions.OnCanceled) > 0);
            if (!executable)
                executable = (!ar.IsCancelled && ar.Exception != null && (continuationOptions & CoroutineTaskContinuationOptions.OnFaulted) > 0);
            return executable;
        }

        public CoroutineTask ContinueWith(Action continuationAction, CoroutineTaskContinuationOptions continuationOptions = CoroutineTaskContinuationOptions.None)
        {
            AsyncResult result = new AsyncResult(true);
            asyncResult.Callbackable().OnCallback(ar =>
            {
                try
                {
                    bool executable = IsExecutable(ar, continuationOptions);
                    if (!executable)
                    {
                        result.SetCancelled();
                        return;
                    }

                    continuationAction();
                    result.SetResult();
                }
                catch (Exception e)
                {
                    result.SetException(e);
                }
            });
            return new CoroutineTask(result);
        }

        public CoroutineTask ContinueWith(Action<CoroutineTask> continuationAction, CoroutineTaskContinuationOptions continuationOptions = CoroutineTaskContinuationOptions.None)
        {
            AsyncResult result = new AsyncResult(true);
            asyncResult.Callbackable().OnCallback(ar =>
            {
                try
                {
                    bool executable = IsExecutable(ar, continuationOptions);
                    if (!executable)
                    {
                        result.SetCancelled();
                        return;
                    }

                    continuationAction(this);
                    result.SetResult();
                }
                catch (Exception e)
                {
                    result.SetException(e);
                }
            });
            return new CoroutineTask(result);
        }

        public CoroutineTask ContinueWith(Action<CoroutineTask, object> continuationAction, object state, CoroutineTaskContinuationOptions continuationOptions = CoroutineTaskContinuationOptions.None)
        {
            AsyncResult result = new AsyncResult(true);
            asyncResult.Callbackable().OnCallback(ar =>
            {
                try
                {
                    bool executable = IsExecutable(ar, continuationOptions);
                    if (!executable)
                    {
                        result.SetCancelled();
                        return;
                    }

                    continuationAction(this, state);
                    result.SetResult();
                }
                catch (Exception e)
                {
                    result.SetException(e);
                }
            });
            return new CoroutineTask(result);
        }

        public CoroutineTask ContinueWith(IEnumerator continuationRoutine, CoroutineTaskContinuationOptions continuationOptions = CoroutineTaskContinuationOptions.None)
        {
            AsyncResult result = new AsyncResult(true);
            asyncResult.Callbackable().OnCallback(ar =>
            {
                try
                {
                    bool executable = IsExecutable(ar, continuationOptions);
                    if (!executable)
                    {
                        result.SetCancelled();
                        return;
                    }

                    Executors.RunOnCoroutine(continuationRoutine, result);
                }
                catch (Exception e)
                {
                    result.SetException(e);
                }
            });
            return new CoroutineTask(result);
        }

        public CoroutineTask ContinueWith(Func<CoroutineTask, IEnumerator> continuationFunction, CoroutineTaskContinuationOptions continuationOptions = CoroutineTaskContinuationOptions.None)
        {
            AsyncResult result = new AsyncResult(true);
            asyncResult.Callbackable().OnCallback(ar =>
            {
                try
                {
                    bool executable = IsExecutable(ar, continuationOptions);
                    if (!executable)
                    {
                        result.SetCancelled();
                        return;
                    }

                    Executors.RunOnCoroutine(continuationFunction(this), result);
                }
                catch (Exception e)
                {
                    result.SetException(e);
                }
            });
            return new CoroutineTask(result);
        }

        public CoroutineTask ContinueWith(Func<CoroutineTask, object, IEnumerator> continuationFunction, object state, CoroutineTaskContinuationOptions continuationOptions = CoroutineTaskContinuationOptions.None)
        {
            AsyncResult result = new AsyncResult(true);
            asyncResult.Callbackable().OnCallback(ar =>
            {
                try
                {
                    bool executable = IsExecutable(ar, continuationOptions);
                    if (!executable)
                    {
                        result.SetCancelled();
                        return;
                    }

                    Executors.RunOnCoroutine(continuationFunction(this, state), result);
                }
                catch (Exception e)
                {
                    result.SetException(e);
                }
            });
            return new CoroutineTask(result);
        }

        public CoroutineTask<TResult> ContinueWith<TResult>(Func<CoroutineTask, TResult> continuationFunction, CoroutineTaskContinuationOptions continuationOptions = CoroutineTaskContinuationOptions.None)
        {
            AsyncResult<TResult> result = new AsyncResult<TResult>(true);
            asyncResult.Callbackable().OnCallback(ar =>
            {
                try
                {
                    bool executable = IsExecutable(ar, continuationOptions);
                    if (!executable)
                    {
                        result.SetCancelled();
                        return;
                    }

                    TResult value = continuationFunction(this);
                    result.SetResult(value);
                }
                catch (Exception e)
                {
                    result.SetException(e);
                }
            });
            return new CoroutineTask<TResult>(result);
        }

        public CoroutineTask<TResult> ContinueWith<TResult>(Func<CoroutineTask, object, TResult> continuationFunction, object state, CoroutineTaskContinuationOptions continuationOptions = CoroutineTaskContinuationOptions.None)
        {
            AsyncResult<TResult> result = new AsyncResult<TResult>(true);
            asyncResult.Callbackable().OnCallback(ar =>
            {
                try
                {
                    bool executable = IsExecutable(ar, continuationOptions);
                    if (!executable)
                    {
                        result.SetCancelled();
                        return;
                    }

                    TResult value = continuationFunction(this, state);
                    result.SetResult(value);
                }
                catch (Exception e)
                {
                    result.SetException(e);
                }
            });
            return new CoroutineTask<TResult>(result);
        }

        public CoroutineTask<TResult> ContinueWith<TResult>(Func<CoroutineTask, IPromise<TResult>, IEnumerator> continuationFunction, CoroutineTaskContinuationOptions continuationOptions = CoroutineTaskContinuationOptions.None)
        {
            AsyncResult<TResult> result = new AsyncResult<TResult>(true);
            asyncResult.Callbackable().OnCallback(ar =>
            {
                try
                {
                    bool executable = IsExecutable(ar, continuationOptions);
                    if (!executable)
                    {
                        result.SetCancelled();
                        return;
                    }

                    Executors.RunOnCoroutine(continuationFunction(this, result), result);
                }
                catch (Exception e)
                {
                    result.SetException(e);
                }
            });
            return new CoroutineTask<TResult>(result);
        }

        public CoroutineTask<TResult> ContinueWith<TResult>(Func<CoroutineTask, object, IPromise<TResult>, IEnumerator> continuationFunction, object state, CoroutineTaskContinuationOptions continuationOptions = CoroutineTaskContinuationOptions.None)
        {
            AsyncResult<TResult> result = new AsyncResult<TResult>(true);
            asyncResult.Callbackable().OnCallback(ar =>
            {
                try
                {
                    bool executable = IsExecutable(ar, continuationOptions);
                    if (!executable)
                    {
                        result.SetCancelled();
                        return;
                    }

                    Executors.RunOnCoroutine(continuationFunction(this, state, result), result);
                }
                catch (Exception e)
                {
                    result.SetException(e);
                }
            });
            return new CoroutineTask<TResult>(result);
        }
    }

    public class CoroutineTask<TResult> : CoroutineTask
    {
        private AsyncResult<TResult> asyncResult;

        public TResult Result {  get { return asyncResult.Result; } }

        protected internal CoroutineTask(AsyncResult<TResult> asyncResult) : base(asyncResult)
        {
            this.asyncResult = asyncResult;
        }

        public CoroutineTask(Func<TResult> function) : this(new AsyncResult<TResult>())
        {
            Executors.RunOnMainThread(() =>
            {
                try
                {
                    TResult value = function();
                    asyncResult.SetResult(value);
                }
                catch (Exception e)
                {
                    asyncResult.SetException(e);
                }
            });
        }

        public CoroutineTask(Func<object, TResult> function, object state) : this(new AsyncResult<TResult>())
        {
            Executors.RunOnMainThread(() =>
            {
                try
                {
                    TResult value = function(state);
                    asyncResult.SetResult(value);
                }
                catch (Exception e)
                {
                    asyncResult.SetException(e);
                }
            });
        }

        public CoroutineTask(Func<IPromise<TResult>, IEnumerator> function) : this(new AsyncResult<TResult>(true))
        {
            try
            {
                Executors.RunOnCoroutine(function(this.asyncResult), this.asyncResult);
            }
            catch (Exception e)
            {
                asyncResult.SetException(e);
            }
        }

        public CoroutineTask(Func<object, IPromise<TResult>, IEnumerator> function, object state) : this(new AsyncResult<TResult>(true))
        {
            try
            {
                Executors.RunOnCoroutine(function(state, asyncResult), asyncResult);
            }
            catch (Exception e)
            {
                asyncResult.SetException(e);
            }
        }

#if NET_STANDARD_2_0 || NET_4_6
        public new IAwaiter<TResult> GetAwaiter()
        {
            return new AsyncResultAwaiter<AsyncResult<TResult>, TResult>(asyncResult);
        }
#endif
    }
}

