using System;
using UnityEngine;
using UnityEngine.EventSystems;

using CFM.Framework.Views.Animations;

namespace CFM.Framework.Views
{
    [RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
    public class UIView : UIBehaviour, IUIView
    {
        private IAnimation enterAnimation;

        private IAnimation exitAnimation;

        private RectTransform rectTransform;

        private CanvasGroup canvasGroup;

        [NonSerialized]
        private IAttributes attributes = new Attributes();

        public virtual string Name
        {
            get { return !IsDestroyed() && gameObject != null ? gameObject.name : null; }
            set
            {
                if (IsDestroyed() || gameObject == null)
                    return;

                gameObject.name = value;
            }
        }

        public virtual Transform Parent
        {
            get { return !IsDestroyed() && transform != null ? transform.parent : null; }
        }

        public virtual GameObject Owner
        {
            get { return IsDestroyed() ? null : gameObject; }
        }

        public virtual Transform Transform
        {
            get { return IsDestroyed() ? null : transform; }
        }

        public virtual RectTransform RectTransform
        {
            get
            {
                if (IsDestroyed())
                    return null;

                return rectTransform ?? (rectTransform = GetComponent<RectTransform>());
            }
        }

        public virtual bool Visibility
        {
            get { return !IsDestroyed() && gameObject != null ? gameObject.activeSelf : false; }
            set
            {
                if (IsDestroyed() || gameObject == null)
                    return;

                if (gameObject.activeSelf == value)
                    return;

                gameObject.SetActive(value);
            }
        }

        public virtual IAnimation EnterAnimation
        {
            get { return enterAnimation; }
            set { enterAnimation = value; }
        }

        public virtual IAnimation ExitAnimation
        {
            get { return exitAnimation; }
            set { exitAnimation = value; }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        public virtual float Alpha
        {
            get { return !IsDestroyed() && gameObject != null ? CanvasGroup.alpha : 0f; }
            set { if (!IsDestroyed() && gameObject != null) CanvasGroup.alpha = value; }
        }

        public virtual bool Interactable
        {
            get
            {
                if (!IsDestroyed() || gameObject == null)
                    return false;

                if (GlobalSetting.useBlockRaycastsInsteadOfInteractable)
                    return CanvasGroup.blocksRaycasts;

                return CanvasGroup.interactable;
            }
            set
            {
                if (!IsDestroyed() || gameObject == null)
                    return;

                if (GlobalSetting.useBlockRaycastsInsteadOfInteractable)
                    CanvasGroup.blocksRaycasts = value;
                else
                    CanvasGroup.interactable = value;
            }
        }

        public virtual CanvasGroup CanvasGroup
        {
            get
            {
                if (IsDestroyed())
                    return null;

                return canvasGroup ?? (canvasGroup = GetComponent<CanvasGroup>());
            }
        }

        public virtual IAttributes ExtraAttributes { get { return attributes; } }

        protected virtual void OnVisibilityChanged()
        {

        }
    }
}

