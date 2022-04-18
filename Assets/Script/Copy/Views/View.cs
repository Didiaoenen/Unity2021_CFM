using System;
using UnityEngine;

namespace CFM.Framework.Views
{
    public class View : MonoBehaviour, IView
    {
        [NonSerialized]
        private IAttributes attributes = new Attributes();

        public virtual string Name
        {
            get { return gameObject != null ? gameObject.name : null; }
            set
            {
                if (gameObject == null)
                    return;

                gameObject.name = value;
            }
        }

        public virtual Transform Parent
        {
            get { return transform != null ? transform.parent : null; }
        }

        public virtual GameObject Owner
        {
            get { return gameObject; }
        }

        public virtual Transform Transform
        {
            get { return transform; }
        }

        public virtual bool Visibility
        {
            get { return gameObject != null ? gameObject.activeSelf : false; }
            set
            {
                if (gameObject == null)
                    return;

                if (gameObject.activeSelf == value)
                    return;

                gameObject.SetActive(value);
            }
        }

        protected virtual void OnEnable()
        {
            OnVisibilityChanged();
        }

        protected virtual void OnDisable()
        {
            OnVisibilityChanged();
        }

        public virtual IAttributes ExtraAttributes { get { return attributes; } }

        protected virtual void OnVisibilityChanged()
        {
            
        }
    }
}

