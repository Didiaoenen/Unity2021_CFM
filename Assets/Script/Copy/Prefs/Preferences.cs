using System;
using System.Collections.Generic;

namespace CFM.Framework.Prefs
{
    public abstract class Preferences
    {
        protected static readonly string GLOBAL_NAME = "_GLOBAL_";

        protected const char ARRAY_SEPARATOR = '|';

        private static Dictionary<string, Preferences> cache = new Dictionary<string, Preferences>();

        private static IFactory _defaultFactory;

        private static IFactory _factory;

        private string name;

        static Preferences()
        {
            _defaultFactory = new PlayerPrefsPreferencesFactory();
        }

        protected static IFactory GetFactory()
        {
            if (_factory != null)
                return _factory;
            return _defaultFactory;
        }

        public static Preferences GetGlobalPreferences()
        {
            return GetPreferences(GLOBAL_NAME);
        }

        public static Preferences GetPreferences(string name)
        {
            Preferences prefs;
            if (cache.TryGetValue(name, out prefs))
                return prefs;

            prefs = GetFactory().Create(name);
            cache[name] = prefs;
            return prefs;
        }

        public static void Register(IFactory factory)
        {
            _factory = factory;
        }

        public static void SaveAll()
        {
            foreach (Preferences prefs in cache.Values)
            {
                prefs.Save();
            }
        }

        public static void DeleteAll()
        {
            foreach (Preferences prefs in cache.Values)
            {
                prefs.Delete();
            }
        }

        public Preferences(string name)
        {
            this.name = name;
            if (string.IsNullOrEmpty(this.name))
                this.name = GLOBAL_NAME;
        }

        public string Name
        {
            get { return name; }
            protected set { name = value; }
        }

        protected abstract void Load();

        public string GetString(string key)
        {
            return GetObject<string>(key, null);
        }

        public string GetString(string key, string defaultValue)
        {
            return GetObject<string>(key, defaultValue);
        }

        public void SetString(string key, string value)
        {
            SetObject<string>(key, value);
        }

        public float GetFloat(string key)
        {
            return GetObject<float>(key, 0f);
        }

        public float GetFloat(string key, float defaultValue)
        {
            return GetObject<float>(key, defaultValue);
        }

        public void SetFloat(string key, float value)
        {
            SetObject<float>(key, value);
        }

        public double GetDouble(string key)
        {
            return GetObject<double>(key, 0d);
        }

        public double GetDouble(string key, double defaultValue)
        {
            return GetObject<double>(key, defaultValue);
        }

        public void SetDouble(string key, double value)
        {
            SetObject(key, value);
        }

        public bool GetBool(string key)
        {
            return GetObject<bool>(key, false);
        }

        public bool GetBool(string key, bool defaultValue)
        {
            return GetObject<bool>(key, defaultValue);
        }

        public void SetBool(string key, bool value)
        {
            SetObject<bool>(key, value);
        }

        public int GetInt(string key)
        {
            return GetObject<int>(key, 0);
        }

        public int GetInt(string key, int defaultValue)
        {
            return GetObject<int>(key, defaultValue);
        }

        public void SetInt(string key, int value)
        {
            SetObject<int>(key, value);
        }

        public long GetLong(string key)
        {
            return GetObject<long>(key, 0L);
        }

        public long GetLong(string key, long defaultValue)
        {
            return GetObject<long>(key, defaultValue);
        }

        public void SetLong(string key, long value)
        {
            SetObject<long>(key, value);
        }

        public object GetObject(string key, Type type)
        {
            return GetObject(key, type, null);
        }

        public abstract object GetObject(string key, Type type, object defaultValue);

        public abstract void SetObject(string key, object value);

        public T GetObject<T>(string key)
        {
            return GetObject<T>(key, default(T));
        }

        public abstract T GetObject<T>(string key, T defaultValue);

        public abstract void SetObject<T>(string key, T value);

        public object[] GetArray(string key, Type type)
        {
            return GetArray(key, type, null);
        }

        public abstract object[] GetArray(string key, Type type, object[] defaultValue);

        public abstract void SetArray(string key, object[] values);

        public T[] GetArray<T>(string key)
        {
            return GetArray<T>(key, null);
        }

        public abstract T[] GetArray<T>(string key, T[] defaultValue);

        public abstract void SetArray<T>(string key, T[] values);

        public abstract bool ContainsKey(string key);

        public abstract void Remove(string key);

        public abstract void RemoveAll();

        public abstract void Save();

        public abstract void Delete();
    }
}


