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
    public class StartupWindow: Window
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
            this.viewLocator = ContextManager.GetApplicationContext().GetService<IUIViewLocator>();
            this.viewModel = new StartupViewModel();

            BindingSet<StartupWindow, StartupViewModel> bindingSet = this.CreateBindingSet(viewModel);
            bindingSet.Bind().For(v => v.OnOpenLoginWindow).To(vm => vm.LoginRequest);
        }

        public override void DoDismiss()
        {
            base.DoDismiss();
        }

        protected void DoDismissRequest(object sender, InteractionEventArgs args)
        {

        }

        protected void OnOpenLoginWindow(object sender, InteractionEventArgs args)
        {
            
        }
    }
}

