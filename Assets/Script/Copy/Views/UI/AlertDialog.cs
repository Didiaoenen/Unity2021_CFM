using System;

using UnityEngine;

using CFM.Log;
using CFM.Framework.Contexts;
using CFM.Framework.Execution;
using CFM.Framework.ViewModels.UI;
using CFM.Framework.Views.Locators;

namespace CFM.Framework.Views.UI
{
    public class AlertDialog : IDialog
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AlertDialog));

        public const int BUTTON_POSITIVE = -1;

        public const int BUTTON_NEGATIVE = -2;

        public const int BUTTON_NEUTRAL = -3;

        private const string DEFAULT_VIEW_LOCATOR_KEY = "_DEFAULT_VIEW_LOCATOR";

        private const string DEFAULT_VIEW_NAME = "";

        private static string viewName;

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

        public static AlertDialog ShowMessage(
            string message,
            string title)
        {
            return ShowMessage(message, title, null, null, null, true, null);
        }

        public static AlertDialog ShowMessage(
            string message,
            string title,
            string buttonText,
            Action<int> afterHideCallback)
        {
            return ShowMessage(message, title, buttonText, null, null, false, afterHideCallback);
        }

        public static AlertDialog ShowMessage(
            string message,
            string title,
            string confirmButtonText,
            string cancelButtonText,
            Action<int> afterHideCallback)
        {
            return ShowMessage(message, title, confirmButtonText, null, cancelButtonText, false, afterHideCallback);
        }

        public static AlertDialog ShowMessage(
            string message,
            string title,
            string confirmButtonText,
            string neutralButtonText,
            string cancelButtonText,
            bool canceledOnTouchOutside,
            Action<int> afterHideCallback)
        {
            AlertDialogViewModel viewModel = new AlertDialogViewModel();
            viewModel.Message = message;
            viewModel.Title = title;
            viewModel.ConfirmButtonText = confirmButtonText;
            viewModel.NeutralButtonText = neutralButtonText;
            viewModel.CancelButtonText = cancelButtonText;
            viewModel.CanceledOnTouchOutside = canceledOnTouchOutside;
            viewModel.Click = afterHideCallback;

            return ShowMessage(ViewName, viewModel);
        }

        public static AlertDialog ShowMessage(
            IUIView contentView,
            string title,
            string confirmButtonText,
            string neutralButtonText,
            string cancelButtonText,
            bool canceledOnTouchOutside,
            Action<int> afterHideCallback)
        {
            AlertDialogViewModel viewModel = new AlertDialogViewModel();
            viewModel.Title = title;
            viewModel.ConfirmButtonText = confirmButtonText;
            viewModel.NeutralButtonText = neutralButtonText;
            viewModel.CancelButtonText = cancelButtonText;
            viewModel.CanceledOnTouchOutside = canceledOnTouchOutside;
            viewModel.Click = afterHideCallback;

            IUIViewLocator locator = GetUIViewLocator();
            AlertDialogWindow window = locator.LoadView<AlertDialogWindow>(ViewName);
            if (window == null)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("Not found the dialog window named \"{0}\".", viewName);

                throw new NotFoundException(string.Format("Not found the dialog window named \"{0}\".", viewName));
            }

            AlertDialog dialog = new AlertDialog(window, contentView, viewModel);
            dialog.Show();
            return dialog;
        }

        public static AlertDialog ShowMessage(AlertDialogViewModel viewModel)
        {
            return ShowMessage(ViewName, null, viewModel);
        }

        public static AlertDialog ShowMessage(string viewName, AlertDialogViewModel viewModel)
        {
            return ShowMessage(viewName, null, viewModel);
        }

        public static AlertDialog ShowMessage(string viewName, string contentViewName, AlertDialogViewModel viewModel)
        {
            AlertDialogWindow window = null;
            IUIView contentView = null;
            try
            {
                if (string.IsNullOrEmpty(viewName))
                    viewName = ViewName;

                IUIViewLocator locator = GetUIViewLocator();
                window = locator.LoadView<AlertDialogWindow>(viewName);
                if (window == null)
                {
                    if (log.IsWarnEnabled)
                        log.WarnFormat("Not found the dialog window named \"{0}\".", viewName);

                    throw new NotFoundException(string.Format("Not found the dialog window named \"{0}\".", viewName));
                }

                if (!string.IsNullOrEmpty(contentViewName))
                    contentView = locator.LoadView<IUIView>(contentViewName);

                AlertDialog dialog = new AlertDialog(window, contentView, viewModel);
                dialog.Show();
                return dialog;
            }
            catch (Exception e)
            {
                if (window != null)
                    window.Dismiss();
                if (contentView != null)
                    GameObject.Destroy(contentView.Owner);

                throw e;
            }
        }

        private AlertDialogWindow window;

        private IUIView contentView;

        private AlertDialogViewModel viewModel;

        public AlertDialog(AlertDialogWindow window, AlertDialogViewModel viewModel) : this(window, null, viewModel)
        {
        }

        public AlertDialog(AlertDialogWindow window, IUIView contentView, AlertDialogViewModel viewModel)
        {
            this.window = window;
            this.contentView = contentView;
            this.viewModel = viewModel;
        }

        public virtual object WaitForClosed()
        {
            return Executors.WaitWhile(() => !viewModel.Closed);
        }

        public void Show()
        {
            window.ViewModel = viewModel;
            if (contentView != null)
                window.ContentView = contentView;
            window.Create();
            window.Show();
        }

        public void Cancel()
        {
            this.window.Cancel();
        }
    }
}

