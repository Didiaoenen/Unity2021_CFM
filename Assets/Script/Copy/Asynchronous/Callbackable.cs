using System;

using CFM.Log;

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
                try
                {
                    if (callback == null)
                        return;

                    var list = callback.GetInvocationList();
                    callback = null;

                    foreach (Action<IAsyncResult> action in list)
                    {
                        try
                        {
                            action(result);
                        }
                        catch (Exception e)
                        {
                            if (log.IsWarnEnabled)
                                log.WarnFormat("Class[{0}] callback exception.Error:{1}", GetType(), e);
                        }
                    }
                }
                catch (Exception e)
                {
                    if (log.IsWarnEnabled)
                        log.WarnFormat("Class[{0}] callback exception.Error:{1}", GetType(), e);
                }
            }
        }

        public void OnCallback(Action<IAsyncResult> callback)
        {
            lock (_lock)
            {
                if (callback == null)
                    return;

                if (result.IsDone)
                {
                    try
                    {
                        callback(result);
                    }
                    catch (Exception e)
                    {
                        if (log.IsWarnEnabled)
                            log.WarnFormat("Class[{0}] callback exception.Error:{1}", GetType(), e);
                    }
                    return;
                }

                this.callback += callback;
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
                try
                {
                    if (callback == null)
                        return;

                    var list = callback.GetInvocationList();
                    callback = null;

                    foreach (Action<IAsyncResult<TResult>> action in list)
                    {
                        try
                        {
                            action(result);
                        }
                        catch (Exception e)
                        {
                            if (log.IsWarnEnabled)
                                log.WarnFormat("Class[{0}] callback exception.Error:{1}", GetType(), e);
                        }
                    }
                }
                catch (Exception e)
                {
                    if (log.IsWarnEnabled)
                        log.WarnFormat("Class[{0}] callback exception.Error:{1}", GetType(), e);
                }
            }
        }

        public void OnCallback(Action<IAsyncResult<TResult>> callback)
        {
            lock (_lock)
            {
                if (callback == null)
                    return;

                if (result.IsDone)
                {
                    try
                    {
                        callback(result);
                    }
                    catch (Exception e)
                    {
                        if (log.IsWarnEnabled)
                            log.WarnFormat("Class[{0}] callback exception.Error:{1}", GetType(), e);
                    }
                    return;
                }

                this.callback += callback;
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
                try
                {
                    if (callback == null)
                        return;

                    var list = callback.GetInvocationList();
                    callback = null;

                    foreach (Action<IProgressResult<TProgress>> action in list)
                    {
                        try
                        {
                            action(result);
                        }
                        catch (Exception e)
                        {
                            if (log.IsWarnEnabled)
                                log.WarnFormat("Class[{0}] callback exception.Error:{1}", GetType(), e);
                        }
                    }
                }
                catch (Exception e)
                {
                    if (log.IsWarnEnabled)
                        log.WarnFormat("Class[{0}] callback exception.Error:{1}", GetType(), e);
                }
                finally
                {
                    progressCallback = null;
                }
            }
        }

        public void RaiseOnProgressCallback(TProgress progress)
        {
            lock (_lock)
            {
                try
                {
                    if (progressCallback == null)
                        return;

                    var list = progressCallback.GetInvocationList();
                    foreach (Action<TProgress> action in list)
                    {
                        try
                        {
                            action(progress);
                        }
                        catch (Exception e)
                        {
                            if (log.IsWarnEnabled)
                                log.WarnFormat("Class[{0}] progress callback exception.Error:{1}", GetType(), e);
                        }
                    }
                }
                catch (Exception e)
                {
                    if (log.IsWarnEnabled)
                        log.WarnFormat("Class[{0}] progress callback exception.Error:{1}", GetType(), e);
                }
            }
        }

        public void OnCallback(Action<IProgressResult<TProgress>> callback)
        {
            lock (_lock)
            {
                if (callback == null)
                    return;

                if (result.IsDone)
                {
                    try
                    {
                        callback(result);
                    }
                    catch (Exception e)
                    {
                        if (log.IsWarnEnabled)
                            log.WarnFormat("Class[{0}] callback exception.Error:{1}", GetType(), e);
                    }
                    return;
                }

                this.callback += callback;
            }
        }

        public void OnProgressCallback(Action<TProgress> callback)
        {
            lock (_lock)
            {
                if (callback == null)
                    return;

                if (result.IsDone)
                {
                    try
                    {
                        callback(result.Progress);
                    }
                    catch (Exception e)
                    {
                        if (log.IsWarnEnabled)
                            log.WarnFormat("Class[{0}] progress callback exception.Error:{1}", GetType(), e);
                    }
                    return;
                }

                progressCallback += callback;
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
                try
                {
                    if (callback == null)
                        return;

                    var list = callback.GetInvocationList();
                    callback = null;

                    foreach (Action<IProgressResult<TProgress, TResult>> action in list)
                    {
                        try
                        {
                            action(result);
                        }
                        catch (Exception e)
                        {
                            if (log.IsWarnEnabled)
                                log.WarnFormat("Class[{0}] callback exception.Error:{1}", GetType(), e);
                        }
                    }
                }
                catch (Exception e)
                {
                    if (log.IsWarnEnabled)
                        log.WarnFormat("Class[{0}] callback exception.Error:{1}", GetType(), e);
                }
                finally
                {
                    progressCallback = null;
                }
            }
        }

        public void RaiseOnProgressCallback(TProgress progress)
        {
            lock (_lock)
            {
                try
                {
                    if (progressCallback == null)
                        return;

                    var list = progressCallback.GetInvocationList();
                    foreach (Action<TProgress> action in list)
                    {
                        try
                        {
                            action(progress);
                        }
                        catch (Exception e)
                        {
                            if (log.IsWarnEnabled)
                                log.WarnFormat("Class[{0}] progress callback exception.Error:{1}", GetType(), e);
                        }
                    }
                }
                catch (Exception e)
                {
                    if (log.IsWarnEnabled)
                        log.WarnFormat("Class[{0}] progress callback exception.Error:{1}", GetType(), e);
                }
            }
        }

        public void OnCallback(Action<IProgressResult<TProgress, TResult>> callback)
        {
            lock (_lock)
            {
                if (callback == null)
                    return;

                if (result.IsDone)
                {
                    try
                    {
                        callback(result);
                    }
                    catch (Exception e)
                    {
                        if (log.IsWarnEnabled)
                            log.WarnFormat("Class[{0}] callback exception.Error:{1}", GetType(), e);
                    }
                    return;
                }

                this.callback += callback;
            }
        }

        public void OnProgressCallback(Action<TProgress> callback)
        {
            lock (_lock)
            {
                if (callback == null)
                    return;

                if (result.IsDone)
                {
                    try
                    {
                        callback(result.Progress);
                    }
                    catch (Exception e)
                    {
                        if (log.IsWarnEnabled)
                            log.WarnFormat("Class[{0}] progress callback exception.Error:{1}", GetType(), e);
                    }
                    return;
                }

                progressCallback += callback;
            }
        }
    }
}

