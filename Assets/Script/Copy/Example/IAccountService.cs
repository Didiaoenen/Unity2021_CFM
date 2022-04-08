using System;

using CFM.Framework.Asynchronous;

namespace CFM.Framework.Example
{
    public class LoginEventArgs: EventArgs
    {
        public LoginEventArgs(bool succeed, Account account)
        {
            this.IsSucceed = succeed;
            this.Account = account;
        }

        public bool IsSucceed { get; private set; }

        public Account Account { get; private set; }
    }

    public interface IAccountService
    {
        event EventHandler<LoginEventArgs> LoginFinished;

        IAsyncResult<Account> Register(Account account);

        IAsyncResult<Account> Update(Account account);

        IAsyncResult<Account> Login(string username, string password);

        IAsyncResult<Account> GetAccount(string username);
    }
}

