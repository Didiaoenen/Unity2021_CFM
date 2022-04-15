using System;
using System.Threading;
using System.Collections.Generic;

using CFM.Framework.Asynchronous;

namespace CFM.Framework.Execution
{
    public class ThreadScheduledExecutor : AbstractExecutor, IScheduledExecutor
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

            private Action wrappedAction;

            private ThreadScheduledExecutor executor;

            public OneTimeDelayTask(ThreadScheduledExecutor executor, Action command, TimeSpan delay) : base(true)
            {
                startTime = DateTime.Now.Ticks;
                this.delay = delay;
                this.executor = executor;
                wrappedAction = () =>
                {
                    try
                    {
                        if (IsDone)
                        {
                            return;
                        }

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
                    }
                };
                this.executor.Add(this);
            }

            public virtual TimeSpan Delay { get { return new TimeSpan(startTime + delay.Ticks - DateTime.Now.Ticks); } }

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
                    Executors.RunAsyncNoReturn(() => this.wrappedAction());
                }
                catch (Exception)
                {
                }
            }
        }

        class OneTimeDelayTask<TResult> : AsyncResult<TResult>, IDelayTask
        {
            private long startTime;

            private TimeSpan delay;

            private Action wrappedAction;

            private ThreadScheduledExecutor executor;

            public OneTimeDelayTask(ThreadScheduledExecutor executor, Func<TResult> command, TimeSpan delay)
            {
                startTime = DateTime.Now.Ticks;
                this.delay = delay;
                this.executor = executor;
                wrappedAction = () =>
                {
                    try
                    {
                        if (IsDone)
                        {
                            return;
                        }

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
                    }
                };
                this.executor.Add(this);
            }

            public virtual TimeSpan Delay { get { return new TimeSpan(startTime + delay.Ticks - DateTime.Now.Ticks); } }

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
                    Executors.RunAsyncNoReturn(() => wrappedAction());
                }
                catch (Exception)
                {
                }
            }
        }

        class FixedRateDelayTask : AsyncResult, IDelayTask
        {
            private long startTime;

            private TimeSpan initialDelay;

            private TimeSpan period;

            private ThreadScheduledExecutor executor;

            private Action wrappedAction;

            private int count = 0;

            public FixedRateDelayTask(ThreadScheduledExecutor executor, Action command, TimeSpan initialDelay, TimeSpan period) : base()
            {
                startTime = DateTime.Now.Ticks;
                this.initialDelay = initialDelay;
                this.period = period;
                this.executor = executor;

                wrappedAction = () =>
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
                            //count++;
                            this.executor.Add(this);
                            command();
                        }
                    }
                    catch (Exception)
                    {
                    }
                };
                this.executor.Add(this);
            }

            public virtual TimeSpan Delay { get { return new TimeSpan(startTime + initialDelay.Ticks + period.Ticks * count - DateTime.Now.Ticks); } }

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
                    Executors.RunAsyncNoReturn(() => this.wrappedAction());
                }
                catch (Exception)
                {
                }
            }
        }

        class FixedDelayDelayTask : AsyncResult, IDelayTask
        {
            private TimeSpan delay;

            private DateTime nextTime;

            private ThreadScheduledExecutor executor;

            private Action wrappedAction;

            public FixedDelayDelayTask(ThreadScheduledExecutor executor, Action command, TimeSpan initialDelay, TimeSpan delay) : base()
            {
                this.delay = delay;
                this.executor = executor;
                nextTime = DateTime.Now + initialDelay;
                wrappedAction = () =>
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
                    catch (Exception)
                    {
                    }
                    finally
                    {
                        if (IsCancellationRequested)
                        {
                            SetCancelled();
                        }
                        else
                        {
                            //this.nextDelay = this.nextDelay.Add(this.delay);
                            nextTime = DateTime.Now + this.delay;
                            this.executor.Add(this);
                        }
                    }
                };
                this.executor.Add(this);
            }

            public virtual TimeSpan Delay { get { return this.nextTime - DateTime.Now; } }

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
                    Executors.RunAsyncNoReturn(() => this.wrappedAction());
                }
                catch (Exception)
                {
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

        private IComparer<IDelayTask> comparer = new ComparerImpl<IDelayTask>();

        private List<IDelayTask> queue = new List<IDelayTask>();

        private object _lock = new object();

        private bool running = false;

        public ThreadScheduledExecutor()
        {
        }

        public void Start()
        {
            if (running)
                return;

            running = true;
            Executors.RunAsyncNoReturn(() =>
            {
                IDelayTask task = null;
                while (running)
                {
                    lock (_lock)
                    {
                        if (queue.Count <= 0)
                        {
                            Monitor.Wait(_lock);
                            continue;
                        }

                        task = queue[0];
                        if (task.Delay.Ticks > 0)
                        {
                            Monitor.Wait(_lock, task.Delay);
                            continue;
                        }

                        queue.RemoveAt(0);
                    }

                    task.Run();
                }
            });
        }

        public void Stop()
        {
            if (!running)
                return;

            lock (_lock)
            {
                running = false;
                Monitor.PulseAll(_lock);
            }

            List<IDelayTask> list = new List<IDelayTask>(queue);
            foreach (IDelayTask task in list)
            {
                if (task != null && !task.IsDone)
                    task.Cancel();
            }
            queue.Clear();
        }

        private void Add(IDelayTask task)
        {

            lock (_lock)
            {
                queue.Add(task);
                queue.Sort(comparer);
                Monitor.PulseAll(_lock);
            }
        }

        private bool Remove(IDelayTask task)
        {
            lock (_lock)
            {
                if (queue.Remove(task))
                {
                    queue.Sort(comparer);
                    Monitor.PulseAll(_lock);
                    return true;
                }
            }
            return false;
        }

        protected virtual void Check()
        {
            if (!running)
                throw new RejectedExecutionException("The ScheduledExecutor isn't started.");
        }
        public Asynchronous.IAsyncResult Schedule(Action command, long delay)
        {
            return Schedule(command, new TimeSpan(delay * TimeSpan.TicksPerMillisecond));
        }

        public Asynchronous.IAsyncResult Schedule(Action command, TimeSpan delay)
        {
            Check();
            return new OneTimeDelayTask(this, command, delay);
        }

        public IAsyncResult<TResult> Schedule<TResult>(Func<TResult> command, long delay)
        {
            return Schedule(command, new TimeSpan(delay * TimeSpan.TicksPerMillisecond));
        }

        public virtual IAsyncResult<TResult> Schedule<TResult>(Func<TResult> command, TimeSpan delay)
        {
            Check();
            return new OneTimeDelayTask<TResult>(this, command, delay);
        }

        public Asynchronous.IAsyncResult ScheduleAtFixedRate(Action command, long initialDelay, long period)
        {
            return ScheduleAtFixedRate(command, new TimeSpan(initialDelay * TimeSpan.TicksPerMillisecond), new TimeSpan(period * TimeSpan.TicksPerMillisecond));
        }

        public Asynchronous.IAsyncResult ScheduleAtFixedRate(Action command, TimeSpan initialDelay, TimeSpan period)
        {
            Check();
            return new FixedRateDelayTask(this, command, initialDelay, period);
        }

        public Asynchronous.IAsyncResult ScheduleWithFixedDelay(Action command, long initialDelay, long delay)
        {
            return ScheduleWithFixedDelay(command, new TimeSpan(initialDelay * TimeSpan.TicksPerMillisecond), new TimeSpan(delay * TimeSpan.TicksPerMillisecond));
        }

        public Asynchronous.IAsyncResult ScheduleWithFixedDelay(Action command, TimeSpan initialDelay, TimeSpan delay)
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

