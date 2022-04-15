using CFM.Framework.Execution;
using CFM.Log;
using System;
using System.Collections;
using System.Threading;

namespace CFM.Framework.Asynchronous
{
    public class AsyncTask : IAsyncTask
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AsyncTask));

        private Action action;

        private Action preCallbackOnMainThread;

        private Action preCallbackOnWorkerThread;

        private Action postCallbackOnMainThread;

        private Action postCallbackOnWorkerThread;

        private Action<Exception> errorCallbackOnMainThread;

        private Action<Exception> errorCallbackOnWorkerThread;

        private Action finishCallbackOnMainThread;

        private Action finishCallbackOnWorkerThread;

        private int running = 0;

        private AsyncResult result;

        public AsyncTask(Action task, bool runOnMainThread = false)
        {
            if (task == null)
                throw new ArgumentNullException("task");

            result = new AsyncResult();
            if (runOnMainThread)
            {
                action = WrapAction(() =>
                {
                    Executors.RunOnMainThread(task, true);
                    result.SetResult();
                });
            }
            else
            {
                action = WrapAction(() =>
                {
                    task();
                    result.SetResult();
                });
            }
        }

        public AsyncTask(Action<IPromise> task, bool runOnMainThread = false, bool cancelable = false)
        {
            if (task == null)
                throw new ArgumentNullException("task");

            result = new AsyncResult(!runOnMainThread && cancelable);
            if (runOnMainThread)
            {
                action = WrapAction(() =>
                {
                    Executors.RunOnMainThread(() => task(result), true);
                    result.SetResult();
                });
            }
            else
            {
                action = WrapAction(() =>
                {
                    task(result);
                    result.SetResult();
                });
            }
        }

        public AsyncTask(IEnumerator task, bool cancelable = false)
        {
            if (task == null)
                throw new ArgumentNullException("task");

            result = new AsyncResult(cancelable);
            action = WrapAction(() =>
            {
                Executors.RunOnCoroutine(task, result);
                result.Synchronized().WaitForResult();
            });
        }

        public virtual object Result
        {
            get { return result.Result; }
        }

        public virtual Exception Exception
        {
            get { return result.Exception; }
        }

        public virtual bool IsDone
        {
            get { return result.IsDone && running == 0; }
        }

        public virtual bool IsCancelled
        {
            get { return result.IsCancelled; }
        }

        protected virtual Action WrapAction(Action action)
        {
            Action wrapAction = () =>
            {
                try
                {
                    try
                    {
                        if (preCallbackOnWorkerThread != null)
                            preCallbackOnMainThread();
                    }
                    catch (Exception e)
                    {
                        if (log.IsWarnEnabled)
                            log.WarnFormat("{0}", e);
                    }

                    if (result.IsCancellationRequested)
                    {
                        result.SetCancelled();
                        return;
                    }

                    action();
                }
                catch (Exception e)
                {
                    result.SetException(e);
                }
                finally
                {
                    try
                    {
                        if (Exception != null)
                        {
                            if (errorCallbackOnMainThread != null)
                                Executors.RunOnMainThread(() => errorCallbackOnMainThread(Exception), true);

                            if (errorCallbackOnWorkerThread != null)
                                errorCallbackOnWorkerThread(Exception);
                        }
                        else
                        {
                            if (postCallbackOnMainThread != null)
                                Executors.RunOnMainThread(postCallbackOnMainThread, true);

                            if (postCallbackOnWorkerThread != null)
                                postCallbackOnWorkerThread();
                        }
                    }
                    catch (Exception e)
                    {
                        if (log.IsWarnEnabled)
                            log.WarnFormat("{0}", e);
                    }

                    try
                    {
                        if (finishCallbackOnMainThread != null)
                            Executors.RunOnMainThread(finishCallbackOnMainThread, true);

                        if (finishCallbackOnWorkerThread != null)
                            finishCallbackOnWorkerThread();
                    }
                    catch (Exception e)
                    {
                        if (log.IsWarnEnabled)
                            log.WarnFormat("{0}", e);
                    }

                    Interlocked.Exchange(ref running, 0);
                }
            };
            return wrapAction;
        }

        public virtual bool Cancel()
        {
            return result.Cancel();
        }

        public virtual ICallbackable Callbackable()
        {
            return result.Callbackable();
        }

        public virtual ISynchronizable Synchronized()
        {
            return result.Synchronized();
        }

        public virtual object WaitForDone()
        {
            return Executors.WaitWhile(() => !IsDone);
        }

        public IAsyncTask OnPreExecute(Action callback, bool runOnMainThread = true)
        {
            if (runOnMainThread)
                preCallbackOnMainThread += callback;
            else
                preCallbackOnWorkerThread += callback;
            return this;
        }

        public IAsyncTask OnPostExecute(Action callback, bool runOnMainThread = true)
        {
            if (runOnMainThread)
                postCallbackOnMainThread += callback;
            else
                postCallbackOnWorkerThread += callback;
            return this;
        }

        public IAsyncTask OnError(Action<Exception> callback, bool runOnMainThread = true)
        {
            if (runOnMainThread)
                errorCallbackOnMainThread += callback;
            else
                errorCallbackOnWorkerThread += callback;
            return this;
        }

        public IAsyncTask OnFinished(Action callback, bool runOnMainThread = true)
        {
            if (runOnMainThread)
                finishCallbackOnMainThread += callback;
            else
                finishCallbackOnWorkerThread += callback;
            return this;
        }

        public IAsyncTask Start(int delay)
        {
            if (delay < 0)
                return Start();

            Executors.RunAsyncNoReturn(() =>
            {
                Thread.Sleep(delay);
                if (IsDone || running == 1)
                    return;

                Start();
            });
            return this;
        }

        public IAsyncTask Start()
        {
            if (IsDone)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("The task has been done!");

                return this;
            }    

            if (Interlocked.CompareExchange(ref running, 1, 0) == 1)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("The task is running!");

                return this;
            }

            try
            {
                if (preCallbackOnMainThread != null)
                    Executors.RunOnMainThread(preCallbackOnMainThread, true);
            }
            catch (Exception e)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("{0}", e);
            }

            Executors.RunAsync(action);
            
            return this;
        }
    }

    public class AsyncTask<TResult> : IAsyncTask<TResult>
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AsyncTask<TResult>));

        private Action action;

        private Action preCallbackOnMainThread;

        private Action preCallbackOnWorkerThread;

        private Action<TResult> postCallbackOnMainThread;

        private Action<TResult> postCallbackOnWorkerThread;

        private Action<Exception> errorCallbackOnMainThread;

        private Action<Exception> errorCallbackOnWorkerThread;

        private Action finishCallbackOnMainThread;

        private Action finishCallbackOnWorkerThread;

        private int running = 0;

        private AsyncResult<TResult> result;

        public AsyncTask(Func<TResult> task, bool runOnMainThread = false)
        {
            if (task == null)
                throw new ArgumentNullException();

            result = new AsyncResult<TResult>();

            if (runOnMainThread)
            {
                action = WrapAction(() =>
                {
                    return Executors.RunOnMainThread(task);
                });
            }
            else
            {
                action = WrapAction(() => task());
            }
        }

        public AsyncTask(Action<IPromise<TResult>> task, bool runOnMainThread = false, bool cancelable = false)
        {
            if (task == null)
                throw new ArgumentNullException();

            result = new AsyncResult<TResult>(!runOnMainThread && cancelable);

            if (runOnMainThread)
            {
                action = WrapAction(() =>
                {
                    Executors.RunOnMainThread(() => task(result));
                    return result.Synchronized().WaitForResult();
                });
            }
            else
            {
                action = WrapAction(() =>
                {
                    task(result);
                    return result.Synchronized().WaitForResult();
                });
            }
        }

        public AsyncTask(Func<IPromise<TResult>, IEnumerator> task, bool cancelable = false)
        {
            if (task == null)
                throw new ArgumentNullException();

            this.result = new AsyncResult<TResult>(cancelable);
            this.action = WrapAction(() =>
            {
                Executors.RunOnCoroutine(task(this.result), this.result);
                return this.result.Synchronized().WaitForResult();
            });
        }

        public virtual TResult Result
        {
            get { return result.Result; }
        }

        object IAsyncResult.Result
        {
            get { return result.Result; }
        }

        public virtual Exception Exception
        {
            get { return result.Exception; }
        }

        public virtual bool IsDone
        {
            get { return result.IsDone && running == 0; }
        }

        public bool IsCancelled
        {
            get { return result.IsCancelled; }
        }

        protected virtual Action WrapAction(Func<TResult> action)
        {
            Action wrapAction = () =>
            {
                try
                {
                    try
                    {
                        if (preCallbackOnWorkerThread != null)
                            preCallbackOnWorkerThread();
                    }
                    catch (Exception e)
                    {
                        if (log.IsWarnEnabled)
                            log.WarnFormat("{0}", e);
                    }

                    if (result.IsCancellationRequested)
                    {
                        result.SetCancelled();
                        return;
                    }

                    TResult obj = action();
                    result.SetResult(obj);
                }
                catch (Exception e)
                {
                    result.SetException(e);
                }
                finally
                {
                    try
                    {
                        if (Exception != null)
                        {
                            if (errorCallbackOnMainThread != null)
                                Executors.RunOnMainThread(() => errorCallbackOnMainThread(Exception), true);

                            if (errorCallbackOnWorkerThread != null)
                                errorCallbackOnWorkerThread(Exception);

                        }
                        else
                        {
                            if (postCallbackOnMainThread != null)
                                Executors.RunOnMainThread(() => postCallbackOnMainThread(Result), true);

                            if (postCallbackOnWorkerThread != null)
                                postCallbackOnWorkerThread(Result);
                        }
                    }
                    catch (Exception e)
                    {
                        if (log.IsWarnEnabled)
                            log.WarnFormat("{0}", e);
                    }

                    try
                    {
                        if (finishCallbackOnMainThread != null)
                            Executors.RunOnMainThread(finishCallbackOnMainThread, true);

                        if (finishCallbackOnWorkerThread != null)
                            finishCallbackOnWorkerThread();
                    }
                    catch (Exception e)
                    {
                        if (log.IsWarnEnabled)
                            log.WarnFormat("{0}", e);
                    }

                    Interlocked.Exchange(ref running, 0);
                }
            };
            return wrapAction;
        }

        public virtual bool Cancel()
        {
            return result.Cancel();
        }

        public virtual ICallbackable<TResult> Callbackable()
        {
            return result.Callbackable();
        }

        public ISynchronizable<TResult> Synchronized()
        {
            return result.Synchronized();
        }

        ICallbackable IAsyncResult.Callbackable()
        {
            return (result as IAsyncResult).Callbackable();
        }

        ISynchronizable IAsyncResult.Synchronized()
        {
            return (result as IAsyncResult).Synchronized();
        }

        public object WaitForDone()
        {
            return Executors.WaitWhile(() => !IsDone);
        }

        public IAsyncTask<TResult> OnPreExecute(Action callback, bool runOnMainThread = true)
        {
            if (runOnMainThread)
                preCallbackOnMainThread += callback;
            else
                preCallbackOnWorkerThread += callback;
            return this;
        }

        public IAsyncTask<TResult> OnPostExecute(Action<TResult> callback, bool runOnMainThread = true)
        {
            if (runOnMainThread)
                postCallbackOnMainThread += callback;
            else
                postCallbackOnWorkerThread += callback;
            return this;
        }

        public IAsyncTask<TResult> OnError(Action<Exception> callback, bool runOnMainThread = true)
        {
            if (runOnMainThread)
                errorCallbackOnMainThread += callback;
            else
                errorCallbackOnWorkerThread += callback;
            return this;
        }

        public IAsyncTask<TResult> OnFinished(Action callback, bool runOnMainThread = true)
        {
            if (runOnMainThread)
                finishCallbackOnMainThread += callback;
            else
                finishCallbackOnWorkerThread += callback;
            return this;
        }

        public IAsyncTask<TResult> Start(int delay)
        {
            if (delay <= 0)
                return Start();

            Executors.RunAsyncNoReturn(() =>
            {
                Thread.Sleep(delay);
                if (IsDone || running == 1)
                    return;

                Start();
            });

            return this;
        }

        public IAsyncTask<TResult> Start()
        {
            if (IsDone)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("The task has been done!");
                return this;
            }

            if (Interlocked.CompareExchange(ref running, 1, 0) == 1)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("The task is running!");
                return this;
            }

            try
            {
                if (preCallbackOnMainThread != null)
                    Executors.RunOnMainThread(preCallbackOnMainThread, true);
            }
            catch (Exception e)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("{0}", e);
            }

            Executors.RunAsync(action);

            return this;
        }
    }
}

