using System;
using System.Collections.Generic;

namespace CFM.Framework.Configuration
{
    public interface IConfiguration
    {
        IConfiguration Subset(string prefix);

        bool IsEmpty { get; }

        bool ContainsKey(string key);

        IEnumerator<string> GetKeys(string prefix);

        IEnumerator<string> GetKeys();

        bool GetBoolean(string key);

        bool GetBoolean(string key, bool defaultValue);

        float GetFloat(string key);

        float GetFloat(string key, float defaultValue);

        double GetDouble(string key);

        double GetDouble(string key, double defaultValue);

        short GetShort(string key);

        short GetShort(string key, short defaultValue);

        int GetInt(string key);

        int GetInt(string key, int defaultValue);

        long GetLong(string key);

        long GetLong(string key, long defaultValue);

        string GetString(string key);

        string GetString(string key, string defaultValue);

        string GetFormattedString(string key, params object[] args);

        DateTime GetDateTime(string key);

        DateTime GetDateTime(string key, DateTime defaultValue);

        Version GetVersion(string key);

        Version GetVersion(string key, Version defaultValue);

        T GetObject<T>(string key);

        T GetObject<T>(string key, T defaultValue);

        object[] GetArray(string key, Type type);

        object[] GetArray(string key, Type type, object[] defaultValue);

        T[] GetArray<T>(string key);

        T[] GetArray<T>(string key, T[] defaultValue);

        object GetProperty(string key);

        void AddProperty(string key, object value);

        void SetProperty(string key, object value);

        void RemoveProperty(string key);

        void Clear();
    }
}
