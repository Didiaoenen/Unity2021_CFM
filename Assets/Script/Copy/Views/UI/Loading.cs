using System;

using CFM.Log;
using CFM.Framework.Contexts;
using CFM.Framework.Views.Locators;

namespace CFM.Framework.Views.UI
{
    public class Loading : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Loading));

        private const string DEFAULT_VIEW_LOCATOR_KEY = "_DEFAULT_VIEW_LOCATOR";

        private const string DEFAULT_VIEW_NAME = "";

        private static object _lock = new object();

        private static int refCount = 0;

        private static LoadingWindow window;

        private static string viewName;

        private bool ignoreAnimation;

        public static string ViewName
        {
            get { return string.IsNullOrEmpty(viewName) ? DEFAULT_VIEW_NAME : viewName; }
            set { viewName = value; }
        }

        private static IUIViewLocator GetUIViewLocator()
        {
            ApplicationContext context = ContextManager.GetApplicationContext();
            IUIViewLocator locator = context.GetService<IUIViewLocator>();
            if (locator == null)
            {
                if (log.IsWarnEnabled)
                    log.Warn("Not found the \"IUIViewLocator\" in the ApplicationContext.Try loading the AlertDialog using the DefaultUIViewLocator.");

                locator = context.GetService<IUIViewLocator>(DEFAULT_VIEW_LOCATOR_KEY);
                if (locator == null)
                {
                    locator = new DefaultUIViewLocator();
                    context.GetContainer().Register(DEFAULT_VIEW_LOCATOR_KEY, locator);
                }
            }
            return locator;
        }

        public static Loading Show(bool ignoreAnimation = false)
        {
            return new Loading(ignoreAnimation);
        }

        protected Loading(bool ignoreAnimation)
        {
            this.ignoreAnimation = ignoreAnimation;
            lock (_lock)
            {
                if (refCount <= 0)
                {
                    IUIViewLocator locator = GetUIViewLocator();
                    window = locator.LoadWindow<LoadingWindow>(ViewName);
                    window.Create();
                    window.Show(this.ignoreAnimation);
                }
                refCount++;
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;
                Execution.Executors.RunOnMainThread(() =>
                {
                    lock (_lock)
                    {
                        refCount--;
                        if (refCount <= 0)
                        {
                            window.Dismiss(ignoreAnimation);
                            window = null;
                        }
                    }
                });
            }
        }

        ~Loading()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

