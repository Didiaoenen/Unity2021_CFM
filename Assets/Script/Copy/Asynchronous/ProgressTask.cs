using System;
using System.Threading;
using System.Collections;

using CFM.Log;
using CFM.Framework.Execution;

namespace CFM.Framework.Asynchronous
{
    public class ProgressTask<TProgress> : IProgressTask<TProgress>
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ProgressTask<TProgress>));

        private Action action;

        private Action preCallbackOnMainThread;

        private Action preCallbackOnWorkerThread;

        private Action postCallbackOnMainThread;

        private Action postCallbackOnWorkerThread;

        private Action<TProgress> progressCallbackOnMainThread;

        private Action<TProgress> progressCallbackOnWorkerThread;

        private Action<Exception> errorCallbackOnMainThread;

        private Action<Exception> errorCallbackOnWorkerThread;

        private Action finishCallbackOnMainThread;

        private Action finishCallbackOnWorkerThread;

        private int running = 0;

        private ProgressResult<TProgress> result;

        public ProgressTask(Action<IProgressPromise<TProgress>> task, bool runOnMainThread = false, bool cancelable = false)
        {
            if (task == null)
                throw new ArgumentNullException();

            result = new ProgressResult<TProgress>(!runOnMainThread && cancelable);
            result.Callbackable().OnProgressCallback(OnProgressChanged);
            if (runOnMainThread)
            {
                action = WrapAction(() =>
                {
                    Executors.RunOnMainThread(() => task(result), true);
                    result.Synchronized().WaitForResult();
                });
            }
            else
            {
                action = WrapAction(() =>
                {
                    task(result);
                    result.Synchronized().WaitForResult();
                });
            }
        }

        public ProgressTask(Func<IProgressPromise<TProgress>, IEnumerator> task, bool cancelable = false)
        {
            if (task == null)
                throw new ArgumentNullException();

            result = new ProgressResult<TProgress>(cancelable);
            result.Callbackable().OnProgressCallback(OnProgressChanged);
            action = WrapAction(() =>
            {
                Executors.RunOnCoroutine(task(result), result);
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

        public virtual TProgress Progress
        {
            get { return result.Progress; }
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

        protected virtual IEnumerator DoUpdateProgressOnMainThread()
        {
            while (!result.IsDone)
            {
                try
                {
                    if (progressCallbackOnMainThread != null)
                        progressCallbackOnMainThread(result.Progress);
                }
                catch (Exception e)
                {
                    if (log.IsWarnEnabled)
                        log.WarnFormat("{0}", e);
                }
                yield return null;
            }
        }

        protected virtual void OnProgressChanged(TProgress progress)
        {
            try
            {
                if (result.IsDone || progressCallbackOnWorkerThread == null)
                    return;

                progressCallbackOnWorkerThread(progress);
            }
            catch (Exception e)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("{0}", e);
            }
        }

        public bool Cancel()
        {
            return this.result.Cancel();
        }

        public IProgressCallbackable<TProgress> Callbackable()
        {
            return result.Callbackable();
        }

        ICallbackable IAsyncResult.Callbackable()
        {
            return (result as IAsyncResult).Callbackable();
        }

        public ISynchronizable Synchronized()
        {
            return result.Synchronized();
        }

        public object WaitForDone()
        {
            return Executors.WaitWhile(() => !IsDone);
        }

        public IProgressTask<TProgress> OnPreExecute(Action callback, bool runOnMainThread = true)
        {
            if (runOnMainThread)
                preCallbackOnMainThread += callback;
            else
                preCallbackOnWorkerThread += callback;
            return this;
        }

        public IProgressTask<TProgress> OnPostExecute(Action callback, bool runOnMainThread = true)
        {
            if (runOnMainThread)
                postCallbackOnMainThread += callback;
            else
                postCallbackOnWorkerThread += callback;
            return this;
        }

        public IProgressTask<TProgress> OnError(Action<Exception> callback, bool runOnMainThread = true)
        {
            if (runOnMainThread)
                errorCallbackOnMainThread += callback;
            else
                errorCallbackOnWorkerThread += callback;
            return this;
        }
        public IProgressTask<TProgress> OnProgressUpdate(Action<TProgress> callback, bool runOnMainThread = true)
        {
            if (runOnMainThread)
                progressCallbackOnMainThread += callback;
            else
                progressCallbackOnWorkerThread += callback;
            return this;
        }

        public IProgressTask<TProgress> OnFinish(Action callback, bool runOnMainThread = true)
        {
            if (runOnMainThread)
                finishCallbackOnMainThread += callback;
            else
                finishCallbackOnWorkerThread += callback;
            return this;
        }

        public IProgressTask<TProgress> Start(int delay)
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

        public IProgressTask<TProgress> Start()
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

                if (progressCallbackOnMainThread != null)
                    Executors.RunOnCoroutineNoReturn(DoUpdateProgressOnMainThread());
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

    public class ProgressTask<TProgress, TResult> : IProgressTask<TProgress, TResult>
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ProgressTask<TProgress, TResult>));

        private Action action;

        private Action preCallbackOnMainThread;

        private Action preCallbackOnWorkerThread;

        private Action<TResult> postCallbackOnMainThread;

        private Action<TResult> postCallbackOnWorkerThread;

        private Action<TProgress> progressCallbackOnMainThread;

        private Action<TProgress> progressCallbackOnWorkerThread;

        private Action<Exception> errorCallbackOnMainThread;

        private Action<Exception> errorCallbackOnWorkerThread;

        private Action finishCallbackOnMainThread;

        private Action finishCallbackOnWorkerThread;

        private int running = 0;

        private ProgressResult<TProgress, TResult> result;

        public ProgressTask(Action<IProgressPromise<TProgress, TResult>> task, bool runOnMainThread, bool cancelable = false)
        {
            if (task == null)
                throw new ArgumentNullException();

            result = new ProgressResult<TProgress, TResult>(!runOnMainThread && cancelable);
            result.Callbackable().OnProgressCallback(OnProgressChanged);

            if (runOnMainThread)
            {
                action = WrapAction(() =>
                {
                    Executors.RunOnMainThread(() => task(result), true);
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

        public ProgressTask(Func<IProgressPromise<TProgress, TResult>, IEnumerator> task, bool cancelable = false)
        {
            if (task == null)
                throw new ArgumentNullException();

            result = new ProgressResult<TProgress, TResult>(cancelable);
            result.Callbackable().OnProgressCallback(OnProgressChanged);
            action = WrapAction(() =>
            {
                Executors.RunOnCoroutine(task(this.result), this.result);
                return result.Synchronized().WaitForResult();
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

        public virtual bool IsCancelled
        {
            get { return result.IsCancelled; }
        }

        public virtual TProgress Progress
        {
            get { return result.Progress; }
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

        protected virtual IEnumerator DoUpdateProgressOnMainThread()
        {
            while (!result.IsDone)
            {
                try
                {
                    if (progressCallbackOnMainThread != null)
                        progressCallbackOnMainThread(result.Progress);
                }
                catch (Exception e)
                {
                    if (log.IsWarnEnabled)
                        log.WarnFormat("{0}", e);
                }

                yield return null;
            }
        }

        protected virtual void OnProgressChanged(TProgress progress)
        {
            try
            {
                if (result.IsDone || progressCallbackOnWorkerThread == null)
                    return;

                progressCallbackOnWorkerThread(progress);
            }
            catch (Exception e)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("{0}", e);
            }
        }

        public virtual bool Cancel()
        {
            return result.Cancel();
        }

        public virtual IProgressCallbackable<TProgress, TResult> Callbackable()
        {
            return result.Callbackable();
        }

        public virtual ISynchronizable<TResult> Synchronized()
        {
            return result.Synchronized();
        }

        ICallbackable IAsyncResult.Callbackable()
        {
            return (result as IAsyncResult).Callbackable();
        }

        ICallbackable<TResult> IAsyncResult<TResult>.Callbackable()
        {
            return (result as IAsyncResult<TResult>).Callbackable();
        }

        IProgressCallbackable<TProgress> IProgressResult<TProgress>.Callbackable()
        {
            return (result as IProgressResult<TProgress>).Callbackable();
        }

        ISynchronizable IAsyncResult.Synchronized()
        {
            return (result as IAsyncResult).Synchronized();
        }

        public virtual object WaitForDone()
        {
            return Executors.WaitWhile(() => !IsDone);
        }

        public IProgressTask<TProgress, TResult> OnPreExecute(Action callback, bool runOnMainThread = true)
        {
            if (runOnMainThread)
                preCallbackOnMainThread += callback;
            else
                preCallbackOnWorkerThread += callback;
            return this;
        }

        public IProgressTask<TProgress, TResult> OnPostExecute(Action<TResult> callback, bool runOnMainThread = true)
        {
            if (runOnMainThread)
                postCallbackOnMainThread += callback;
            else
                postCallbackOnWorkerThread += callback;
            return this;
        }

        public IProgressTask<TProgress, TResult> OnError(Action<Exception> callback, bool runOnMainThread = true)
        {
            if (runOnMainThread)
                errorCallbackOnMainThread += callback;
            else
                errorCallbackOnWorkerThread += callback;
            return this;
        }

        public IProgressTask<TProgress, TResult> OnProgressUpdate(Action<TProgress> callback, bool runOnMainThread = true)
        {
            if (runOnMainThread)
                progressCallbackOnMainThread += callback;
            else
                progressCallbackOnWorkerThread += callback;
            return this;
        }

        public IProgressTask<TProgress, TResult> OnFinish(Action callback, bool runOnMainThread = true)
        {
            if (runOnMainThread)
                finishCallbackOnMainThread += callback;
            else
                finishCallbackOnWorkerThread += callback;
            return this;
        }

        public IProgressTask<TProgress, TResult> Start(int delay)
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

        public IProgressTask<TProgress, TResult> Start()
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

                if (progressCallbackOnMainThread != null)
                    Executors.RunOnCoroutineNoReturn(DoUpdateProgressOnMainThread());
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

