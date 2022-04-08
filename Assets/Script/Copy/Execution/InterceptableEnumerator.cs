using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CFM.Log;

namespace CFM.Framework.Execution
{
    public class InterceptableEnumerator: IEnumerator
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(InterceptableEnumerator));

        private object current;

        private Stack<IEnumerator> stack = new Stack<IEnumerator>();

        private Action<Exception> onException;

        private Action onFinally;

        private Func<bool> hasNext;

        public InterceptableEnumerator(IEnumerator routine)
        {
            this.stack.Push(routine);
        }

        public object Current { get { return this.current; }}

        public bool MoveNext()
        {
            return false;
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        private void OnException(Exception e)
        {

        }

        private void OnFinally()
        {

        }

        private bool HasNext()
        {
            return hasNext();
        }

        public virtual void RegisterConditionBlock(Func<bool> hasNext)
        {
            this.hasNext = hasNext;
        }

        public virtual void RegisterCatchBlock(Action<Exception> onException)
        {
            this.onException += onException;
        }

        public virtual void RegisterFinallyBlock(Action onFinally)
        {
            this.onFinally += onFinally;
        }
    }
}

