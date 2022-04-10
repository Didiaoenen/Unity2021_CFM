using CFM.Log;
using CFM.Framework.ViewModels;
using CFM.Framework.Observables;

namespace CFM.Framework.Example
{
    public class LoginViewModel : ViewModelBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(LoginViewModel));

        private const string LAST_USERNAME_KEY = "LAST_USERNAME";

        private ObservableDictionary<string, string> errors = new ObservableDictionary<string, string>();


    }
}

