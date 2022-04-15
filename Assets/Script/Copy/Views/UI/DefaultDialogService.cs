using System;

using CFM.Log;
using CFM.Framework.Binding;
using CFM.Framework.Contexts;
using CFM.Framework.Interactivity;
using CFM.Framework.ViewModels;
using CFM.Framework.Asynchronous;
using CFM.Framework.ViewModels.UI;

namespace CFM.Framework.Views.UI
{
    public class DefaultDialogService : IDialogService
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DefaultDialogService));

        public virtual IAsyncResult<int> ShowDialog(string title, string message)
        {
            return ShowDialog(title, message, null, null, null, true);
        }

        public virtual IAsyncResult<int> ShowDialog(string title, string message, string buttonText)
        {
            return ShowDialog(title, message, buttonText, null, null, false);
        }

        public virtual IAsyncResult<int> ShowDialog(string title, string message, string confirmButtonText, string cancelButtonText)
        {
            return ShowDialog(title, message, confirmButtonText, cancelButtonText, null, false);
        }

        public virtual IAsyncResult<int> ShowDialog(string title, string message, string confirmButtonText, string cancelButtonText, string neutralButtonText)
        {
            return ShowDialog(title, message, confirmButtonText, cancelButtonText, neutralButtonText, false);
        }

        public virtual IAsyncResult<int> ShowDialog(string title, string message, string confirmButtonText, string cancelButtonText, string neutralButtonText, bool canceledOnTouchOutside)
        {
            AsyncResult<int> result = new AsyncResult<int>();
            try
            {
                AlertDialog.ShowMessage(message, title, confirmButtonText, neutralButtonText, cancelButtonText, canceledOnTouchOutside, (which) => { result.SetResult(which); });
            }
            catch (Exception e)
            {
                result.SetException(e);
            }
            return result;
        }

        public virtual IAsyncResult<TViewModel> ShowDialog<TViewModel>(string viewName, TViewModel viewModel) where TViewModel : IViewModel
        {
            AsyncResult<TViewModel> result = new AsyncResult<TViewModel>();
            Window window = null;
            try
            {
                ApplicationContext context = ContextManager.GetApplicationContext();
                IUIViewLocator locator = context.GetService<IUIViewLocator>();
                if (locator == null)
                {
                    if (log.IsWarnEnabled)
                        log.Warn("Not found the \"IUIViewLocator\".");

                    throw new NotFoundException("Not found the \"IUIViewLocator\".");
                }

                if (string.IsNullOrEmpty(viewName))
                    throw new ArgumentNullException("The view name is null.");

                window = locator.LoadView<Window>(viewName);
                if (window == null)
                {
                    if (log.IsWarnEnabled)
                        log.WarnFormat("Not found the dialog window named \"{0}\".", viewName);

                    throw new NotFoundException(string.Format("Not found the dialog window named \"{0}\".", viewName));
                }

                if (window is AlertDialogWindow && viewModel is AlertDialogViewModel)
                    (window as AlertDialogWindow).ViewModel = viewModel as AlertDialogViewModel;
                else
                    window.SetDataContext(viewModel);

                EventHandler handler = null;
                handler = (sender, eventArgs) =>
                {
                    window.OnDismissed -= handler;
                    result.SetResult(viewModel);
                };
                window.Create();
                window.OnDismissed += handler;
                window.Show(true);
            }
            catch (Exception e)
            {
                result.SetException(e);
                if (window != null)
                    window.Dismiss();
            }
            return result;
        }

        public IAsyncResult<IViewModel> ShowDialog(string viewName, IViewModel viewModel)
        {
            return ShowDialog<IViewModel>(viewName, viewModel);
        }
    }
}

