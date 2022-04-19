using CFM.Framework.Prefs;
using CFM.Framework.Services;
using CFM.Framework.Execution;

namespace CFM.Framework.Contexts
{
    public class ApplicationContext : Context
    {
        private IMainLoopExecutor mainLoopExecutor;

        public ApplicationContext(): this(null, null)
        {

        }

        public ApplicationContext(IServiceContainer container, IMainLoopExecutor mainLooopExecutor): base(container, null)
        {
            mainLoopExecutor = mainLooopExecutor;
            if (mainLoopExecutor == null)
                mainLoopExecutor = new MainLoopExecutor();
        }

        public virtual IMainLoopExecutor GetMainLoopExecutor()
        {
            return mainLoopExecutor;
        }

        public virtual Preferences GetGlobalPreferences()
        {
            return Preferences.GetGlobalPreferences();
        }

        public virtual Preferences GetUserPreferences(string name)
        {
            return Preferences.GetPreferences(name);
        }
    }
}


