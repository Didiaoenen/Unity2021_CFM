using CFM.Framework.Asynchronous;

namespace CFM.Framework.Example
{
    public interface IAccountRepository
    {
        IAsyncResult<Account> Get(string username);

        IAsyncResult<Account> Save(Account account);

        IAsyncResult<Account> Update(Account account);

        IAsyncResult<bool> Delete(string username);
    }
}

