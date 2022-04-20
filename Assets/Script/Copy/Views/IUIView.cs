using UnityEngine;
using CFM.Framework.Views.Animations;

namespace CFM.Framework.Views
{
    public interface IUIView : IView
    {
        RectTransform RectTransform { get; }

        float Alpha { get; set; }

        bool Interactable { get; }

        CanvasGroup CanvasGroup { get; }

        IAnimation EnterAnimation { get; set; }

        IAnimation ExitAnimation { get; set; }
    }
}

