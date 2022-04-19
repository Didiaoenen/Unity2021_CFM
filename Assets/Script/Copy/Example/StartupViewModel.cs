using UnityEngine;
using Object = UnityEngine.Object;

using CFM.Framework.Contexts;
using CFM.Framework.ViewModels;
using CFM.Framework.Commands;
using CFM.Framework.Localizations;
using CFM.Framework.Interactivity;
using CFM.Framework.Messaging;
using CFM.Framework.Asynchronous;

using CFM.Log;

namespace CFM.Framework.Example
{
    public class StartupViewModel : ViewModelBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(StartupViewModel));

        private ProgressBar progressBar = new ProgressBar();

        private SimpleCommand command;

        private Localization localization;

        private InteractionRequest<LoginViewModel> loginRequest;

        private InteractionRequest dismissRequest;

        public StartupViewModel() : this(null)
        {

        }

        public StartupViewModel(IMessenger messenger) : base(messenger)
        {
            ApplicationContext context = ContextManager.GetApplicationContext();
            localization = context.GetService<Localization>();
            var accountService = context.GetService<IAccountService>();
            var globalPreferences = context.GetGlobalPreferences();

            loginRequest = new InteractionRequest<LoginViewModel>(this);
            dismissRequest = new InteractionRequest(this);

            var loginViewModel = new LoginViewModel(accountService, localization, globalPreferences);

            command = new SimpleCommand(() =>
            {
                command.Enabled = false;
                loginRequest.Raise(loginViewModel, vm =>
                {
                    command.Enabled = true;

                    if (vm.Account != null)
                        LoadScene();
                });
            });
        }

        public ProgressBar ProgressBar
        {
            get { return progressBar; }
        }

        public ICommand Click
        {
            get { return command; }
        }

        public IInteractionRequest LoginRequest
        {
            get { return loginRequest; }
        }

        public IInteractionRequest DismissRequest
        {
            get { return dismissRequest; }
        }

        public void OnClick()
        {
            log.Debug("onClick");
        }

        public async void Unzip()
        {
            command.Enabled = false;
            progressBar.Enable = true;
            ProgressBar.Tip = "";

            try
            {
                var progress = 0f;
                while (progress < 1f)
                {
                    progress += 0.01f;
                    ProgressBar.Progress = progress;
                    await new WaitForSecondsRealtime(0.02f);
                }
            }
            finally
            {
                command.Enabled = true;
                progressBar.Enable = false;
                progressBar.Tip = "";
                command.Execute(null);
            }
        }

        public async void LoadScene()
        {
            try
            {
                progressBar.Enable = true;
                ProgressBar.Tip = "";

                ResourceRequest request = Resources.LoadAsync<GameObject>("");
                while (!request.isDone)
                {
                    ProgressBar.Progress = request.progress;
                    await new WaitForSecondsRealtime(0.02f);
                }

                GameObject sceneTemplate = (GameObject)request.asset;
                Object.Instantiate(sceneTemplate);
            }
            finally
            {
                ProgressBar.Tip = "";
                progressBar.Enable = false;
                dismissRequest.Raise();
            }
        }
    }
}

