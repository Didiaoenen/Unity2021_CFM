using System;
using System.IO;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;

using CFM.Log;
using CFM.Framework.Localizations;

namespace CFM.Framework.Example
{
    public class ResourcesDataProvider: IDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ResourcesDataProvider));

        private string root;

        private IDocumentParser parser;

        public ResourcesDataProvider(string root, IDocumentParser parser)
        {
            if (string.IsNullOrEmpty(root))
                throw new ArgumentNullException("root");

            if (parser == null)
                throw new ArgumentNullException("parser");

            this.root = root;
            this.parser = parser;
        }

        protected string GetDefaultPath()
        {
            return GetPath("default");
        }

        protected string GetPath(string dir)
        {
            StringBuilder buf = new StringBuilder();
            buf.Append(this.root);
            if (!this.root.EndsWith("/"))
                buf.Append("/");
            buf.Append(dir);
            return buf.ToString();
        }

        public virtual Task<Dictionary<string, object>> Load(CultureInfo cultureInfo)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                TextAsset[] defaultTexts = Resources.LoadAll<TextAsset>(GetDefaultPath());
                TextAsset[] towLetterISOTexts = Resources.LoadAll<TextAsset>(GetPath(cultureInfo.TwoLetterISOLanguageName));
                TextAsset[] texts = cultureInfo.Name.Equals(cultureInfo.TwoLetterISOLanguageName) ? null : Resources.LoadAll<TextAsset>(GetPath(cultureInfo.Name));

                FillData(dict, defaultTexts, cultureInfo);
                FillData(dict, towLetterISOTexts, cultureInfo);
                FillData(dict, texts, cultureInfo);
                return Task.FromResult(dict);
            }
            catch (Exception e)
            {
                return Task.FromException<Dictionary<string, object>>(e);
            }
        }

        private void FillData(Dictionary<string, object> dict, TextAsset[] texts, CultureInfo cultureInfo)
        {
            try
            {
                if (texts == null || texts.Length <= 0)
                    return;

                foreach (TextAsset text in texts)
                {
                    try
                    {
                        using (MemoryStream stream = new MemoryStream(text.bytes))
                        {
                            var data = parser.Parse(stream, cultureInfo);
                            foreach (KeyValuePair<string, object> kv in data)
                            {
                                dict[kv.Key] = kv.Value;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        if (log.IsWarnEnabled)
                            log.WarnFormat("{0}{1}", text.name, e);
                    }
                }
            }
            catch (Exception) { }
        }
    }
}

