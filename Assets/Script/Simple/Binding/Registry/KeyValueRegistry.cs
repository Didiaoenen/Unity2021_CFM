using System.Collections.Generic;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Registry
{
    public class KeyValueRegistry<K, V> : IKeyValueRegistry<K, V>
    {
        private readonly Dictionary<K, V> lookups = new Dictionary<K, V>();

        public V Find(K key)
        {
            V toReturns;
            lookups.TryGetValue(key, out toReturns);
            return toReturns;
        }

        public V Find(K key, V defaultValue)
        {
            V toReturns;
            if (lookups.TryGetValue(key, out toReturns))
                return toReturns;
            return defaultValue;
        }

        public void Register(K key, V value)
        {
            if (lookups.ContainsKey(key))
            {

            }
            lookups[key] = value;
        }
    }
}

