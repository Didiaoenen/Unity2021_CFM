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
            Unbind();
        }

        protected virtual void Bind()
        {
            if (bound)
                return;

            bound = true;
            if (window != null)
                window.StateChanged += StateChanged;
        }

        protected virtual void Unbind()
        {
            if (!bound)
                return;

            bound = false;
            if (window != null)
                window.StateChanged -= StateChanged;
        }

        public virtual IManageable Window
        {
            get { return window; }
            set { window = value; }
        }

        public virtual bool IsDone
        {
            get { return done; }
            protected set { done = value; }
        }

        public virtual object WaitForDone()
        {
            return Executors.WaitWhile(() => !IsDone);
        }

        public virtual bool AnimationDisabled
        {
            get { return animationDisabled; }
            protected set { animationDisabled = value; }
        }

        public virtual int Layer
        {
            get { return layer; }
            protected set { layer = value; }
        }

        public virtual Func<IWindow, IWindow, ActionType> OverlayPolicy
        {
            get { return overlayPolicy; }
            protected set { overlayPolicy = value; }
        }

        protected void StateChanged(object sender, WindowStateEventArgs e)
        {
            RaiseStateChanged((IWindow)sender, e.State);
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

#if NET_STANDARD_2_0
        public IAwaiter GetAwaiter()
        {
            return new TransitionAwaiter(this);
        }
#endif

        public ITransition DisableAnimation(bool disabled)
        {
            if (running)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("The transition is running.DisableAnimation failed.");

                return this;
            }

            animationDisabled = disabled;
            return this;
        }

        public ITransition AtLayer(int layer)
        {
            if (running)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("The transition is running.DisableAnimation failed.");

                return this;
            }

            this.layer = layer;
            return this;
        }

        public ITransition Overlay(Func<IWindow, IWindow, ActionType> policy)
        {
            if (running)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("");

                return this;
            }

            overlayPolicy = policy;
            return this;
        }


        public ITransition OnStart(Action callback)
        {
            if (running)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("");

                return this;
            }

            onStart += callback;
            return this;
        }

        public ITransition OnStateChanged(Action<IWindow, WindowState> callback)
        {
            if (running)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("");

                return this;
            }

            onStateChanged += callback;
            return this;
        }

        public ITransition OnFinish(Action callback)
        {
            if (done)
            {
                callback();
                return this;
            }

            onFinish += callback;
            return this;
        }

        public virtual IEnumerator TransitionTask()
        {
            this.running = true;
            this.OnStart();
            yield return this.DoTransition();

            var transitionAction = this.DoTransition();
            while (transitionAction.MoveNext())
                yield return transitionAction.Current;
            this.OnEnd();
        }

        protected abstract IEnumerator DoTransition();
    }

#if NET_STANDARD_2_0
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
                throw new Exception("The task is not finished yet");
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


