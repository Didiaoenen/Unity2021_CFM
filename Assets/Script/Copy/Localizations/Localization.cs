using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

using UnityEngine;

using CFM.Framework.Observables;

namespace CFM.Framework.Localizations
{
    public class Localization : ILocalization
    {
        private static readonly object _instanceLock = new object();

        private static Localization instance;

        private readonly object _lock = new object();

        private readonly ConcurrentDictionary<string, IObservableProperty> data = new ConcurrentDictionary<string, IObservableProperty>();

        private readonly List<ProviderEntry> providers = new List<ProviderEntry>();

        private CultureInfo cultureInfo;

        private EventHandler cultureInfoChanged;

        public static Localization Current
        {
            get
            {
                if (instance != null)
                    return instance;

                lock (_instanceLock)
                {
                    if (instance == null)
                        instance = new Localization();
                    return instance;
                }
            }
            set { lock (_instanceLock) { instance = value; } }
        }

        protected Localization() : this(null)
        {

        }

        protected Localization(CultureInfo cultureInfo)
        {

        }

        public event EventHandler CultureInfoChanged
        {
            add { lock (_lock) { this.cultureInfoChanged += value; } }
            remove { lock (_lock) { this.cultureInfoChanged -= value; } }
        }

        public virtual CultureInfo CultureInfo
        {
            get { return this.cultureInfo; }
            set
            {
                if (value == null || (this.cultureInfo != null && this.cultureInfo.Equals(value)))
                    return;

                this.cultureInfo = value;
                this.OnCultureInfoChanged();
            }
        }

        protected void RaiseCultureInfoChanged()
        {

        }

        protected virtual void OnCultureInfoChanged()
        {

        }

        public Task AddDataProvider(IDataProvider provider)
        {
            return DoAddDataProvider(provider);
        }


        protected virtual async Task DoAddDataProvider(IDataProvider provider)
        {

        }

        public virtual void RemoveDataProvider(IDataProvider provider)
        {

        }

        public Task Refresh()
        {
            return null;
        }

        protected virtual async Task Load(params ProviderEntry[] providers)
        {

        }

        protected virtual void OnLoadCompleted(ProviderEntry entry, Dictionary<string, object> dict)
        {

        }

        protected virtual void AddValue(string key, object value)
        {
            IObservableProperty property;
            if (!data.TryGetValue(key, out property))
            {

            }
        }

        protected virtual void OnUnloadCompleted(List<string> key)
        {

        }

        public virtual ILocalization Subset(string prefix)
        {
            return new SubsetLocalization(this, prefix);
        }

        public virtual bool ContainsKey(string key)
        {
            return data.ContainsKey(key);
        }

        public virtual string GetText(string key)
        {
            return this.GetText(key, key);
        }

        public virtual string GetText(string key, string defaultValue)
        {
            return this.Get(key, defaultValue);
        }

        public virtual string GetFormattedText(string key, params object[] args)
        {
            string format = this.Get<string>(key, null);
            if (format == null)
                return key;

            return string.Format(format, args);
        }

        public virtual bool GetBoolean(string key)
        {
            return this.Get(key, false);
        }

        public virtual bool GetBoolean(string key, bool defaultValue)
        {
            return this.Get(key, defaultValue);
        }

        public virtual int GetInt(string key)
        {
            return this.Get<int>(key);
        }

        public virtual int GetInt(string key, int defaultValue)
        {
            return this.Get(key, defaultValue);
        }

        public virtual long GetLong(string key)
        {
            return this.Get<long>(key);
        }

        public virtual long GetLong(string key, long defaultValue)
        {
            return this.Get(key, defaultValue);
        }

        public virtual double GetDouble(string key)
        {
            return this.Get<double>(key);
        }

        public virtual double GetDouble(string key, double defaultValue)
        {
            return this.Get(key, defaultValue);
        }

        public virtual float GetFloat(string key)
        {
            return this.Get<float>(key);
        }

        public virtual float GetFloat(string key, float defaultValue)
        {
            return this.Get(key, defaultValue);
        }

        public virtual Color GetColor(string key)
        {
            return this.Get<Color>(key);
        }

        public virtual Color GetColor(string key, Color defaultVlaue)
        {
            return this.Get(key, defaultVlaue);
        }

        public virtual Vector3 GetVector3(string key)
        {
            return this.Get<Vector3>(key);
        }

        public virtual Vector3 GetVector3(string key, Vector3 defaultValue)
        {
            return this.Get(key, defaultValue);
        }

        public virtual DateTime GetDateTime(string key)
        {
            return this.Get(key, new DateTime(0));
        }

        public virtual DateTime GetDateTime(string key, DateTime defaultValue)
        {
            return this.Get(key, defaultValue);
        }

        public virtual T Get<T>(string key)
        {
            return this.Get(key, default(T));
        }

        public virtual T Get<T>(string key, T defaultValue)
        {
            if (typeof(IObservableProperty).IsAssignableFrom(typeof(T)))
                return (T)GetValue(key);

            IObservableProperty value;
            if (data.TryGetValue(key, out value))
            {
                var p = value as IObservableProperty<T>;
                if (p != null)
                    return p.Value;

                if (value.Value is T)
                    return (T)value.Value;

                return (T)Convert.ChangeType(value.Value, typeof(T));
            }
            return defaultValue;
        }

        public virtual IObservableProperty GetValue(string key)
        {
            return GetValue(key, true);
        }

        public virtual IObservableProperty GetValue(string key, bool isAutoCreated)
        {
            IObservableProperty value;
            if (data.TryGetValue(key, out value))
                return value;

            if (!isAutoCreated)
                return null;

            lock (_lock)
            {
                if (data.TryGetValue(key, out value))
                    return value;

                value = new ObservableProperty();
                data[key] = value;
                return value;
            }
        }

        protected class ProviderEntry
        {
            public ProviderEntry(IDataProvider provider)
            {

            }

            public IDataProvider Provider { get; private set; }

            public List<string> Keys { get; private set; }
        }
    }
}

