using System;

using UnityEngine;

using CFM.Framework.Observables;

namespace CFM.Framework.Localizations
{
    public class SubsetLocalization: ILocalization
    {
        private readonly string prefix;

        private readonly Localization parent;

        public SubsetLocalization(Localization parent, string prefix): base()
        {
            this.parent = parent;
            this.prefix = prefix;
        }

        protected string GetParentKey(string key)
        {
            if ("".Equals(key) || key == null)
                throw new ArgumentNullException(key);

            return string.Format("{0}.{1}", prefix, key);
        }

        public ILocalization Subset(string prefix)
        {
            return parent.Subset(GetParentKey(prefix));
        }

        public bool ContainsKey(string key)
        {
            return parent.ContainsKey(GetParentKey(key));
        }

        public string GetText(string key)
        {
            return GetText(key, key);
        }

        public string GetText(string key, string defaultValue)
        {
            return Get(key, defaultValue);
        }

        public string GetFormattedText(string key, params object[] args)
        {
            return GetFormattedText(key, key, args);
        }

        public bool GetBoolean(string key)
        {
            return Get<bool>(key);
        }

        public bool GetBoolean(string key, bool defaultValue)
        {
            return Get(key, defaultValue);
        }

        public int GetInt(string key)
        {
            return Get<int>(key);
        }

        public int GetInt(string key, int defaultValue)
        {
            return Get(key, defaultValue);
        }

        public long GetLong(string key)
        {
            return Get<long>(key);
        }

        public long GetLong(string key, long defaultValue)
        {
            return Get(key, defaultValue);
        }

        public double GetDouble(string key)
        {
            return Get<double>(key);
        }

        public double GetDouble(string key, double defaultValue)
        {
            return Get(key, defaultValue);
        }

        public float GetFloat(string key)
        {
            return Get<float>(key);
        }

        public float GetFloat(string key, float defaultValue)
        {
            return Get(key, defaultValue);
        }

        public Color GetColor(string key)
        {
            return Get<Color>(key);
        }

        public Color GetColor(string key, Color defaultValue)
        {
            return Get(key, defaultValue);
        }

        public Vector3 GetVector3(string key)
        {
            return Get<Vector3>(key);
        }

        public Vector3 GetVector3(string key, Vector3 defaultValue)
        {
            return Get(key, defaultValue);
        }

        public DateTime GetDateTime(string key)
        {
            return Get(key, new DateTime(0));
        }

        public DateTime GetDateTime(string key, DateTime defaultValue)
        {
            return Get(key, defaultValue);
        }

        public T Get<T>(string key)
        {
            return this.Get(key, default(T));
        }

        public T Get<T>(string key, T defaultValue)
        {
            return parent.Get(GetParentKey(key), defaultValue);
        }

        public IObservableProperty GetValue(string key)
        {
            return parent.GetValue(key);
        }
    }
}

