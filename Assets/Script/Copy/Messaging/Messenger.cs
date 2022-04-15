using System;
using System.Collections.Concurrent;

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
            SubjectBase notifier;
            if (!notifiers.TryGetValue(type, out notifier))
            {
                notifier = new Subject<object>();
                if (!notifiers.TryAdd(type, notifier))
                    notifiers.TryGetValue(type, out notifier);
            }
            return (notifier as Subject<object>).Subscribe(action);
        }

        public virtual ISubscription<T> Subscribe<T>(Action<T> action)
        {
            Type type = typeof(T);
            SubjectBase notifier;
            if (!notifiers.TryGetValue(type, out notifier))
            {
                notifier = new Subject<T>();
                if (!notifiers.TryAdd(type, notifier))
                    notifiers.TryGetValue(type, out notifier);
            }
            return (notifier as Subject<T>).Subscribe(action);
        }

        public virtual ISubscription<object> Subscribe(string channel, Type type, Action<object> action)
        {
            SubjectBase notifier = null;
            ConcurrentDictionary<Type, SubjectBase> dict = null;
            if (!channelNotifiers.TryGetValue(channel, out dict))
            {
                dict = new ConcurrentDictionary<Type, SubjectBase>();
                if (!channelNotifiers.TryAdd(channel, dict))
                    channelNotifiers.TryGetValue(channel, out dict);
            }

            if (!dict.TryGetValue(type, out notifier))
            {
                notifier = new Subject<object>();
                if (!dict.TryAdd(type, notifier))
                    dict.TryGetValue(type, out notifier);
            }
            return (notifier as Subject<object>).Subscribe(action);
        }

        public virtual ISubscription<T> Subscribe<T>(string channel, Action<T> action)
        {
            SubjectBase notifier = null;
            ConcurrentDictionary<Type, SubjectBase> dict = null;
            if (!channelNotifiers.TryGetValue(channel, out dict))
            {
                dict = new ConcurrentDictionary<Type, SubjectBase>();
                if (!channelNotifiers.TryAdd(channel, dict))
                    channelNotifiers.TryGetValue(channel, out dict);
            }

            if (!dict.TryGetValue(typeof(T), out notifier))
            {
                notifier = new Subject<T>();
                if (!dict.TryAdd(typeof(T), notifier))
                    dict.TryGetValue(typeof(T), out notifier);
            }
            return (notifier as Subject<T>).Subscribe(action);
        }

        public virtual void Publish(object message)
        {
            Publish<object>(message);
        }

        public virtual void Publish<T>(T message)
        {
            if (message == null || notifiers.Count <= 0)
                return;

            Type messageType = message.GetType();
            foreach (var kv in notifiers)
            {
                try
                {
                    if (kv.Key.IsAssignableFrom(messageType))
                        kv.Value.Publish(message);
                }
                catch (Exception e)
                {
                    if (log.IsWarnEnabled)
                        log.Warn(e);
                }
            }
        }

        public virtual void Publish(string channel, object message)
        {
            Publish<object>(channel, message);
        }

        public virtual void Publish<T>(string channel, T message)
        {
            if (string.IsNullOrEmpty(channel) || message == null)
                return;

            ConcurrentDictionary<Type, SubjectBase> dict = null;
            if (!channelNotifiers.TryGetValue(channel, out dict) || dict.Count <= 0)
                return;

            Type messageType = message.GetType();
            foreach (var kv in dict)
            {
                try
                {
                    if (kv.Key.IsAssignableFrom(messageType))
                        kv.Value.Publish(message);
                }
                catch (Exception e)
                {
                    if (log.IsWarnEnabled)
                        log.Warn(e);
                }
            }
        }
    }
}

