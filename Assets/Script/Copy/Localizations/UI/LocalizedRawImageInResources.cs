using System;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;

using CFM.Log;

namespace CFM.Framework.Localizations.UI
{
    [AddComponentMenu("CFM/Framework/Localization/LocalizedRawImageInResources")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RawImage))]
    public class LocalizedRawImageInResources : AbstractLocalized<RawImage>
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(LocalizedRawImageInResources));

        protected override void OnValueChanged(object sender, EventArgs e)
        {
            object v = value.Value;
            if (v is Texture2D)
            {
                target.texture = (Texture2D)v;
            }
            else if (v is string)
            {
                string path = (string)v;
                StartCoroutine(DoLoad(path));
            }
            else if (v != null)
            {
                if (log.IsErrorEnabled)
                    log.ErrorFormat("There is an invalid localization value \"{0}\" on the GameObject named \"{1}\".", v, this.name);
            }

        }

        protected virtual IEnumerator DoLoad(string path)
        {
            var result = Resources.LoadAsync<Texture2D>(path);
            yield return result;
            target.texture = (Texture2D)result.asset;
        }
    }
}

