using CFM.Log;
using CFM.Framework.ViewModels;
using CFM.Framework.Commands;
using CFM.Framework.Localizations;
using CFM.Framework.Interactivity;

namespace CFM.Framework.Example
{
    public class StartupViewModel : ViewModelBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(StartupViewModel));

        private ProgressBar progressBar = new ProgressBar();

        private SimpleCommand command;

        private Localization localizations;

        private InteractionRequest<LoginViewModel>
    }
}

