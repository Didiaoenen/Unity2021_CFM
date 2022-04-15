using System;
using System.Collections;

using UnityEngine;

using CFM.Log;
using CFM.Framework.Views;
using CFM.Framework.Contexts;
using CFM.Framework.Views.Locators;

namespace CFM.Framework.Views.UI
{
    public class Toast
    {
        private static readonly ILog log = LogManager.GetLogger(type: typeof(Toast));

        private const string DEFAULT_VIEW_LOCATOR_KEY = "_DEFAULT_VIEW_LOCATOR";

        private const string DEFAULT_VIEW_NAME = "";

        private static string viewName;

        public static string ViewName
        {
            get {  return string.IsNullOrEmpty(viewName) ? DEFAULT_VIEW_NAME : viewName; }
            set { viewName = value; }
        }

        private static IUIViewLocator GetUIViewLocator()
        {
            ApplicationContext context = ContextManager.GetApplicationContext();
            IUIViewLocator locator = context.GetService<IUIViewLocator>();
            if (locator == null)
            {
                if (log.IsWarnEnabled)
                    log.Warn("");

                locator = context.GetService<IUIViewLocator>(DEFAULT_VIEW_LOCATOR_KEY);
                if (locator == null)
                {
                    locator = new DefaultUIViewLocator();
                    context.GetContainer().Register(DEFAULT_VIEW_LOCATOR_KEY, locator);
                }
            }
            return locator;
        }

        public static IUIViewGroup GetCurrentViewGroup()
        {
            GlobalWindowManager windowManager = GlobalWindowManager.Root;
            IWindow window = windowManager.Current;
            while (window is WindowContainer windowContainer)
            {
                window = windowManager.Current;
            }
            return window as IUIViewGroup;
        }

        public static Toast Show(string text, float duration = 3f)
        {
            return Show(ViewName, null, text, duration, null, null);
        }

        public static Toast Show(string text, float duration, UILayout layout)
        {
            return Show(ViewName, null, text, duration, layout, null);
        }

        public static Toast Show(string text, float duration, UILayout layout, Action callback)
        {
            return Show(ViewName, null, text, duration, layout, callback);
        }

        public static Toast Show(IUIViewGroup viewGroup, string text, float duration = 3f)
        {
            return Show(ViewName, viewGroup, text, duration, null, null);
        }

        public static Toast Show(IUIViewGroup viewGroup, string text, float duration, UILayout layout)
        {
            return Show(ViewName, viewGroup, text, duration, layout, null);
        }

        public static Toast Show(IUIViewGroup viewGroup, string text, float duration, UILayout layout, Action callback)
        {
            return Show(ViewName, viewGroup, text, duration, layout, callback);
        }

        public static Toast Show(string viewName, IUIViewGroup viewGroup, string text, float duration, UILayout layout, Action callback)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ViewName;

            IUIViewLocator locator = GetUIViewLocator();
            ToastView view = locator.LoadView<ToastView>(viewName);
            if (view == null)
                throw new NotFoundException("");

            if (viewGroup == null)
                viewGroup = GetCurrentViewGroup();

            Toast toast = new Toast(view, viewGroup, text, duration, layout, callback);
            toast.Show();
            return toast;
        }

        private readonly IUIViewGroup viewGroup;

        private readonly float duration;

        private readonly string text;

        private readonly ToastView view;

        private readonly UILayout layout;

        private readonly Action callback;

        protected Toast(ToastView view, IUIViewGroup viewGroup, string text, float duration) : this(view, viewGroup, text, duration, null, null)
        {

        }

        protected Toast(ToastView view, IUIViewGroup viewGroup, string text, float duration, UILayout layout) : this(view, viewGroup, text, duration, layout, null)
        {

        }

        protected Toast(ToastView view, IUIViewGroup viewGroup, string text, float duration, UILayout layout, Action callback)
        {
            this.view = view;
            this.viewGroup = viewGroup;
            this.text = text;
            this.duration = duration;
            this.layout = layout;
            this.callback = callback;
        }

        public float Duration
        {
            get { return duration; }
        }

        public string Text
        {
            get { return text; }
        }

        public ToastView View
        {
            get { return view; }
        }

        public void Cancel()
        {
            if (view == null || view.Owner == null)
                return;

            if (!view.Visibility)
            {
                GameObject.Destroy(view.Owner);
                return;
            }

            if (view.ExitAnimation != null)
            {
                view.ExitAnimation.OnEnd(() =>
                {
                    view.Visibility = false;
                    viewGroup.RemoveView(view);
                    GameObject.Destroy(view.Owner);
                    DoCallBack();
                }).Play();
            }
            else
            {
                view.Visibility = false;
                viewGroup.RemoveView(view);
                GameObject.Destroy(view.Owner);
                DoCallBack();
            }
        }

        public void Show()
        {
            if (view.Visibility)
                return;

            viewGroup.AddView(view, layout);
            view.Visibility = true;
            view.text.text = text;

            if (view.EnterAnimation != null)
                view.EnterAnimation.Play();

            view.StartCoroutine(DelayDismiss(duration));
        }

        protected IEnumerator DelayDismiss(float duration)
        {
            yield return new WaitForSeconds(duration);
            Cancel();
        }

        protected void DoCallBack()
        {
            try
            {
                if (callback != null)
                    callback();
            }
            catch (Exception)
            {

            }
        }
    }
}

