using UnityEngine.UI;

using CFM.Framework.Views;
using CFM.Framework.Binding;
using CFM.Framework.Interactivity;
using CFM.Framework.Views.UI;
using CFM.Framework.Binding.Builder;

namespace CFM.Framework.Example
{
    public class LoginWindow : Window
    {
        public InputField userame;

        public InputField password;

        public Text usernameErrorPrompt;

        public Text passwordErrorPrompt;

        public Button confirmButton;

        public Button cancelButton;

        protected override void OnCreate(IBundle bundle)
        {
            BindingSet<LoginWindow, LoginViewModel> bindingSet = this.CreateBindingSet<LoginWindow, LoginViewModel>();
            bindingSet.Bind().For(v => v.OnInteractionFinished).To(vm => vm.InteractionFinished);
            bindingSet.Bind().For(v => v.OnToastShow).To(vm => vm.ToastRequest);

            bindingSet.Bind(userame).For(v => v.text, v => v.onEndEdit).To(vm => vm.Username).TwoWay();
            bindingSet.Bind(usernameErrorPrompt).For(v => v.text).To(vm => vm.Errors["username"]).OneWay();
            bindingSet.Bind(password).For(v => v.text, v => v.onEndEdit).To(vm => vm.Password).TwoWay();
            bindingSet.Bind(passwordErrorPrompt).For(v => v.text).To(vm => vm.Errors["password"]).TwoWay();
            bindingSet.Bind(confirmButton).For(v => v.onClick).To(vm => vm.LoginCommand);
            bindingSet.Bind(cancelButton).For(v => v.onClick).To(vm => vm.CancelCommand);
            bindingSet.Build();
        }

        public virtual void OnInteractionFinished(object sender, InteractionEventArgs args)
        {
            Dismiss();
        }

        public virtual void OnToastShow(object sender, InteractionEventArgs args)
        {
            Notification notification = args.Context as Notification;
            if (notification == null)
                return;

            Toast.Show(this, notification.Message, 2f);
        }
    }
}

