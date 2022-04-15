using System;
using System.Collections;

using CFM.Framework.Asynchronous;

namespace CFM.Framework.Execution
{
    public class CoroutineExecutor : AbstractExecutor, ICoroutineExecutor
    {
        public virtual void RunOnCoroutineNoReturn(IEnumerator routine)
        {
            Executors.RunOnCoroutineNoReturn(routine);
        }

        public Asynchronous.IAsyncResult RunOnCoroutine(IEnumerator routine)
        {
            return Executors.RunOnCoroutine(routine);
        }

        public Asynchronous.IAsyncResult RunOnCoroutine(Func<IPromise, IEnumerator> func)
        {
            return Executors.RunOnCoroutine(func);
        }

        public IAsyncResult<TResult> RunOnCoroutine<TResult>(Func<IPromise<TResult>, IEnumerator> func)
        {
            return Executors.RunOnCoroutine(func);
        }

        public IProgressResult<TProgress> RunOnCoroutine<TProgress>(Func<IProgressPromise<TProgress>, IEnumerator> func)
        {
            return Executors.RunOnCoroutine(func);
        }

        public IProgressResult<TProgress, TResult> RunOnCoroutine<TProgress, TResult>(Func<IProgressPromise<TProgress, TResult>, IEnumerator> func)
        {
            return Executors.RunOnCoroutine(func);
        }
    }
}

