using System;
using System.Collections;
using System.Runtime.CompilerServices;
using CFM.Log;
using CFM.Framework.Execution;
using CFM.Framework.Asynchronous;


namespace CFM.Framework.Views
{
    public abstract class Transition : ITransition
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Transition));

        private IManageable window;

        private bool done;

        private bool animationDisabled;

        private int layer;

        private Func<IWindow, IWindow, ActionType> overlayPolicy;

        private bool running;

        //
        private bool bound;

        private Action onStart;

        private Action<IWindow, WindowState> onStateChanged;

        private Action onFinish;

        public Transition(IManageable window)
        {
            this.window = window;
        }

        ~Transition()
        {
            this.Unbind();
        }

        protected virtual void Bind()
        {
            if (bound)
                return;

            this.bound = true;
            if (this.window != null)
                this.window.StateChanged += StateChanged;
        }

        protected virtual void Unbind()
        {
            if (!bound)
                return;

            this.bound = false;
            if (this.window != null)
                this.window.StateChanged -= StateChanged;
        }

        public virtual IManageable Window
        {
            get { return this.window; }
            set { this.window = value; }
        }

        public virtual bool IsDone
        {
            get { return this.done; }
            protected set { this.done = value; }
        }

        public virtual object WaitForDone()
        {
            return Executors.WaitWhile(() => !IsDone);
        }

        public virtual bool AnimationDisabled
        {
            get { return this.animationDisabled; }
            protected set { this.animationDisabled = value; }
        }

        public virtual int Layer
        {
            get { return this.layer; }
            protected set { this.layer = value; }
        }

        public virtual Func<IWindow, IWindow, ActionType> OverlayPolicy
        {
            get { return this.overlayPolicy; }
            protected set { this.overlayPolicy = value; }
        }

        protected void StateChanged(object send, WindowStateEventArgs e)
        {

        }

        protected virtual void RaiseStart()
        {
            try
            {
                if (this.onStart != null)
                    this.onStart();
            }
            catch (Exception e)
            {
                if (log.IsWarnEnabled)
                    log.Warn("", e);
            }
        }

        public virtual void RaiseStateChanged(IWindow window, WindowState state)
        {
            try
            {
                if (this.onStateChanged != null)
                    this.onStateChanged(window, state);
            }
            catch (Exception e)
            {
                if (log.IsWarnEnabled)
                    log.Warn("", e);
            }
        }

        protected virtual void RaiseFinished()
        {
            try
            {
                if (this.onFinish != null)
                    this.onFinish();
            }
            catch (Exception e)
            {
                if (log.IsWarnEnabled)
                    log.Warn("", e);
            }
        }

        protected virtual void OnStart()
        {
            this.Bind();
            this.RaiseStart();
        }

        protected virtual void OnEnd()
        {
            this.done = true;
            this.RaiseFinished();
            this.Unbind();
        }

#if NETFX_CORE || NET_STANDARD_2_0 || NET_4_6
        public IAwaiter GetAwaiter()
        {
            return new TransitionAwaiter(this);
        }
#endif

        public ITransition DisableAnimation(bool disabled)
        {
            if (this.running)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("");

                return this;
            }

            this.animationDisabled = disabled;
            return this;
        }

        public ITransition AtLayer(int layer)
        {
            if (this.running)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("");

                return this;
            }

            this.layer = layer;
            return this;
        }

        public ITransition Overlay(Func<IWindow, IWindow, ActionType> policy)
        {
            if (this.running)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("");

                return this;
            }

            this.overlayPolicy = policy;
            return this;
        }


        public ITransition OnStart(Action callback)
        {
            if (this.running)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("");

                return this;
            }

            this.onStart += callback;
            return this;
        }

        public ITransition OnStateChanged(Action<IWindow, WindowState> callback)
        {
            if (this.running)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("");

                return this;
            }

            this.onStateChanged += callback;
            return this;
        }

        public ITransition OnFinish(Action callback)
        {
            if (this.done)
            {
                callback();
                return this;
            }

            this.onFinish += callback;
            return this;
        }

        public virtual IEnumerator TransitionTask()
        {
            this.running = true;
            this.OnStart();
#if UNITY_5_3 || UNITY_5_3_OR_NEWER
            yield return this.DoTransition();
#else
            var transitionAction = this.DoTransition();
            while (transitionAction.MoveNext())
                yield return transitionAction.Current;
#endif
            this.OnEnd();
        }

        protected abstract IEnumerator DoTransition();
    }

#if NETFX_CORE || NET_STANDARD_2_0 || NET_4_6
    public struct TransitionAwaiter: IAwaiter, ICriticalNotifyCompletion
    {
        private Transition transition;

        public TransitionAwaiter(Transition transition)
        {
            this.transition = transition ?? throw new ArgumentNullException("transition");
        }

        public bool IsCompleted => transition.IsDone;

        public void GetResult()
        {
            if (!IsCompleted)
                throw new Exception("");
        }

        public void OnCompleted(Action continuation)
        {
            UnsafeOnCompleted(continuation);
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            if (continuation == null)
                throw new ArgumentNullException("continuation");
            
            transition.OnFinish(continuation);
        }
    }
#endif
}


