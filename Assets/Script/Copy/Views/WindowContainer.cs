using System;
using System.Collections.Generic;

using UnityEngine;

using CFM.Framework.Asynchronous;
using IAsyncResult = CFM.Framework.Asynchronous.IAsyncResult;

namespace CFM.Framework.Views
{
    [DisallowMultipleComponent]
    public class WindowContainer : Window, IWindowManager
    {
        public static WindowContainer Create(string name)
        {
            return Create(null, name);
        }

        public static WindowContainer Create(IWindowManager windowManager, string name)
        {
            GameObject root = new GameObject(name, typeof(CanvasGroup));
            RectTransform rectTransform = root.AddComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.zero;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.localPosition = Vector2.zero;

            WindowContainer container = root.AddComponent<WindowContainer>();
            container.WindowManager = windowManager;
            container.Create();
            container.Show(true);
            return container;
        }

        private IWindowManager localWindowManager;

        protected override void OnCreate(IBundle bundle)
        {
            WindowType = WindowType.FULL;
            localWindowManager = CreateWindowManager();
        }

        protected virtual IWindowManager CreateWindowManager()
        {
            return gameObject.AddComponent<WindowManager>();
        }

        protected override void OnActivatedChanged()
        {
            if (localWindowManager != null)
                localWindowManager.Activated = Activated;
            base.OnActivatedChanged();
        }

        bool IWindowManager.Activated
        {
            get { return localWindowManager.Activated; }
            set { localWindowManager.Activated = value; }
        }

        public IWindow Current { get { return localWindowManager.Current; } }

        public int Count { get { return localWindowManager.Count; } }

        public override IAsyncResult Activate(bool ignoreAnimation)
        {
            if (!Visibility)
                throw new InvalidOperationException("The window is not visible.");

            if (localWindowManager.Current != null)
            {
                Activated = true;
                return (localWindowManager.Current as IManageable).Activate(ignoreAnimation);
            }

            AsyncResult result = new AsyncResult();
            try
            {
                if (Activated)
                {
                    result.SetResult();
                    return result;
                }

                if (!ignoreAnimation && ActivationAnimation != null)
                {
                    ActivationAnimation.OnStart(() =>
                    {
                        State = WindowState.ACTIVATION_ANIMATION_BEGIN;
                    }).OnEnd(() =>
                    {
                        State = WindowState.ACTIVATION_ANIMATION_END;
                        Activated = true;
                        State = WindowState.ACTIVATED;
                        result.SetResult();
                    }).Play();
                }
                else
                {
                    Activated = transform;
                    State = WindowState.ACTIVATED;
                    result.SetResult();
                }
            }
            catch (Exception e)
            {
                result.SetException(e);
            }

            return result;
        }

        public override IAsyncResult Passivate(bool ignoreAnimation)
        {
            if (!Visibility)
                throw new InvalidOperationException("The window is not visible.");

            if (localWindowManager.Current != null)
            {
                IAsyncResult currResult = (localWindowManager.Current as IManageable).Passivate(ignoreAnimation);
                currResult.Callbackable().OnCallback((r) =>
                {
                    Activated = false;
                });
                return currResult;
            }

            AsyncResult result = new AsyncResult();
            try
            {
                if (!Activated)
                {
                    result.SetResult();
                    return result;
                }

                Activated = false;
                State = WindowState.PASSIVATED;

                if (!ignoreAnimation && PassivationAnimation != null)
                {
                    PassivationAnimation.OnStart(() =>
                    {
                        State = WindowState.PASSIVATION_ANIMATION_BEGIN;
                    }).OnEnd(() =>
                    {
                        State = WindowState.PASSIVATION_ANIMATION_END;
                        result.SetResult();
                    }).Play();
                }
                else
                {
                    result.SetResult();
                }
            }
            catch (Exception e)
            {
                result.SetException(e);
            }
            return result;
        }

        public IEnumerator<IWindow> Visibles()
        {
            return localWindowManager.Visibles();
        }

        public IWindow Get(int index)
        {
            return localWindowManager.Get(index);
        }

        public void Add(IWindow window)
        {
            localWindowManager.Add(window);
        }

        public bool Remove(IWindow window)
        {
            return localWindowManager.Remove(window);
        }

        public IWindow RemoveAt(int index)
        {
            return localWindowManager.RemoveAt(index);
        }

        public bool Contains(IWindow window)
        {
            return localWindowManager.Contains(window);
        }

        public int IndexOf(IWindow window)
        {
            return localWindowManager.IndexOf(window);
        }

        public List<IWindow> Find(bool visible)
        {
            return localWindowManager.Find(visible);
        }

        public T Find<T>() where T : IWindow
        {
            return localWindowManager.Find<T>();
        }

        public T Find<T>(string name) where T : IWindow
        {
            return localWindowManager.Find<T>(name);
        }

        public List<T> FindAll<T>() where T : IWindow
        {
            return localWindowManager.FindAll<T>();
        }

        public void Clear()
        {
            localWindowManager.Clear();
        }

        public ITransition Show(IWindow window)
        {
            return localWindowManager.Show(window);
        }

        public ITransition Hide(IWindow window)
        {
            return localWindowManager.Hide(window);
        }

        public ITransition Dismiss(IWindow window)
        {
            return localWindowManager.Dismiss(window);
        }
    }
}

