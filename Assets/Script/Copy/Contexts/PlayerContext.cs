using CFM.Framework.Services;

namespace CFM.Framework.Contexts
{
    public class PlayerContext : Context
    {
        private string username;

        public string Username { get { return username; } }

        public PlayerContext(string username) : this(username, null)
        {
            this.username = username;
        }

        public PlayerContext(string username, IServiceContainer container) : base(container, ContextManager.GetApplicationContext())
        {
            this.username=username;
        }
    }
}

