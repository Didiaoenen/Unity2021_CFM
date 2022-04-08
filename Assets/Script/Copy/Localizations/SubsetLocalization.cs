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
            
        }

        public bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key, T defaultValue)
        {
            throw new NotImplementedException();
        }

        public bool GetBoolean(string key)
        {
            throw new NotImplementedException();
        }

        public bool GetBoolean(string key, bool defaultValue)
        {
            throw new NotImplementedException();
        }

        public Color GetColor(string key)
        {
            throw new NotImplementedException();
        }

        public Color GetColor(string key, Color defaultValue)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime(string key)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime(string key, DateTime defaultValue)
        {
            throw new NotImplementedException();
        }

        public double GetDouble(string key)
        {
            throw new NotImplementedException();
        }

        public double GetDouble(string key, double defaultValue)
        {
            throw new NotImplementedException();
        }

        public float GetFloat(string key)
        {
            throw new NotImplementedException();
        }

        public float GetFloat(string key, float defaultValue)
        {
            throw new NotImplementedException();
        }

        public string GetFormattedText(string key, params object[] args)
        {
            throw new NotImplementedException();
        }

        public int GetInt(string key)
        {
            throw new NotImplementedException();
        }

        public int GetInt(string key, int defaultValue)
        {
            throw new NotImplementedException();
        }

        public long GetLong(string key)
        {
            throw new NotImplementedException();
        }

        public long GetLong(string key, long defaultValue)
        {
            throw new NotImplementedException();
        }

        public string GetText(string key)
        {
            throw new NotImplementedException();
        }

        public string GetText(string key, string defaultValue)
        {
            throw new NotImplementedException();
        }

        public IObservableProperty GetValue(string key)
        {
            throw new NotImplementedException();
        }

        public Vector3 GetVector3(string key)
        {
            throw new NotImplementedException();
        }

        public Vector3 GetVector3(string key, Vector3 defaultValue)
        {
            throw new NotImplementedException();
        }

        public ILocalization Subset(string prefix)
        {
            throw new NotImplementedException();
        }
    }
}

