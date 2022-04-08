using System.Collections.Concurrent;
using System;
using System.Collections.Generic;

using CFM.Log;

namespace CFM.Framework.Messaging
{
    public class Messenger : IMessenger
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Messenger));

        private static readonly Messenger Default = new Messenger();

        private readonly ConcurrentDictionary<Type, SubjectBase> notifiers = new ConcurrentDictionary<Type, SubjectBase>();

        private readonly ConcurrentDictionary<string, ConcurrentDictionary<Type, SubjectBase>> channelNotifiers = new ConcurrentDictionary<string, ConcurrentDictionary<Type, SubjectBase>>();

        public virtual ISubscription<object> Subscribe(Type type, Action<object> action)
        {
            return null;
        }

        public virtual ISubscription<T> Subscribe<T>(Action<T> action)
        {
            return null;
        }

        public virtual ISubscription<object> Subscribe(string channel, Type type, Action<object> action)
        {
            return null;
        }

        public virtual ISubscription<T> Subscribe<T>(string channel, Action<T> action)
        {
            return null;
        }

        public virtual void Publish(object message)
        {

        }

        public virtual void Publish<T>(T message)
        {

        }

        public virtual void Publish(string channel, object message)
        {

        }

        public virtual void Publish<T>(string channel, T message)
        {

        }
    }
}

