using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CFM.Log;
using CFM.Framework.Asynchronous;

#if NETFX_CORE || !NET_LEGACY
using System.Threading.Tasks;
#endif
using System.Threading;

namespace CFM.Framework.Execution
{
    public class Executors
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Executors));

        private static object syncLock = new object();

        private static bool disposed;

        private static MainThreadExecutor executor;

#if NETFX_CORE || !NET_LEGACY
        private static int mainThreadId;
#else
        private static Thread mainThread;
#endif

#if UNITY_EDITOR
        private static Dictionary<int, Thread> threads = new Dictionary<int, Thread>();
#endif

        static void Destroy()
        {
            disposed = true;

#if UNITY_EDITOR
            lock (threads)
            {
                foreach (Thread thread in threads.Values)
                {
                    try
                    {
                        thread.Abort();
                    }
                    catch (Exception) { }
                }
                threads.Clear();
            }
#endif
        }

        static Executors()
        {
            Create();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnRuntimeCreate()
        {
            Create();
        }

        private static void CheckDisposed()
        {
            if (disposed)
                throw new ObjectDisposedException("");
        }

        private static MainThreadExecutor CreateMainThreadExecutor(bool dontDestroy, bool useFixedUpdate)
        {
            GameObject go = new GameObject("MainThreadExecutor");
            var executor = go.AddComponent<MainThreadExecutor>();
            go.hideFlags = HideFlags.HideAndDontSave;

            if (dontDestroy)
                GameObject.DontDestroyOnLoad(go);

            executor.useFixedUpdate = useFixedUpdate;
            return executor;
        }

        public static void Create(bool dontDestroy = true, bool useFixedUpdate = true)
        {
            lock (syncLock)
            {
                try
                {
                    if (executor)
                        return;

#if NET_CORE || !NET_LEGACY
                    mainThreadId = Environment.CurrentManagedThreadId;
#else
                    Thread currentThread = Thread.CurrentThread;
                    if (currentThread.ManagedThreadId > 1 || currentThread.IsThreadPoolThread)
                        throw new Exception("");

                    mainThread = currentThread;
#endif
                }
                catch (Exception e)
                {
                    if (log.IsErrorEnabled)
                        log.ErrorFormat("", e);
                }
            }
        }

        public static bool useFixedUpdate
        {
            get { return executor.useFixedUpdate; }
            set { executor.useFixedUpdate = value; }
        }

#if NETFX_CORE || !NET_LEGACY
        public static bool IsMainThread
        {
            get { return Environment.CurrentManagedThreadId == mainThreadId; }
        }
#else
        public static bool IsMainThread
        {
            get { return Thread.CurrentThread == mainThread; }
        }
#endif

        public static void RunOnMainThread(Action action, bool waitForExecution = false)
        {
            if (disposed)
                return;

            if (waitForExecution)
            {
                AsyncResult result = new AsyncResult();
                RunOnMainThread(action, result);
                result.Synchronized().WaitForResult();
                return;
            }

            if (IsMainThread)
            {
                action();
                return;
            }

            executor.Execute(action);
        }

        public static TResult RunOnMainThread<TResult>(Func<TResult> func)
        {
            if (disposed)
                return default(TResult);

            AsyncResult<TResult> result = new AsyncResult<TResult>();
            RunOnMainThread<TResult>(func, result);
            return result.Synchronized().WaitForResult();
        }

        public static void RunOnMainThread(Action action, IPromise promise)
        {
            try
            {
                CheckDisposed();

                if (IsMainThread)
                {
                    action();
                    promise.SetResult();
                    return;
                }

                executor.Execute(() =>
                {
                    try
                    {
                        action();
                        promise.SetResult();
                    }
                    catch (Exception e)
                    {
                        promise.SetException(e);
                    }
                });
            }
            catch (Exception e)
            {
                promise.SetException(e);
            }
        }

        public static void RunOnMainThread<TResult>(Func<TResult> func, IPromise<TResult> promise)
        {
            try
            {
                CheckDisposed();

                if (IsMainThread)
                {
                    promise.SetResult(func());
                    return;
                }

                executor.Execute(() =>
                {
                    try
                    {
                        promise.SetResult(func());
                    }
                    catch (Exception e)
                    {
                        promise.SetException(e);
                    }
                });
            }
            catch (Exception e)
            {
                promise.SetException(e);
            }
        }

        public static object WaitWhile(Func<bool> predicate)
        {
            if (executor != null && IsMainThread)
                return new WaitWhile(predicate);

            throw new NotSupportedException("");
        }

        protected static InterceptableEnumerator WrapEnumerator(IEnumerator routine, IPromise promise)
        {
            InterceptableEnumerator enumerator = routine is InterceptableEnumerator ? (InterceptableEnumerator)routine : new InterceptableEnumerator(routine);
            if (promise != null)
            {
                enumerator.RegisterConditionBlock(() => !(promise.IsCancellationRequested));
                enumerator.RegisterCatchBlock(e =>
                {
                    if (promise != null)
                        promise.SetException(e);

                    if (log.IsErrorEnabled)
                        log.Error(e);
                });
                enumerator.RegisterFinallyBlock(() =>
                {
                    if (promise != null && !promise.IsDone)
                    {
                        if (promise.GetType().IsSubclassOfGenericTypeDefinition(typeof(IPromise<>)))
                            promise.SetException(new Exception(""));
                        else
                            promise.SetResult();
                    }
                });
            }

            return enumerator;
        }

        public static void RunOnCoroutineNoReturn(IEnumerator routine)
        {
            if (disposed || executor == null)
                return;

            if (IsMainThread)
            {
                executor.StartCoroutine(routine);
                return;
            }

            executor.Execute(routine);
        }

        public static Coroutine RunOnCoroutineReturn(IEnumerator routine)
        {
            if (disposed || executor == null)
                return null;

            if (IsMainThread)
                return executor.StartCoroutine(routine);

            AsyncResult<Coroutine> result = new AsyncResult<Coroutine>();
            executor.Execute(() =>
            {
                try
                {
                    Coroutine coroutine = executor.StartCoroutine(routine);
                    result.SetResult(coroutine);
                }
                catch (Exception e)
                {
                    result.SetException(e);
                }
            });

            return result.Synchronized().WaitForResult();
        }

        internal static void StopCoroutine(Coroutine routine)
        {
            if (disposed || executor == null)
                return;

            if (IsMainThread)
            {
                executor.StopCoroutine(routine);
                return;
            }

            executor.Stop(routine);
        }

        internal static void DoRunOnCoroutine(IEnumerator routine, ICoroutinePromise promise)
        {
            if (disposed)
            {
                promise.SetException(new ObjectDisposedException("Executors"));
                return;
            }

            if (executor == null)
            {
                promise.SetException(new ArgumentNullException("executor"));
                return;
            }

            if (IsMainThread)
            {
                Coroutine coroutine = executor.StartCoroutine(WrapEnumerator(routine, promise));
                promise.AddCoroutine(coroutine);
                return;
            }

            executor.Execute(() =>
            {
                try
                {
                    Coroutine coroutine = executor.StartCoroutine(WrapEnumerator(routine, promise));
                    promise.AddCoroutine(coroutine);
                }
                catch (Exception e)
                {
                    promise.SetException(e);
                }
            });
        }

        public static Asynchronous.IAsyncResult RunOnCoroutine(IEnumerator routine)
        {
            CoroutineResult result = new CoroutineResult();
            DoRunOnCoroutine(routine, result);
            return result;
        }

        public static IAsyncResult<TResult> RunOnCoroutine<TResult>(Func<IPromise<TResult>, IEnumerator> func)
        {
            CoroutineResult<TResult> result = new CoroutineResult<TResult>();
            DoRunOnCoroutine(func(result), result);
            return result;
        }

        public static IProgressResult<TProgress> RunOnCoroutine<TProgress>(Func<IProgressPromise<TProgress>, IEnumerator> func)
        {
            CoroutineProgressResult<TProgress> result = new CoroutineProgressResult<TProgress>();
            DoRunOnCoroutine(func(result), result);
            return result;
        }

        public static IProgressResult<TProgress, TResult> RunOnCoroutine<TProgress, TResult>(Func<IProgressPromise<TProgress, TResult>, IEnumerator> func)
        {
            CoroutineProgressResult<TProgress, TResult> result = new CoroutineProgressResult<TProgress, TResult>();
            DoRunOnCoroutine(func(result), result);
            return result;
        }

        public static void RunOnCoroutine(IEnumerator routine, IPromise promise)
        {
            if (disposed)
            {
                promise.SetException(new ObjectDisposedException("Executors"));
                return;
            }

            if (executor == null)
            {
                promise.SetException(new ArgumentNullException("executor"));
            }

            if (IsMainThread)
            {
                executor.StartCoroutine(WrapEnumerator(routine, promise));
                return;
            }

            executor.Execute(WrapEnumerator(routine, promise));
        }

        private static void DoRunAsync(Action action)
        {
#if UNITY_EDITOR
            ThreadPool.QueueUserWorkItem((state) =>
            {
                var thread = Thread.CurrentThread;
                try
                {
                    lock (threads)
                    {
                        threads[thread.ManagedThreadId] = thread;
                    }
                    action();
                }
                finally
                {
                    lock (threads)
                    {
                        threads.Remove(thread.ManagedThreadId);
                    }
                }
            });
#elif NETFX_CORE || !NET_LEGACY
            Task.Factory.StartNew(action);
#else
            ThreadPool.QueueUserWorkItem((state) => { action(); });
#endif
        }

        public static void RunAsyncNoReturn(Action action)
        {
            DoRunAsync(action);
        }

        public static void RunAsyncNoReturn<T>(Action<T> action, T t)
        {
            DoRunAsync(() => { action(t); });
        }

        public static Asynchronous.IAsyncResult RunAsync(Action action)
        {
            AsyncResult result = new AsyncResult();
            DoRunAsync(() =>
            {
                try
                {
                    CheckDisposed();
                    action();
                    result.SetResult();
                }
                catch (Exception e)
                {
                    result.SetException(e);
                }
            });
            return result;
        }

        public static IProgressResult<TProgress> RunAsync<TProgress>(Action<IProgressPromise<TProgress>> action)
        {
            ProgressResult<TProgress> result = new ProgressResult<TProgress>();
            DoRunAsync(() =>
            {
                try
                {
                    CheckDisposed();
                    action(result);
                    if (!result.IsDone)
                        result.SetResult(null);
                }
                catch (Exception e)
                {
                    if (!result.IsDone)
                        result.SetException(e);
                }
            });
            return result;
        }

        public static IAsyncResult<TResult> RunAsync<TResult>(Action<IPromise<TResult>> action)
        {
            AsyncResult<TResult> result = new AsyncResult<TResult>();
            DoRunAsync(() =>
            {
                try
                {
                    CheckDisposed();
                    action(result);
                    if (!result.IsDone)
                        result.SetResult(null);
                }
                catch (Exception e)
                {
                    if (!result.IsDone)
                        result.SetException(e);
                }
            });
            return result;
        }

        public static IProgressResult<TProgress, TResult> RunAsync<TProgress, TResult>(Action<IProgressPromise<TProgress, TResult>> action)
        {
            ProgressResult<TProgress, TResult> result = new ProgressResult<TProgress, TResult>();
            DoRunAsync(() =>
            {
                try
                {
                    CheckDisposed();
                    action(result);
                    if (!result.IsDone)
                        result.SetResult(null);
                }
                catch (Exception e)
                {
                    if (!result.IsDone)
                        result.SetException(e);
                }
            });
            return result;
        }

        class MainThreadExecutor : MonoBehaviour
        {
            private static readonly ILog log = LogManager.GetLogger(typeof(MainThreadExecutor));

            public bool useFixedUpdate;

            private List<object> pendingQueue = new List<object>();

            private List<object> stopingQueue = new List<object>();

            private List<object> runningQueue = new List<object>();

            private List<object> stopingTempQueue = new List<object>();

            private void OnApplicationQuit()
            {
                this.StopAllCoroutines();
                Executors.Destroy();
                if (this.gameObject != null)
                    Destroy(this.gameObject);
            }

            private void Update()
            {
                if (useFixedUpdate)
                    return;

                if (pendingQueue.Count <= 0 && stopingQueue.Count <= 0)
                    return;

                this.DoStopingQueue();

                this.DoPendingQueue();
            }

            private void FixedUpdate()
            {
                if (!useFixedUpdate)
                    return;

                if (pendingQueue.Count <= 0 && stopingQueue.Count <= 0)
                    return;

                this.DoStopingQueue();

                this.DoPendingQueue();
            }

            protected void DoStopingQueue()
            {
                lock (stopingQueue)
                {
                    if (stopingQueue.Count <= 0)
                        return;

                    stopingTempQueue.Clear();
                    stopingTempQueue.AddRange(stopingQueue);
                    stopingQueue.Clear();
                }

                for (int i = 0; i < stopingTempQueue.Count; i++)
                {
                    try
                    {
                        object task = stopingTempQueue[i];
                        if (task is IEnumerator)
                        {
                            this.StopCoroutine((IEnumerator)task);
                            continue;
                        }

                        if (task is Coroutine)
                        {
                            this.StopCoroutine((Coroutine)task);
                            continue;
                        }
                    }
                    catch (Exception e)
                    {
                        if (log.IsWarnEnabled)
                            log.WarnFormat("", e);
                    }
                }
                stopingTempQueue.Clear();
            }

            protected void DoPendingQueue()
            {
                lock (pendingQueue)
                {
                    if (pendingQueue.Count <= 0)
                        return;

                    runningQueue.Clear();
                    runningQueue.AddRange(pendingQueue);
                    pendingQueue.Clear();
                }

                float startTime = Time.realtimeSinceStartup;
                for (int i = 0; i < runningQueue.Count; i++)
                {
                    try
                    {
                        object task = runningQueue[i];
                        if (task is Action)
                        {
                            ((Action)task)();
                            continue;
                        }

                        if (task is IEnumerator)
                        {
                            this.StartCoroutine((IEnumerator)task);
                            continue;
                        }
                    }
                    catch (Exception e)
                    {
                        if (log.IsWarnEnabled)
                            log.WarnFormat("", e);
                    }
                }
                runningQueue.Clear();

                float time = Time.realtimeSinceStartup - startTime;
                if (time > 0.15f)
                    log.DebugFormat("", (int)(time * 1000));
            }

            public void Execute(Action action)
            {
                if (action == null)
                    return;

                lock (pendingQueue)
                {
                    pendingQueue.Add(action);
                }
            }

            public void Execute(IEnumerator routine)
            {
                if (routine == null)
                    return;

                lock (pendingQueue)
                {
                    pendingQueue.Add(routine);
                }
            }

            public void Stop(IEnumerator routine)
            {
                if (routine == null)
                    return;

                lock (pendingQueue)
                {
                    if (pendingQueue.Contains(routine))
                    {
                        pendingQueue.Remove(routine);
                        return;
                    }
                }

                lock(stopingQueue)
                {
                    stopingQueue.Add(routine);
                }
            }

            public void Stop(Coroutine routine)
            {
                if (routine == null)
                    return;

                lock (stopingQueue)
                {
                    stopingQueue.Add(routine);
                }
            }
        }
    }
}

