using System;
using System.Collections;
using System.Collections.Generic;

namespace Assembly_CSharp.Assets.Script.Simple.Execution
{
    public class InterceptableEnumerator : IEnumerator
    {
        private object current;

        private Stack<IEnumerator> stack = new Stack<IEnumerator>();

        private Action<Exception> onException;

        private Action onFinally;

        private Func<bool> hasNext;

        public object Current { get { return current; } }

        public InterceptableEnumerator(IEnumerator routine)
        {
            stack.Push(routine);
        }

        public bool MoveNext()
        {
            try
            {
                if (!HasNext())
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
                bool hasNext = ie.MoveNext();
                if (!hasNext)
                {
                    stack.Pop();
                    return MoveNext();
                }

                current = ie.Current;
                if (current is IEnumerator)
                {
                    stack.Push(current as IEnumerator);
                    return MoveNext();
                }

                return true;
            }
            catch (Exception e)
            {
                OnException(e);
                OnFinally();
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

