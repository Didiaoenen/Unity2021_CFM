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
            this.cultureInfo = cultureInfo;
            if (this.cultureInfo == null)
                this.cultureInfo = Locale.GetCultureInfo();
        }

        public event EventHandler CultureInfoChanged
        {
            add { lock (_lock) { cultureInfoChanged += value; } }
            remove { lock (_lock) { cultureInfoChanged -= value; } }
        }

        public virtual CultureInfo CultureInfo
        {
            get { return cultureInfo; }
            set
            {
                if (value == null || (cultureInfo != null && cultureInfo.Equals(value)))
                    return;

                cultureInfo = value;
                OnCultureInfoChanged();
            }
        }

        protected void RaiseCultureInfoChanged()
        {
            try
            {
                cultureInfoChanged?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception) { }
        }

        protected virtual void OnCultureInfoChanged()
        {
            RaiseCultureInfoChanged();
            Refresh();
        }

        public Task AddDataProvider(IDataProvider provider)
        {
            return DoAddDataProvider(provider);
        }


        protected virtual async Task DoAddDataProvider(IDataProvider provider)
        {
            if (providers == null)
                return;

            var entry = new ProviderEntry(provider);
            lock(_lock)
            {
                if (providers.Exists(e => e.Provider == provider))
                    return;
                providers.Add(entry);
            }

            await Load(entry);
        }

        public virtual void RemoveDataProvider(IDataProvider provider)
        {
            if (provider == null)
                return;

            lock (_lock)
            {
                for (int i = providers.Count - 1; i >= 0; i--)
                {
                    var entry = providers[i];
                    if (entry.Provider == provider)
                    {
                        providers.RemoveAt(i);
                        OnUnloadCompleted(entry.Keys);
                        (provider as IDisposable)?.Dispose();
                        return;
                    }
                }
            }
        }

        public Task Refresh()
        {
            return Load(this.providers.ToArray());
        }

        protected virtual async Task Load(params ProviderEntry[] providers)
        {
            if (providers == null || providers.Length <= 0)
                return;

            int count = providers.Length;
            var cultureInfo = CultureInfo;
            for (int i = 0; i < count; i++)
            {
                try
                {
                    var entry = providers[i];
                    var provider = entry.Provider;
                    var dict = await provider.Load(cultureInfo);
                    OnLoadCompleted(entry, dict);
                }
                catch (Exception) { }
            }
        }

        protected virtual void OnLoadCompleted(ProviderEntry entry, Dictionary<string, object> dict)
        {
            if (dict == null || dict.Count <= 0)
                return;

            lock (_lock)
            {
                var keys = entry.Keys;
                keys.Clear();

                foreach (KeyValuePair<string, object> kv in dict)
                {
                    var key = kv.Key;
                    var value = kv.Value;
                    keys.Add(key);
                    AddValue(key, value);
                }
            }
        }

        protected virtual void AddValue(string key, object value)
        {
            IObservableProperty property;
            if (!data.TryGetValue(key, out property))
            {
                Type valueType = value != null ? value.GetType() : typeof(object);
                TypeCode typeCode = Type.GetTypeCode(valueType);
                switch (typeCode)
                {
                    case TypeCode.Boolean:
                        {
                            property = new ObservableProperty<bool>();
                            break;
                        }
                    case TypeCode.Byte:
                        {
                            property = new ObservableProperty<byte>();
                            break;
                        }
                    case TypeCode.Char:
                        {
                            property = new ObservableProperty<char>();
                            break;
                        }
                    case TypeCode.DateTime:
                        {
                            property = new ObservableProperty<DateTime>();
                            break;
                        }
                    case TypeCode.Decimal:
                        {
                            property = new ObservableProperty<Decimal>();
                            break;
                        }
                    case TypeCode.Double:
                        {
                            property = new ObservableProperty<Double>();
                            break;
                        }
                    case TypeCode.Int16:
                        {
                            property = new ObservableProperty<short>();
                            break;
                        }
                    case TypeCode.Int32:
                        {
                            property = new ObservableProperty<int>();
                            break;
                        }
                    case TypeCode.Int64:
                        {
                            property = new ObservableProperty<long>();
                            break;
                        }
                    case TypeCode.SByte:
                        {
                            property = new ObservableProperty<sbyte>();
                            break;
                        }
                    case TypeCode.Single:
                        {
                            property = new ObservableProperty<float>();
                            break;
                        }
                    case TypeCode.String:
                        {
                            property = new ObservableProperty<string>();
                            break;
                        }
                    case TypeCode.UInt16:
                        {
                            property = new ObservableProperty<UInt16>();
                            break;
                        }
                    case TypeCode.UInt32:
                        {
                            property = new ObservableProperty<UInt32>();
                            break;
                        }
                    case TypeCode.UInt64:
                        {
                            property = new ObservableProperty<UInt64>();
                            break;
                        }
                    case TypeCode.Object:
                        {
                            if (valueType.Equals(typeof(Vector2)))
                            {
                                property = new ObservableProperty<Vector2>();
                            }
                            else if (valueType.Equals(typeof(Vector3)))
                            {
                                property = new ObservableProperty<Vector3>();
                            }
                            else if (valueType.Equals(typeof(Vector4)))
                            {
                                property = new ObservableProperty<Vector4>();
                            }
                            else if (valueType.Equals(typeof(Color)))
                            {
                                property = new ObservableProperty<Color>();
                            }
                            else
                            {
                                property = new ObservableProperty();
                            }
                            break;
                        }
                    default:
                        {
                            property = new ObservableProperty();
                            break;
                        }
                }
                data[key] = property;
            }
            property.Value = value;
        }

        protected virtual void OnUnloadCompleted(List<string> keys)
        {
            foreach (string key in keys)
            {
                IObservableProperty value;
                if (data.TryRemove(key, out value) && value != null)
                    value.Value = null;
            }
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
            return GetText(key, key);
        }

        public virtual string GetText(string key, string defaultValue)
        {
            return Get(key, defaultValue);
        }

        public virtual string GetFormattedText(string key, params object[] args)
        {
            string format = Get<string>(key, null);
            if (format == null)
                return key;

            return string.Format(format, args);
        }

        public virtual bool GetBoolean(string key)
        {
            return Get(key, false);
        }

        public virtual bool GetBoolean(string key, bool defaultValue)
        {
            return Get(key, defaultValue);
        }

        public virtual int GetInt(string key)
        {
            return Get<int>(key);
        }

        public virtual int GetInt(string key, int defaultValue)
        {
            return Get(key, defaultValue);
        }

        public virtual long GetLong(string key)
        {
            return Get<long>(key);
        }

        public virtual long GetLong(string key, long defaultValue)
        {
            return Get(key, defaultValue);
        }

        public virtual double GetDouble(string key)
        {
            return Get<double>(key);
        }

        public virtual double GetDouble(string key, double defaultValue)
        {
            return Get(key, defaultValue);
        }

        public virtual float GetFloat(string key)
        {
            return Get<float>(key);
        }

        public virtual float GetFloat(string key, float defaultValue)
        {
            return Get(key, defaultValue);
        }

        public virtual Color GetColor(string key)
        {
            return Get<Color>(key);
        }

        public virtual Color GetColor(string key, Color defaultVlaue)
        {
            return Get(key, defaultVlaue);
        }

        public virtual Vector3 GetVector3(string key)
        {
            return Get<Vector3>(key);
        }

        public virtual Vector3 GetVector3(string key, Vector3 defaultValue)
        {
            return Get(key, defaultValue);
        }

        public virtual DateTime GetDateTime(string key)
        {
            return Get(key, new DateTime(0));
        }

        public virtual DateTime GetDateTime(string key, DateTime defaultValue)
        {
            return Get(key, defaultValue);
        }

        public virtual T Get<T>(string key)
        {
            return Get(key, default(T));
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
                Provider = provider;
                Keys = new List<string>();
            }

            public IDataProvider Provider { get; private set; }

            public List<string> Keys { get; private set; }
        }
    }
}

