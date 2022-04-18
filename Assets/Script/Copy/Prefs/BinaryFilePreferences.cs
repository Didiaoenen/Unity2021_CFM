using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

using UnityEngine;

using CFM.Log;

namespace CFM.Framework.Prefs
{
    public class BinaryFilePreferencesFactory : AbstractFactory
    {
        public BinaryFilePreferencesFactory() : this(null, null)
        {

        }

        public BinaryFilePreferencesFactory(ISerializer serializer) : this(serializer, null)
        {

        }

        public BinaryFilePreferencesFactory(ISerializer serializer, IEncryptor encryptor) : base(serializer, encryptor)
        {

        }

        public override Preferences Create(string name)
        {
            return new BinaryFilePreferences(name, Serializer, Encryptor);
        }
    }

    public class BinaryFilePreferences : Preferences
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(BinaryFilePreferences));

        private string root;

        protected readonly Dictionary<string, string> dict = new Dictionary<string, string>();

        protected readonly ISerializer serializer;

        protected readonly IEncryptor encryptor;

        public BinaryFilePreferences(string name, ISerializer serializer, IEncryptor encryptor) : base(name)
        {
            root = Application.persistentDataPath;
            this.serializer = serializer;
            this.encryptor = encryptor;
            Load();
        }

        public virtual StringBuilder GetDirectory()
        {
            StringBuilder buf = new StringBuilder(root);
            buf.Append("/").Append(Name).Append("/");
            return buf;
        }

        public virtual StringBuilder GetFullFileName()
        {
            return GetDirectory().Append("prefs.dat");
        }

        protected override void Load()
        {
            try
            {
                string filename = GetFullFileName().ToString();
                if (!File.Exists(filename))
                    return;

                byte[] data = File.ReadAllBytes(filename);
                if (data == null || data.Length <= 0)
                    return;

                if (encryptor != null)
                    data = encryptor.Decode(data);

                dict.Clear();
                using (MemoryStream stream = new MemoryStream(data))
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        int count = reader.ReadInt32();
                        for (int i = 0; i < count; i++)
                        {
                            string key = reader.ReadString();
                            string value = reader.ReadString();
                            dict.Add(key, value);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("Load failed,{0}", e);
            }
        }

        public override object GetObject(string key, Type type, object defaultValue)
        {
            if (!dict.ContainsKey(key))
                return defaultValue;

            string str = dict[key];
            if (string.IsNullOrEmpty(str))
                return defaultValue;

            return serializer.Deserialize(str, type);
        }

        public override void SetObject(string key, object value)
        {
            if (value == null)
            {
                dict.Remove(key);
                return;
            }

            dict[key] = serializer.Serialize(value);
        }

        public override T GetObject<T>(string key, T defaultValue)
        {
            if (!dict.ContainsKey(key))
                return defaultValue;

            string str = dict[key];
            if (string.IsNullOrEmpty(str))
                return defaultValue;

            return (T)serializer.Deserialize(str, typeof(T));
        }

        public override void SetObject<T>(string key, T value)
        {
            if (value == null)
            {
                dict.Remove(key);
                return;
            }

            dict[key] = serializer.Serialize(value);
        }

        public override object[] GetArray(string key, Type type, object[] defaultValue)
        {
            if (!dict.ContainsKey(key))
                return defaultValue;

            string str = dict[key];
            if (string.IsNullOrEmpty(str))
                return defaultValue;

            string[] items = str.Split(ARRAY_SEPARATOR);
            List<object> list = new List<object>();
            for (int i = 0; i < items.Length; i++)
            {
                string item = items[i];
                if (string.IsNullOrEmpty(item))
                    list.Add(null);
                else
                {
                    list.Add(serializer.Deserialize(items[i], type));
                }
            }
            return list.ToArray();
        }

        public override void SetArray(string key, object[] values)
        {
            if (values == null || values.Length == 0)
            {
                dict.Remove(key);
                return;
            }

            StringBuilder buf = new StringBuilder();
            for (int i = 0; i < values.Length; i++)
            {
                var value = values[i];
                buf.Append(serializer.Serialize(value));
                if (i < values.Length - 1)
                    buf.Append(ARRAY_SEPARATOR);
            }

            dict[key] = buf.ToString();
        }

        public override T[] GetArray<T>(string key, T[] defaultValue)
        {
            if (!dict.ContainsKey(key))
                return defaultValue;

            string str = dict[key];
            if (string.IsNullOrEmpty(str))
                return defaultValue;

            string[] items = str.Split(ARRAY_SEPARATOR);
            List<T> list = new List<T>();
            for (int i = 0; i < items.Length; i++)
            {
                string item = items[i];
                if (string.IsNullOrEmpty(item))
                    list.Add(default(T));
                else
                {
                    list.Add((T)serializer.Deserialize(items[i], typeof(T)));
                }
            }
            return list.ToArray();
        }

        public override void SetArray<T>(string key, T[] values)
        {
            if (values == null || values.Length == 0)
            {
                dict.Remove(key);
                return;
            }

            StringBuilder buf = new StringBuilder();
            for (int i = 0; i < values.Length; i++)
            {
                var value = values[i];
                buf.Append(serializer.Serialize(value));
                if (i < values.Length - 1)
                    buf.Append(ARRAY_SEPARATOR);
            }

            dict[key] = buf.ToString();
        }

        public override bool ContainsKey(string key)
        {
            return dict.ContainsKey(key);
        }

        public override void Remove(string key)
        {
            if (dict.ContainsKey(key))
                dict.Remove(key);
        }

        public override void RemoveAll()
        {
            dict.Clear();
        }

        public override void Save()
        {
            if (dict.Count <= 0)
            {
                Delete();
                return;
            }

            Directory.CreateDirectory(GetDirectory().ToString());
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(dict.Count);
                    foreach (KeyValuePair<string, string> kv in dict)
                    {
                        writer.Write(kv.Key);
                        writer.Write(kv.Value);
                    }
                    writer.Flush();
                }
                byte[] data = stream.ToArray();
                if (encryptor != null)
                    data = encryptor.Encode(data);

                var filename = GetFullFileName().ToString();
                File.WriteAllBytes(filename, data);
            }
        }

        public override void Delete()
        {
            dict.Clear();
            string filename = GetFullFileName().ToString();
            if (File.Exists(filename))
                File.Delete(filename);
        }
    }
}

