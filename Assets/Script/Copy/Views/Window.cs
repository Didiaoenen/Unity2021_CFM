using System.Runtime.InteropServices.ComTypes;
using System;
using UnityEngine;

using CFM.Log;
using CFM.Framework.Messaging;
using CFM.Framework.Asynchronous;
using IAsyncResult = CFM.Framework.Asynchronous.IAsyncResult;

namespace CFM.Framework.Views
{
    [DisallowMultipleComponent]
    public abstract class Window : WindowView, IWindow, IManageable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Window));

        public static readonly IMessenger Messenger = new Messenger();

        [SerializeField]
        private WindowType windowType = WindowType.FULL;

        [SerializeField]
        [Range(0, 10)]
        private int windowPriority = 0;

        [SerializeField]
        private bool stateBroadcast = true;

        private IWindowManager windowManager;

        private bool created = false;

        private bool dismissed = false;

        private bool activated = false;

        private ITransition dismissTransition;

        private WindowState state = WindowState.NONE;

        private readonly object _lock = new object();

        private EventHandler activatedChanged;

        private EventHandler visibilityChanged;

        private EventHandler onDismissed;

        private EventHandler<WindowStateEventArgs> stateChanged;

        public event EventHandler ActivatedChanged
        {
            add { lock (_lock) { activatedChanged += value; } }
            remove { lock (_lock) { activatedChanged -= value; } }
        }

        public event EventHandler VisibilityChanged
        {
            add { lock (_lock) { visibilityChanged += value; } }
            remove { lock (_lock) { visibilityChanged -= value; } }
        }

        public event EventHandler OnDismissed
        {
            add { lock (_lock) { onDismissed += value; } }
            remove { lock (_lock) { onDismissed -= value; } }
        }

        public event EventHandler<WindowStateEventArgs> StateChanged
        {
            add { lock (_lock) { stateChanged += value; } }
            remove { lock (_lock) { stateChanged -= value; } }
        }

        public IWindowManager WindowManager
        {
            get { return windowManager ?? (windowManager = GameObject.FindObjectOfType<GlobalWindowManagerBase>()); }
            set { windowManager = value; }
        }

        public bool Created { get { return created; } }

        public bool Dismissed { get { return dismissed; } }

        protected override void OnEnable()
        {
            base.OnEnable();
            RaiseVisibilityChanged();
        }

        protected override void OnDisable()
        {
            RaiseActivatedChanged();
            base.OnDisable();
        }

        public bool Activated
        {
            get { return activated; }
            protected set
            {
                if (activated == value)
                    return;

                activated = value;
                OnActivatedChanged();
                RaiseActivatedChanged();
            }
        }

        protected WindowState State
        {
            get { return state; }
            set
            {
                if (state.Equals(value))
                    return;

                WindowState old = state;
                state = value;
                RaiseStateChanged(old, state);
            }
        }

        public WindowType WindowType
        {
            get { return windowType; }
            set { windowType = value; }
        }

        public int WindowPriority
        {
            get { return windowPriority; }
            set
            {
                if (value < 0)
                    windowPriority = 0;
                else if (value > 10)
                    windowPriority = 10;
                else
                    windowPriority = value;
            }
        }

        protected void RaiseActivatedChanged()
        {
            try
            {
                if (activatedChanged != null)
                    activatedChanged(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("", e);
            }
        }

        protected void RaiseVisibilityChanged()
        {
            try
            {
                if (visibilityChanged != null)
                    visibilityChanged(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("", e);
            }
        }

        protected void RaiseOnDismissed()
        {
            try
            {
                if (onDismissed != null)
                    onDismissed(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("", e);
            }
        }

        protected void RaiseStateChanged(WindowState oldState, WindowState newState)
        {
            try
            {
                WindowStateEventArgs eventArgs = new WindowStateEventArgs(this, oldState, newState);
                if (GlobalSetting.enableWindowStateBroadcast && stateBroadcast)
                    Messenger.Publish(eventArgs);

                if (stateChanged != null)
                    stateChanged(this, eventArgs);
            }
            catch (Exception e)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("", e);
            }
        }

        public virtual IAsyncResult Activate(bool ignoreAnimation)
        {
            AsyncResult result = new AsyncResult();
            try
            {
                if (!Visibility)
                {
                    result.SetException(new InvalidOperationException("The window is not visible."));
                    return result;
                }

                if (Activated)
                {
                    result.SetResult();
                    return result;
                }

                if (!ignoreAnimation && ActivationAnimation != null)
                {
                    ActivationAnimation.OnStart(() =>
                    {
                        State = WindowState.ACTIVATION_ANIMATION_BEGIN;
                    }).OnEnd(() =>
                    {
                        State = WindowState.ACTIVATION_ANIMATION_END;
                        Activated = true;
                        State = WindowState.ACTIVATED;
                        result.SetResult();
                    }).Play();
                }
                else
                {
                    Activated = true;
                    State = WindowState.ACTIVATED;
                    result.SetResult();
                }
            }
            catch (Exception e)
            {
                result.SetException(e);
            }
            return result;
        }

        public virtual IAsyncResult Passivate(bool ignoreAnimation)
        {

            AsyncResult result = new AsyncResult();
            try
            {
                if (!Visibility)
                {
                    result.SetException(new InvalidOperationException("The window is not visible."));
                    return result;
                }

                if (!Activated)
                {
                    result.SetResult();
                    return result;
                }

                Activated = false;
                State = WindowState.PASSIVATED;

                if (!ignoreAnimation && PassivationAnimation != null)
                {
                    PassivationAnimation.OnStart(() =>
                    {
                        State = WindowState.PASSIVATION_ANIMATION_BEGIN;
                    }).OnEnd(() =>
                    {
                        State = WindowState.PASSIVATION_ANIMATION_END;
                        result.SetResult();
                    }).Play();
                }
                else
                {
                    result.SetResult();
                }
            }
            catch (Exception e)
            {
                result.SetException(e);
            }
            return result;
        }

        protected virtual void OnActivatedChanged()
        {
            Interactable = Activated;
        }

        public void Create(IBundle bundle = null)
        {
            if (dismissTransition != null || dismissed)
                throw new ObjectDisposedException(Name);

            if (created)
                return;

            State = WindowState.CREATE_BEGIN;
            Visibility = false;
            Interactable = Activated;
            OnCreate(bundle);
            WindowManager.Add(this);
            created = true;
            State = WindowState.CREATE_END;
        }

        protected abstract void OnCreate(IBundle bundle);

        public ITransition Show(bool ignoreAnimation = false)
        {
            if (dismissTransition != null || dismissed)
                throw new InvalidOperationException("The window has been destroyed");

            if (Visibility)
                throw new InvalidOperationException("The window is already visible.");

            return WindowManager.Show(this).DisableAnimation(ignoreAnimation);
        }

        public virtual IAsyncResult DoShow(bool ignoreAnimation = false)
        {
            AsyncResult result = new AsyncResult();
            Action<IPromise> action = promise =>
            {
                try
                {
                    if (!created)
                        Create();

                    OnShow();
                    Visibility = true;
                    State = WindowState.VISIBLE;
                    if (!ignoreAnimation && EnterAnimation != null)
                    {
                        EnterAnimation.OnStart(() =>
                        {
                            State = WindowState.ENTER_ANIMATION_BEGIN;
                        }).OnEnd(() =>
                        {
                            State = WindowState.ENTER_ANIMATION_END;
                            promise.SetResult();
                        }).Play();
                    }
                    else
                    {
                        promise.SetResult();
                    }
                }
                catch (Exception e)
                {
                    promise.SetException(e);

                    if (log.IsWarnEnabled)
                        log.WarnFormat("The window named \"{0}\" failed to open!Error:{1}", Name, e);
                }
            };

            action(result);
            return result;
        }

        protected virtual void OnShow()
        {

        }

        public ITransition Hide(bool ignoreAnimation = false)
        {
            if (!created)
                throw new InvalidOperationException("The window has not been created.");

            if (dismissed)
                throw new InvalidOperationException("The window has been destroyed.");

            if (!Visibility)
                throw new InvalidOperationException("The window is not visible.");

            return this.WindowManager.Hide(this).DisableAnimation(ignoreAnimation);
        }

        public virtual IAsyncResult DoHide(bool ignoreAnimation = false)
        {
            AsyncResult result = new AsyncResult();
            Action<IPromise> action = promise =>
            {
                try
                {
                    if (!ignoreAnimation && ExitAnimation != null)
                    {
                        ExitAnimation.OnStart(() =>
                        {
                            State = WindowState.EXIT_ANIMATION_BEGIN;
                        }).OnEnd(() =>
                        {
                            State = WindowState.EXIT_ANIMATION_END;
                            Visibility = false;
                            State = WindowState.INVISIBLE;
                            OnHide();
                            promise.SetResult();
                        }).Play();
                    }
                    else
                    {
                        Visibility = false;
                        State = WindowState.INVISIBLE;
                        OnHide();
                        promise.SetResult();
                    }
                }
                catch (Exception e)
                {
                    promise.SetException(e);

                    if (log.IsWarnEnabled)
                        log.WarnFormat("The window named \"{0}\" failed to hide!Error:{1}", this.Name, e);
                }
            };

            action(result);
            return result;
        }

        protected virtual void OnHide()
        {

        }

        public ITransition Dismiss(bool ignoreAnimation = false)
        {
            if (dismissTransition != null)
                return dismissTransition;

            if (dismissed)
                throw new InvalidOperationException(string.Format("The window[{0}] has been destroyed.", Name));

            dismissTransition = WindowManager.Dismiss(this).DisableAnimation(ignoreAnimation);
            return dismissTransition;
        }

        public virtual void DoDismiss()
        {
            try
            {
                if (!dismissed)
                {
                    State = WindowState.DISMISS_BEGIN;
                    dismissed = true;
                    OnDismiss();
                    RaiseOnDismissed();
                    WindowManager.Remove(this);

                    if (!IsDestroyed() && gameObject != null)
                        GameObject.Destroy(gameObject);
                    State = WindowState.DISMISS_END;
                    dismissTransition = null;
                }
            }
            catch (Exception e)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("The window named \"{0}\" failed to dismiss!Error:{1}", Name, e);
            }
        }

        protected virtual void OnDismiss()
        {

        }

        protected override void OnDestroy()
        {
            if (!Dismissed && dismissTransition == null)
            {
                Dismiss(true);
            }
            base.OnDestroy();
        }
    }
}

