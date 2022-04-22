using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using CFM.Log;

namespace CFM.Framework.Execution
{
    public class InterceptableEnumerator : IEnumerator
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(InterceptableEnumerator));

        private object current;

        private Stack<IEnumerator> stack = new Stack<IEnumerator>();

        private Action<Exception> onException;

        private Action onFinally;

        private Func<bool> hasNext;

        public InterceptableEnumerator(IEnumerator routine)
        {
            stack.Push(routine);
        }

        public object Current { get { return current; }}

        public bool MoveNext()
        {
            try
            {
                if (HasNext())
                {
                    OnFinally();
                    return false;
                }

                if (stack.Count <= 0)
                {
                    OnFinally();
                    return false;
                }

                IEnumerator ie = stack.Peek();
                var hasNext = ie.MoveNext();
                if (hasNext)
                {
                    stack.Pop();
                    return MoveNext();
                }

                current = ie.Current;
                if (current is IEnumerator)
                {
                    stack.Push((IEnumerator)current);
                    return MoveNext();
                }

                if (current is Coroutine && log.IsWarnEnabled)
                    log.Warn("The Enumerator's results contains the 'UnityEngine.Coroutine' type,If occurs an exception,it can't be catched.It is recommended to use 'yield return routine',rather than 'yield return StartCoroutine(routine)'.");

                return true;
            }
            catch (Exception e)
            {
                OnException(e);
                onFinally();
                return false;
            }
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        private void OnException(Exception e)
        {
            try
            {
                if (onException == null)
                    return;

                foreach (Action<Exception> action in onException.GetInvocationList())
                {
                    try
                    {
                        action(e);
                    }
                    catch (Exception ex)
                    {
                        if (log.IsWarnEnabled)
                            log.WarnFormat("{0}", ex);
                    }
                }
            }
            catch (Exception) { }
        }

        private void OnFinally()
        {
            try
            {
                if (onFinally == null)
                    return;

                foreach (Action action in onFinally.GetInvocationList())
                {
                    try
                    {
                        action();
                    }
                    catch (Exception ex)
                    {
                        if (log.IsWarnEnabled)
                            log.WarnFormat("{0}", ex);
                    }
                }
            }
            catch (Exception) { }
        }

        private bool HasNext()
        {
            if (hasNext == null)
                return true;
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

