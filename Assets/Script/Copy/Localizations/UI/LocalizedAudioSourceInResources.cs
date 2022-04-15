using System;
using System.Collections;

using UnityEngine;

using CFM.Log;

namespace CFM.Framework.Localizations.UI
{
    [AddComponentMenu("CFM/Framework/Localization/LocalizedAudioSourceInResources")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public class LocalizedAudioSourceInResources : AbstractLocalized<AudioSource>
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(LocalizedAudioSourceInResources));

        protected override void OnValueChanged(object sender, EventArgs e)
        {
            object v = value.Value;
            if (v is AudioClip)
            {
                target.clip = (AudioClip)v;
            }
            else if (v is string)
            {
                string path = (string)v;
                StartCoroutine(DoLoad(path));
            }
            else if (v != null)
            {
                if (log.IsErrorEnabled)
                    log.ErrorFormat("There is an invalid localization value \"{0}\" on the GameObject named \"{1}\".", v, name);
            }
        }

        protected virtual IEnumerator DoLoad(string path)
        {
            var result = Resources.LoadAsync<AudioClip>(path);
            yield return result;
            target.clip = (AudioClip)result.asset;
        }
    }
}

