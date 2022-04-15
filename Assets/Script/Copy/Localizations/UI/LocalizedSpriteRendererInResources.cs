using System;
using System.Collections;

using UnityEngine;

using CFM.Log;

namespace CFM.Framework.Localizations.UI
{
    [AddComponentMenu("Loxodon/Localization/LocalizedSpriteRendererInResources")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpriteRenderer))]
    public class LocalizedSpriteRendererInResources : AbstractLocalized<SpriteRenderer>
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(LocalizedSpriteRendererInResources));

        protected override void OnValueChanged(object sender, EventArgs e)
        {
            object v = value.Value;
            if (v is Sprite)
            {
                target.sprite = (Sprite)v;
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
            var result = Resources.LoadAsync<Sprite>(path);
            yield return result;
            target.sprite = (Sprite)result.asset;
        }
    }
}

