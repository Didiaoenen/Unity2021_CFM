using System;

using UnityEngine;
using UnityEngine.UI;

using CFM.Log;
using CFM.Framework.Views;
using CFM.Framework.Binding;
using CFM.Framework.Contexts;
using CFM.Framework.Interactivity;
using CFM.Framework.Binding.Builder;

namespace CFM.Framework.Example
{
    public class StartupWindow : Window
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(StartupWindow));

        public Text progressBarText;
        
        public Slider progressBarSlider;

        public Text tipText;

        public Button button;

        private StartupViewModel viewModel;

        private IDisposable subscription;

        private IUIViewLocator viewLocator;

        protected override void OnCreate(IBundle bundle)
        {
            viewLocator = ContextManager.GetApplicationContext().GetService<IUIViewLocator>();
            viewModel = new StartupViewModel();

            BindingSet<StartupWindow, StartupViewModel> bindingSet = this.CreateBindingSet(viewModel);
            bindingSet.Bind().For(v => v.OnOpenLoginWindow).To(vm => vm.LoginRequest);
            bindingSet.Bind().For(v => v.OnDismissRequest).To(vm => vm.DismissRequest);

            bindingSet.Bind(progressBarSlider).For("value", "onValueChanged").To("ProgressBar.Progress").TwoWay();

            bindingSet.Bind(progressBarSlider.gameObject).For(v => v.activeSelf).To(vm => vm.ProgressBar.Enable).OneWay();
            bindingSet.Bind(progressBarText).For(v => v.text).ToExpression(vm => string.Format("{0}%", Mathf.FloorToInt(vm.ProgressBar.Progress * 100f))).OneWay();
            bindingSet.Bind(tipText).For(v => v.text).To(vm => vm.ProgressBar.Tip).OneWay();
            bindingSet.Bind();

            viewModel.Unzip();
        }

        public override void DoDismiss()
        {
            base.DoDismiss();
            if (subscription != null)
            {
                subscription.Dispose();
                subscription = null;
            }
        }

        protected void OnDismissRequest(object sender, InteractionEventArgs args)
        {
            Dismiss();
        }

        protected void OnOpenLoginWindow(object sender, InteractionEventArgs args)
        {
            try
            {
                LoginWindow loginWindow = viewLocator.LoadWindow<LoginWindow>(WindowManager, "");
                var callback = args.Callback;
                var loginViewModel = args.Context;

                if (callback != null)
                {
                    EventHandler handler = null;
                    handler = (window, e) =>
                    {
                        loginWindow.OnDismissed -= handler;
                        if (callback != null)
                            callback();
                    };
                    loginWindow.OnDismissed += handler;
                }

                loginWindow.SetDataContext(loginViewModel);
                loginWindow.Create();
                loginWindow.Show();
            }
            catch (Exception e)
            {
                if (log.IsWarnEnabled)
                    log.Warn(e);
            }
        }
    }
}

