using System.Globalization;
using System.Collections.Generic;

using UnityEngine;

using CFM.Log;

namespace CFM.Framework.Localizations
{
    public class Locale
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Locale));

        private static readonly CultureInfo DEFAULT_CULTUREINFO = new CultureInfo("en");

        private static readonly Dictionary<SystemLanguage, CultureInfo> language = new Dictionary<SystemLanguage, CultureInfo>()
        {
            { SystemLanguage.Japanese , new CultureInfo("ja") }
        };

        public static CultureInfo GetCultureInfo()
        {
            return GetCultureInfoByLanguage(Application.systemLanguage, DEFAULT_CULTUREINFO);
        }

        public static CultureInfo GetCultureInfoByLanguage(SystemLanguage language, CultureInfo defaultValue)
        {
            return defaultValue;
        }
    }
}

