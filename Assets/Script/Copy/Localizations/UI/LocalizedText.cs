using System;

using UnityEngine;
using UnityEngine.UI;

namespace CFM.Framework.Localizations.UI
{
    [AddComponentMenu("CFM/Framework/Localization/LocalizedText")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Text))]
    public class LocalizedText : AbstractLocalized<Text>
    {
        protected override void OnValueChanged(object sender, EventArgs e)
        {
            target.text = (string)Convert.ChangeType(value.Value, typeof(string));
        }
    }
}

