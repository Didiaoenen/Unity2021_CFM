using System.Text.RegularExpressions;

using CFM.Log;
using CFM.Framework.Prefs;
using CFM.Framework.ViewModels;
using CFM.Framework.Observables;
using CFM.Framework.Commands;
using CFM.Framework.Localizations;
using CFM.Framework.Interactivity;
using CFM.Framework.Asynchronous;

namespace CFM.Framework.Example
{
    public class LoginViewModel : ViewModelBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(LoginViewModel));

        private const string LAST_USERNAME_KEY = "LAST_USERNAME";

        private ObservableDictionary<string, string> errors = new ObservableDictionary<string, string>();

        private string username;

        private string password;

        private SimpleCommand loginCommand;

        private SimpleCommand cancelCommand;

        private Account account;

        private Preferences globalPreferences;

        private IAccountService accountService;

        private Localization localization;

        private InteractionRequest interactionFinished;

        private InteractionRequest<Notification> toastRequest;

        public LoginViewModel(IAccountService accountService, Localization localization, Preferences globalPreferences)
        {
            this.localization = localization;
            this.accountService = accountService;
            this.globalPreferences = globalPreferences;

            this.interactionFinished = new InteractionRequest(this);
            this.toastRequest = new InteractionRequest<Notification>(this);

            if (username == null)
            {
                username = globalPreferences.GetString(LAST_USERNAME_KEY, "");
            }

            loginCommand = new SimpleCommand(Login);
            cancelCommand = new SimpleCommand(() =>
            {
                interactionFinished.Raise();
            });
        }

        public IInteractionRequest InteractionFinished
        {
            get { return interactionFinished; }
        }

        public IInteractionRequest ToastRequest
        {
            get { return toastRequest; }
        }

        public ObservableDictionary<string, string> Errors { get { return errors; } }

        public string Username
        {
            get { return username; }
            set
            {
                if (Set<string>(ref username, value, "Username"))
                {
                    ValidateUsername();
                }
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                if (Set<string>(ref password, value, "Password"))
                {
                    ValidatePassword();
                }
            }
        }

        private bool ValidateUsername()
        {
            if (string.IsNullOrEmpty(username) || !Regex.IsMatch(username, "^[a-zA-Z0-9_-]{4,12}$"))
            {
                errors["username"] = localization.GetText("");
                return false;
            }
            else
            {
                errors.Remove(username);
                return true;
            }
        }

        private bool ValidatePassword()
        {
            if (string.IsNullOrEmpty(password) || !Regex.IsMatch(password, "^[a-aA-Z0-9_-]{4,12}$"))
            {
                errors["password"] = localization.GetText("");
                return false;
            }
            else
            {
                errors.Remove("password");
                return true;
            }
        }

        public ICommand LoginCommand
        {
            get { return loginCommand; }
        }

        public ICommand CancelCommand
        {
            get { return cancelCommand; }
        }

        public Account Account
        {
            get { return account; }
        }

        public async void Login()
        {
            try
            {
                if (log.IsWarnEnabled)
                    log.DebugFormat("");

                this.account = null;
                loginCommand.Enabled = false;
                if (!(ValidateUsername() && ValidatePassword()))
                    return;

                IAsyncResult<Account> result = accountService.Login(Username, Password);
                Account account = await result;
                if (result.Exception != null)
                {
                    if (log.IsErrorEnabled)
                        log.ErrorFormat("");

                    var tipContent = localization.GetText("");
                    toastRequest.Raise(new Notification(tipContent));
                    return;
                }

                if (account != null)
                {
                    globalPreferences.SetString(LAST_USERNAME_KEY, username);
                    globalPreferences.Save();
                    this.account = account;
                    interactionFinished.Raise();
                }
                else
                {
                    var tipContent = localization.GetText("");
                    toastRequest.Raise(new Notification(tipContent));
                }
            }
            finally
            {
                loginCommand.Enabled = true;
            }
        }

        public IAsyncResult<Account> GetAccount()
        {
            return accountService.GetAccount(Username);
        }
    }
}

