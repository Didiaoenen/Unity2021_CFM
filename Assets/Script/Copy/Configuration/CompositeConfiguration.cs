using System;
using System.Collections.Generic;

namespace CFM.Framework.Configuration
{
    public class CompositeConfiguration : ConfigurationBase
    {
        private readonly List<IConfiguration> configurations = new List<IConfiguration>();

        private readonly IConfiguration memoryConfiguration;

        public CompositeConfiguration() : this(null)
        {
        }

        public CompositeConfiguration(List<IConfiguration> configurations)
        {
            memoryConfiguration = new MemoryConfiguration();
            this.configurations.Add(memoryConfiguration);

            if (configurations != null && configurations.Count > 0)
            {
                for (int i = 0; i < configurations.Count; i++)
                {
                    var config = configurations[i];
                    if (config == null)
                        continue;

                    AddConfiguration(config);
                }
            }
        }

        public IConfiguration GetFirstConfiguration(string key)
        {
            if (key == null)
                throw new ArgumentException("Key must not be null!");

            for (int i = 0; i < configurations.Count; i++)
            {
                IConfiguration config = configurations[i];
                if (config != null && config.ContainsKey(key))
                    return config;
            }
            return null;
        }

        public IConfiguration GetConfiguration(int index)
        {
            if (index < 0 || index >= configurations.Count)
                return null;

            return configurations[index];
        }

        public IConfiguration getMemoryConfiguration()
        {
            return memoryConfiguration;
        }

        public void AddConfiguration(IConfiguration configuration)
        {
            if (!configurations.Contains(configuration))
            {
                configurations.Insert(1, configuration);
            }
        }

        public void RemoveConfiguration(IConfiguration configuration)
        {
            if (!configuration.Equals(memoryConfiguration))
            {
                configurations.Remove(configuration);
            }
        }

        public int getNumberOfConfigurations()
        {
            return configurations.Count;
        }

        public override bool IsEmpty
        {
            get
            {
                for (int i = 0; i < configurations.Count; i++)
                {
                    IConfiguration config = configurations[i];
                    if (config != null && !config.IsEmpty)
                        return false;
                }
                return true;
            }
        }

        public override bool ContainsKey(string key)
        {
            for (int i = 0; i < configurations.Count; i++)
            {
                IConfiguration config = configurations[i];
                if (config != null && config.ContainsKey(key))
                    return true;
            }
            return false;
        }

        public override IEnumerator<string> GetKeys()
        {
            List<string> keys = new List<string>();
            for (int i = 0; i < configurations.Count; i++)
            {
                IConfiguration config = configurations[i];
                if (config == null)
                    continue;

                IEnumerator<string> j = config.GetKeys();
                while (j.MoveNext())
                {
                    string key = j.Current;
                    if (!keys.Contains(key))
                        keys.Add(key);
                }
            }
            return keys.GetEnumerator();
        }

        public override object GetProperty(string key)
        {
            for (int i = 0; i < configurations.Count; i++)
            {
                IConfiguration config = configurations[i];
                if (config != null && config.ContainsKey(key))
                    return config.GetProperty(key);
            }
            return null;
        }

        public override void AddProperty(string key, object value)
        {
            memoryConfiguration.AddProperty(key, value);
        }

        public override void SetProperty(string key, object value)
        {
            memoryConfiguration.SetProperty(key, value);
        }

        public override void RemoveProperty(string key)
        {
            memoryConfiguration.RemoveProperty(key);
        }

        public override void Clear()
        {
            memoryConfiguration.Clear();
            for (int i = configurations.Count - 1; i > 0; i--)
            {
                configurations.RemoveAt(i);
            }
        }
    }
}
