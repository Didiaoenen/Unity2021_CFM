using System;
using System.Threading;
using System.Collections.Concurrent;

namespace CFM.Framework.Messaging
{
    public abstract class SubjectBase
    {
        public abstract void Publish(object message);
    }

    public class Subject<T>: SubjectBase
    {
        private readonly ConcurrentDictionary<string, WeakReference<Subscription>> subscriptions = new ConcurrentDictionary<string, WeakReference<Subscription>>();

        public bool IsEmpty() { return subscriptions.Count <= 0; }

        public override void Publish(object message)
        {
            throw new NotImplementedException();
        }

        public void Publish(T message)
        {

        }

        public ISubscription<T> Subscribe(Action<T> action)
        {
            return null;
        }

        private void Add(Subscription subScription)
        {

        }

        private void Remove(Subscription subscription)
        {

        }

        class Subscription: ISubscription<T>
        {
            private Subject<T> subject;

            private Action<T> action;

            private SynchronizationContext context;

            public Subscription(Subject<T> subject, Action<T> action)
            {

            }

            public void Publish(T message)
            {

            }

            public ISubscription<T> ObserveOn(SynchronizationContext context)
            {
                return null;
            }

            private bool disposed = false;

            protected virtual void Dispose(bool disposing)
            {

            }

            ~Subscription()
            {
                Dispose(false);
            }

            public void Dispose()
            {

            }
        }
    }
}

