using System;

using CFM.Framework.Observables;

namespace CFM.Framework.Example
{
    public class Account: ObservableObject
    {
        private string username;

        private string password;

        private DateTime created;

        public string Username
        {
            get { return this.username; }
            set { this.Set<string>(ref this.username, value, "Username"); }
        }

        public string Password
        {
            get { return this.password; }
            set { this.Set<string>(ref this.password, value, "Password"); }
        }

        public DateTime Created
        {
            get { return this.created; }
            set { this.Set<DateTime>(ref this.created, value, "Created"); }
        }
    }
}

