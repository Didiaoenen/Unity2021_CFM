using System;

using CFM.Framework.Views.UI;
using CFM.Framework.Interactivity;

namespace CFM.Framework.Views.InterationActions
{
    public class ToastInteractionAction : InteractionActionBase<ToastNotification>
    {
        private string viewName;

        private IUIViewGroup viewGroup;

        public ToastInteractionAction(IUIViewGroup viewGroup) : this(viewGroup, null)
        {
        }

        public ToastInteractionAction(IUIViewGroup viewGroup, string viewName)
        {
            this.viewGroup = viewGroup;
            this.viewName = viewName;
        }

        public override void Action(ToastNotification notification, Action callback)
        {
            if (notification == null)
                return;

            Toast.Show(viewName, viewGroup, notification.Message, notification.Duration, null, callback);
        }
    }
}

