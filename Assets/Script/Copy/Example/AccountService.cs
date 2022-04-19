using System;

using CFM.Framework.Asynchronous;

namespace CFM.Framework.Example
{
    public class AccountService : IAccountService
    {
        private IAccountRepository repository;

        public event EventHandler<LoginEventArgs> LoginFinished;

        public AccountService(IAccountRepository repository)
        {
            this.repository = repository;
        }

        public virtual IAsyncResult<Account> Register(Account account)
        {
            return repository.Save(account);
        }

        public virtual IAsyncResult<Account> Update(Account account)
        {
            return repository.Update(account);
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
                Account account = await GetAccount(username);
                if (account == null || !account.Password.Equals(password))
                {
                    promise.SetResult(null);
                    RaiseLoginFinished(false, null);
                }
                else
                {
                    promise.SetResult(account);
                    RaiseLoginFinished(true, account);
                }
            }
            catch (Exception e)
            {
                promise.SetException(e);
                RaiseLoginFinished(false, null);
            }
        }

        public virtual IAsyncResult<Account> GetAccount(string username)
        {
            return repository.Get(username);
        }

        protected virtual void RaiseLoginFinished(bool succeed, Account account)
        {
            try
            {
                if (LoginFinished != null)
                    LoginFinished(this, new LoginEventArgs(succeed, account));
            }
            catch (Exception) { }
        }
    }
}

