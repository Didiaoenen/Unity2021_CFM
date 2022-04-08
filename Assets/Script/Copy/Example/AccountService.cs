using System;

using CFM.Framework.Asynchronous;

namespace CFM.Framework.Example
{
    public class AccountService: IAccountService
    {
        private IAccountRepository repository;

        public event EventHandler<LoginEventArgs> LoginFinished;

        public AccountService(IAccountRepository repository)
        {
            this.repository = repository;
        }

        public virtual IAsyncResult<Account> Register(Account account)
        {
            return this.repository.Save(account);
        }

        public virtual IAsyncResult<Account> Update(Account account)
        {
            return this.repository.Update(account);
        }

        public virtual IAsyncResult<Account> Login(string username, string password)
        {
            AsyncResult<Account> result = new AsyncResult<Account>();
            DoLogin(result, username, password);
            return result;
        }

        public async void DoLogin(IPromise<Account> promise, string username, string password)
        {
            try
            {
                Account account = await this.GetAccount(username);
                if (account == null || !account.Password.Equals(password))
                {
                    promise.SetResult(null);
                    this.RaiseLoginFinished(false, null);
                }
                else
                {
                    promise.SetResult(account);
                    this.RaiseLoginFinished(true, account);
                }
            }
            catch (Exception e)
            {
                promise.SetException(e);
                this.RaiseLoginFinished(false, null);
            }
        }

        public virtual IAsyncResult<Account> GetAccount(string username)
        {
            return this.repository.Get(username);
        }

        protected virtual void RaiseLoginFinished(bool succeed, Account account)
        {
            try
            {
                if (this.LoginFinished != null)
                    this.LoginFinished(this, new LoginEventArgs(succeed, account));
            }
            catch (Exception) { }
        }

        public async void Login()
        {
            IAsyncResult<Account> result = this.GetAccount("");
            Account account = await result;
        }
    }
}

