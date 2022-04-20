using System;

using CFM.Framework.Observables;

namespace CFM.Framework.Example
{
    public class Account : ObservableObject
    {
        private string username;

        private string password;

        private DateTime created;

        public string Username
        {
            get { return username; }
            set { Set(ref username, value, "Username"); }
        }

        public string Password
        {
            get { return password; }
            set { Set(ref password, value, "Password"); }
        }

        public DateTime Created
        {
            get { return created; }
            set { Set(ref created, value, "Created"); }
        }
    }
}

