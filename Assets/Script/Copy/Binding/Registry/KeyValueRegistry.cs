using System.Collections.Generic;

using CFM.Log;

namespace CFM.Framework.Binding.Registry
{
    public class KeyValueRegistry<K, V> : IKeyValueRegistry<K, V>
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(KeyValueRegistry<K, V>));

        private readonly Dictionary<K, V> lookups = new Dictionary<K, V>();

        public virtual V Find(K key)
        {
            V toReturn;
            lookups.TryGetValue(key, out toReturn);
            return toReturn;
        }

        public virtual V Find(K key, V defaultValue)
        {
            V toReturn;
            if (lookups.TryGetValue(key, out toReturn))
                return toReturn;

            return defaultValue;
        }

        public virtual void Register(K key, V value)
        {
            if (lookups.ContainsKey(key))
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("{0}", key);

                lookups[key] = value;
            }
        }
    }
}

