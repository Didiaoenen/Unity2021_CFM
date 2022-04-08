using System;
using System.Text;
using System.Collections.Generic;

using UnityEngine;

namespace CFM.Framework.Prefs
{
    public class PlayerPrefsPreferencesFactory: AbstractFactory
    {
        public PlayerPrefsPreferencesFactory(): this(null, null)
        {

        }

        public PlayerPrefsPreferencesFactory(ISerializer serializer): this(serializer, null)
        {

        }

        public PlayerPrefsPreferencesFactory(ISerializer serializer, IEncryptor encryptor): base(serializer, encryptor)
        {

        }

        public override Preferences Create(string name)
        {
            return new PlayerPrefsPreferences(name, this.Serializer, this.Encryptor);
        }
    }

    public class PlayerPrefsPreferences: Preferences
    {
        protected static readonly string KEYS = "_KETS_";

        protected readonly ISerializer serializer;

        protected readonly IEncryptor encryptor;

        protected readonly List<string> keys = new List<string>();

        public PlayerPrefsPreferences(string name, ISerializer serializer, IEncryptor encryptor): base(name)
        {
            this.serializer = serializer;
            this.encryptor = encryptor;
            this.Load();
        }

        protected override void Load()
        {
            LoadKeys();
        }

        protected string Key(string key)
        {
            StringBuilder buf = new StringBuilder(this.Name);
            buf.Append(".").Append(key);
            return buf.ToString();
        }

        protected virtual void LoadKeys()
        {
            if (!PlayerPrefs.HasKey(Key(KEYS)))
                return;

            string value = PlayerPrefs.GetString(Key(KEYS));

            string[] keyValues = value.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries);
            foreach (string key in keyValues)
            {
                if (string.IsNullOrEmpty(key))
                    continue;

                this.keys.Add(key);
            }
        }

        protected virtual void SaveKeys()
        {
            if (this.keys == null || this.keys.Count <= 0)
            {
                PlayerPrefs.DeleteKey(Key(KEYS));
                return;
            }

            string[] values = keys.ToArray();

            StringBuilder buf = new StringBuilder();
            for (int i = 0; i < values.Length; i++)
            {
                if (string.IsNullOrEmpty(values[i]))
                    continue;

                buf.Append(values[i]);
                if (i < values.Length - 1)
                    buf.Append(",");
            }

            PlayerPrefs.SetString(Key(KEYS), buf.ToString());
        }

        public override object GetObject(string key, Type type, object defaultValue)
        {
            return null;
        }

        public override void SetObject(string key, object value)
        {

        }

        public override T GetObject<T>(string key, T defaultValue)
        {
            throw new NotImplementedException();
        }

        public override void SetObject<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public override object[] GetArray(string key, Type type, object[] defaultValue)
        {
            throw new NotImplementedException();
        }

        public override void SetArray(string key, object[] values)
        {
            throw new NotImplementedException();
        }

        public override T[] GetArray<T>(string key, T[] defaultValue)
        {
            throw new NotImplementedException();
        }

        public override void SetArray<T>(string key, T[] values)
        {
            throw new NotImplementedException();
        }

        public override bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        public override void Remove(string key)
        {
            throw new NotImplementedException();
        }

        public override void RemoveAll()
        {
            throw new NotImplementedException();
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override void Delete()
        {
            throw new NotImplementedException();
        }
    }
}

