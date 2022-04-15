using System;

namespace CFM.Framework.Asynchronous
{
    public interface IAsyncTask : IAsyncResult
    {
        IAsyncTask OnPreExecute(Action callback, bool runOnMainThread = true);

        IAsyncTask OnPostExecute(Action callback, bool runOnMainThread = true);

        IAsyncTask OnError(Action<Exception> callback, bool runOnMainThread = true);

        IAsyncTask OnFinished(Action callback, bool runOnMainThread = true);

        IAsyncTask Start();
    }

    public interface IAsyncTask<TResult> : IAsyncResult<TResult>
    {
        IAsyncTask<TResult> OnPreExecute(Action callback, bool runOnMainThread = true);

        IAsyncTask<TResult> OnPostExecute(Action<TResult> callback, bool runOnMainThread = true);

        IAsyncTask<TResult> OnError(Action<Exception> callback, bool runOnMainThread = true);

        IAsyncTask<TResult> OnFinished(Action callback, bool runOnMainThread = true);

        IAsyncTask<TResult> Start(int delay);

        IAsyncTask<TResult> Start();
    }
}

