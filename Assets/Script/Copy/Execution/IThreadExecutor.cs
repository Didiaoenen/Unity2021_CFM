using System;

using CFM.Framework.Asynchronous;

namespace CFM.Framework.Execution
{
    public interface IThreadExecutor
    {
        Asynchronous.IAsyncResult Execute(Action action);

        IAsyncResult<TResult> Execute<TResult>(Func<TResult> func);

        Asynchronous.IAsyncResult Execute(Action<IPromise> action);

        IProgressResult<TProgress> Execute<TProgress>(Action<IProgressPromise<TProgress>> action);

        IAsyncResult<TResult> Execute<TResult>(Action<IPromise<TResult>> action);

        IProgressResult<TProgress, TResult> Execute<TProgress, TResult>(Action<IProgressPromise<TProgress, TResult>> action);
    }
}

