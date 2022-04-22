using System;
using System.Collections;
using System.Collections.Generic;

using CFM.Framework.Execution;
using CFM.Framework.Asynchronous;

namespace CFM.Framework.Example
{
    public class AccountRepository : IAccountRepository
    {
        private Dictionary<string, Account> cache = new Dictionary<string, Account>();

        public AccountRepository()
        {
            Account account = new Account() { Username = "test", Password = "test", Created = DateTime.Now };
            cache.Add(account.Username, account);
        }

        public virtual IAsyncResult<Account> Get(string username)
        {
            return Executors.RunOnCoroutine<Account>(promise => DoGet(promise, username));
        }

        protected virtual IEnumerator DoGet(IPromise<Account> promise, string username)
        {
            Account account = null;
            cache.TryGetValue(username, out account);
            yield return null;
            promise.SetResult(account);
        }

        public virtual IAsyncResult<Account> Save(Account account)
        {
            return Executors.RunOnCoroutine<Account>(promise => DoSave(promise, account));
        }

        protected virtual IEnumerator DoSave(IPromise<Account> promise, Account account)
        {
            if (cache.ContainsKey(account.Username))
            {
                promise.SetException(new Exception(""));
                yield break;
            }

            cache.Add(account.Username, account);
            promise.SetResult(account);
        }

        public virtual IAsyncResult<Account> Update(Account account)
        {
            throw new NotImplementedException();
        } 

        public virtual IAsyncResult<bool> Delete(string username)
        {
            throw new NotImplementedException();
        }
    }
}

