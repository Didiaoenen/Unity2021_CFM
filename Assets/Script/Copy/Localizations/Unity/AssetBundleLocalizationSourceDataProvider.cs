using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Collections.Generic;

using UnityEngine;

using CFM.Log;

namespace CFM.Framework.Localizations.Unity
{
    public class AssetBundleLocalizationSourceDataProvider : IDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AssetBundleLocalizationSourceDataProvider));

        protected string[] filenames;

        protected string assetBundleUrl;

        public AssetBundleLocalizationSourceDataProvider(string assetBundleUrl, params string[] filenames)
        {
            if (string.IsNullOrEmpty(assetBundleUrl))
                throw new ArgumentNullException("assetBundleUrl");

            this.assetBundleUrl = assetBundleUrl;
            this.filenames = filenames;
        }

        public Task<Dictionary<string, object>> Load(CultureInfo cultureInfo)
        {
            throw new System.NotImplementedException();
        }

        private void FillData(Dictionary<string, object> dict, AssetBundle bundle, string path)
        {
            if (string.IsNullOrEmpty(path))
                return;

            LocalizationSourceAsset sourceAsset = bundle.LoadAsset<LocalizationSourceAsset>(path);
            if (sourceAsset == null)
                return;

            MonolingualSource source = sourceAsset.Source;
            if (source == null)
                return;

            foreach (KeyValuePair<string, object> kv in source.GetData())
            {
                dict[kv.Key] = kv.Value;
            }
        }
    }
}

