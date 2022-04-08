using CFM.Framework.Execution;
using CFM.Framework.Prefs;
using CFM.Framework.Services;

namespace CFM.Framework.Contexts
{
    public class ApplicationContext: Context
    {
        private IMainLoopExecutor mainLoopExecutor;

        public ApplicationContext(): this(null, null)
        {

        }

        public ApplicationContext(IServiceContainer container, IMainLoopExecutor mainLooopExecutor): base(container, null)
        {
            this.mainLoopExecutor = mainLooopExecutor;
            if (this.mainLoopExecutor == null)
                this.mainLoopExecutor = new MainLoopExecutor();
        }

        public virtual IMainLoopExecutor GetMainLoopExecutor()
        {
            return this.mainLoopExecutor;
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


