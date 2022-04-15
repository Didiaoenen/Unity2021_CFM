using System;

using CFM.Log;
using CFM.Framework.Binding;
using CFM.Framework.Contexts;
using CFM.Framework.Interactivity;
using CFM.Framework.Views.UI;
using CFM.Framework.ViewModels.UI;

namespace CFM.Framework.Views.InteractionActions
{
    public class DialogInteractionAction : InteractionActionBase<object>
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DialogInteractionAction));

        private string viewName;

        public DialogInteractionAction(string viewName)
        {
            this.viewName = viewName;
        }

        public override void Action(object viewModel, Action callback)
        {
            Window window = null;
            try
            {
                ApplicationContext context = ContextManager.GetApplicationContext();
                IUIViewLocator locator = context.GetService<IUIViewLocator>();
                if (locator == null)
                    throw new NotFoundException("Not found the \"IUIViewLocator\".");

                if (string.IsNullOrEmpty(viewName))
                    throw new ArgumentNullException("The view name is null.");

                window = locator.LoadView<Window>(viewName);
                if (window == null)
                    throw new NotFoundException(string.Format("Not found the dialog window named \"{0}\".", viewName));

                if (window is AlertDialogWindow && viewModel is AlertDialogViewModel)
                    (window as AlertDialogWindow).ViewModel = viewModel as AlertDialogViewModel;
                else
                    window.SetDataContext(viewModel);

                EventHandler handler = null;
                handler = (sender, eventArgs) =>
                {
                    window.OnDismissed -= handler;
                };
                window.Create();
                window.OnDismissed += handler;
                window.Show(true);
            }
            catch (Exception e)
            {
                if (window != null)
                    window.Dismiss();

                if (log.IsWarnEnabled)
                    log.Error("", e);
            }
        }
    }
}
