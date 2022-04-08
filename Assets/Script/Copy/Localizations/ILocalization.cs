using System;
using UnityEngine;

using CFM.Framework.Observables;

namespace CFM.Framework.Localizations
{
    public interface ILocalization
    {
        ILocalization Subset(string prefix);

        bool ContainsKey(string key);

        string GetText(string key);

        string GetText(string key, string defaultValue);

        string GetFormattedText(string key, params object[] args);

        bool GetBoolean(string key);

        bool GetBoolean(string key, bool defaultValue);

        int GetInt(string key);

        int GetInt(string key, int defaultValue);

        long GetLong(string key);

        long GetLong(string key, long defaultValue);
    
        double GetDouble(string key);

        double GetDouble(string key, double defaultValue);

        float GetFloat(string key);

        float GetFloat(string key, float defaultValue);

        Color GetColor(string key);

        Color GetColor(string key, Color defaultValue);

        Vector3 GetVector3(string key);

        Vector3 GetVector3(string key, Vector3 defaultValue);

        DateTime GetDateTime(string key);

        DateTime GetDateTime(string key, DateTime defaultValue);

        T Get<T>(string key);

        T Get<T>(string key, T defaultValue);

        IObservableProperty GetValue(string key);
    }
}

