using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using CFM.Log;
using CFM.Framework.Asynchronous;

namespace CFM.Framework.Execution
{
    public class CoroutineScheduledExecutor : AbstractExecutor, IScheduledExecutor
    {
        interface IDelayTask : Asynchronous.IAsyncResult
        {
            TimeSpan Delay { get; }

            void Run();
        }

        class OneTimeDelayTask : AsyncResult, IDelayTask
        {
            private long startTime;

            private TimeSpan delay;

            private Action command;

            private CoroutineScheduledExecutor executor;

            public OneTimeDelayTask(CoroutineScheduledExecutor executor, Action command, TimeSpan delay)
            {
                startTime = (long)(Time.fixedTime * TimeSpan.TicksPerSecond);
                this.delay = delay;
                this.executor = executor;
                this.command = command;
                this.executor.Add(this);
            }

            public TimeSpan Delay { get { return new TimeSpan(startTime + delay.Ticks - (long)(Time.fixedTime * TimeSpan.TicksPerSecond)); } }

            public override bool Cancel()
            {
                if (IsDone)
                    return false;

                if (executor.Remove(this))
                    return false;

                cancellationRequested = true;
                SetCancelled();
                return true;
            }

            public void Run()
            {
                try
                {
                    if (IsDone)
                        return;

                    if (IsCancellationRequested)
                    {
                        SetCancelled();
                    }
                    else
                    {
                        command();
                        SetResult();
                    }
                }
                catch (Exception e)
                {
                    SetException(e);
                    if (log.IsWarnEnabled)
                        log.WarnFormat("{0}", e);
                }
            }
        }

        class OneTimeDelayTask<TResult> : AsyncResult<TResult>, IDelayTask
        {
            private long startTime;

            private TimeSpan delay;

            private Func<TResult> command;

            private CoroutineScheduledExecutor executor;

            public OneTimeDelayTask(CoroutineScheduledExecutor executor, Func<TResult> command, TimeSpan delay)
            {
                startTime = (long)(Time.fixedTime * TimeSpan.TicksPerSecond);
                this.delay = delay;
                this.executor = executor;
                this.command = command;
                this.executor.Add(this);
            }

            public virtual TimeSpan Delay { get { return new TimeSpan(startTime + delay.Ticks - (long)(Time.fixedTime * TimeSpan.TicksPerSecond)); } }

            public override bool Cancel()
            {
                if (IsDone)
                    return false;

                if (!executor.Remove(this))
                    return false;

                cancellationRequested = true;
                SetCancelled();
                return true;
            }

            public virtual void Run()
            {
                try
                {
                    if (IsDone)
                        return;

                    if (IsCancellationRequested)
                    {
                        SetCancelled();
                    }
                    else
                    {
                        SetResult(command());
                    }
                }
                catch (Exception e)
                {
                    SetException(e);
                    if (log.IsWarnEnabled)
                        log.WarnFormat("{0}", e);
                }
            }
        }

        class FixedRateDelayTask : AsyncResult, IDelayTask
        {
            private long startTime;

            private TimeSpan initialDelay;

            private TimeSpan period;

            private CoroutineScheduledExecutor executor;

            private Action command;

            private int count = 0;

            public FixedRateDelayTask(CoroutineScheduledExecutor executor, Action command, TimeSpan initialDelay, TimeSpan period) : base()
            {
                startTime = (long)(Time.fixedTime * TimeSpan.TicksPerSecond);
                this.initialDelay = initialDelay;
                this.period = period;
                this.executor = executor;
                this.command = command;
                this.executor.Add(this);
            }

            public virtual TimeSpan Delay { get { return new TimeSpan(startTime + initialDelay.Ticks + period.Ticks * count - (long)(Time.fixedTime * TimeSpan.TicksPerSecond)); } }

            public override bool Cancel()
            {
                if (IsDone)
                    return false;

                executor.Remove(this);
                cancellationRequested = true;
                SetCancelled();
                return true;
            }

            public virtual void Run()
            {
                try
                {
                    if (IsDone)
                        return;

                    if (IsCancellationRequested)
                    {
                        SetCancelled();
                    }
                    else
                    {
                        Interlocked.Increment(ref count);
                        executor.Add(this);
                        command();
                    }
                }
                catch (Exception e)
                {
                    if (log.IsWarnEnabled)
                        log.WarnFormat("{0}", e);
                }
            }
        }

        class FixedDelayDelayTask : AsyncResult, IDelayTask
        {
            private TimeSpan delay;

            private long nextTime;

            private CoroutineScheduledExecutor executor;

            private Action command;

            public FixedDelayDelayTask(CoroutineScheduledExecutor executor, Action command, TimeSpan initialDelay, TimeSpan delay) : base()
            {
                this.delay = delay;
                this.executor = executor;
                this.command = command;
                nextTime = (long)(Time.fixedTime * TimeSpan.TicksPerSecond + initialDelay.Ticks);
                this.executor.Add(this);
            }

            public virtual TimeSpan Delay { get { return new TimeSpan(nextTime - (long)(Time.fixedTime * TimeSpan.TicksPerSecond)); } }

            public override bool Cancel()
            {
                if (IsDone)
                    return false;

                executor.Remove(this);
                cancellationRequested = true;
                SetCancelled();
                return true;
            }

            public virtual void Run()
            {
                try
                {
                    if (IsDone)
                        return;

                    if (IsCancellationRequested)
                    {
                        SetCancelled();
                    }
                    else
                    {
                        command();
                    }
                }
                catch (Exception e)
                {
                    if (log.IsWarnEnabled)
                        log.WarnFormat("{0}", e);
                }
                finally
                {
                    if (IsCancellationRequested)
                    {
                        SetCancelled();
                    }
                    else
                    {
                        nextTime = (long)(Time.fixedTime * TimeSpan.TicksPerSecond + delay.Ticks);
                        executor.Add(this);
                    }
                }
            }
        }

        class ComparerImpl<T> : IComparer<T> where T : IDelayTask
        {
            public int Compare(T x, T y)
            {
                if (x.Delay.Ticks == y.Delay.Ticks)
                    return 0;

                return x.Delay.Ticks > y.Delay.Ticks ? 1 : -1;
            }
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(CoroutineScheduledExecutor));

        private ComparerImpl<IDelayTask> comparer = new ComparerImpl<IDelayTask>();

        private List<IDelayTask> queue = new List<IDelayTask>();

        private bool running = false;

        public CoroutineScheduledExecutor()
        {

        }

        private void Add(IDelayTask task)
        {
            queue.Add(task);
            queue.Sort(comparer);
        }

        private bool Remove(IDelayTask task)
        {
            if (queue.Remove(task))
            {
                queue.Sort(comparer);
                return true;
            }
            return false;
        }

        public void Start()
        {
            if (running)
                return;

            running = true;

            InterceptableEnumerator ie = new InterceptableEnumerator(DoStart());
            ie.RegisterCatchBlock(e => { running = false; });
            Executors.RunOnCoroutineNoReturn(ie);
        }

        protected virtual IEnumerator DoStart()
        {
            while (running)
            {
                while (running && (queue.Count <= 0 || queue[0].Delay.Ticks > 0))
                {
                    yield return null;
                }

                if (!running)
                    yield break;

                IDelayTask task = queue[0];
                queue.RemoveAt(0);
                task.Run();
            }
        }

        public void Stop()
        {
            if (running)
                return;

            running = false;
            List<IDelayTask> list = new List<IDelayTask>(queue);
            foreach (IDelayTask task in list)
            {
                if (task != null && !task.IsDone)
                    task.Cancel();
            }
            queue.Clear();
        }

        protected virtual void Check()
        {
            if (!running)
                throw new RejectedExecutionException("The ScheduledExecutor isn't started.");
        }

        public virtual Asynchronous.IAsyncResult Schedule(Action command, long delay)
        {
            return this.Schedule(command, new TimeSpan(delay * TimeSpan.TicksPerMillisecond));
        }

        public virtual Asynchronous.IAsyncResult Schedule(Action command, TimeSpan delay)
        {
            Check();
            return new OneTimeDelayTask(this, command, delay);
        }

        public virtual IAsyncResult<TResult> Schedule<TResult>(Func<TResult> command, long delay)
        {
            return Schedule(command, new TimeSpan(delay * TimeSpan.TicksPerMillisecond));
        }

        public virtual IAsyncResult<TResult> Schedule<TResult>(Func<TResult> command, TimeSpan delay)
        {
            Check();
            return new OneTimeDelayTask<TResult>(this, command, delay);
        }

        public virtual Asynchronous.IAsyncResult ScheduleAtFixedRate(Action command, long initialDelay, long period)
        {
            return ScheduleAtFixedRate(command, new TimeSpan(initialDelay * TimeSpan.TicksPerMillisecond), new TimeSpan(period * TimeSpan.TicksPerMillisecond));
        }

        public virtual Asynchronous.IAsyncResult ScheduleAtFixedRate(Action command, TimeSpan initialDelay, TimeSpan period)
        {
            Check();
            return new FixedRateDelayTask(this, command, initialDelay, period);
        }

        public virtual Asynchronous.IAsyncResult ScheduleWithFixedDelay(Action command, long initialDelay, long delay)
        {
            return ScheduleWithFixedDelay(command, new TimeSpan(initialDelay * TimeSpan.TicksPerMillisecond), new TimeSpan(delay * TimeSpan.TicksPerMillisecond));
        }

        public virtual Asynchronous.IAsyncResult ScheduleWithFixedDelay(Action command, TimeSpan initialDelay, TimeSpan delay)
        {
            Check();
            return new FixedDelayDelayTask(this, command, initialDelay, delay);
        }

        public void Dispose()
        {
            Stop();
        }
    }
}

