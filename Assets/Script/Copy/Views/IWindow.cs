using System.Runtime.InteropServices.ComTypes;
using System;

namespace CFM.Framework.Views
{
    public enum WindowType
    {
        FULL,
        POPUP,
        DIALOG,
        PROGRESS,
        QUEUED_POPUP
    }

    public enum WindowState
    {
        NONE,
        CREATE_BEGIN,
        CREATE_END,
        ENTER_ANIMATION_BEGIN,
        VISIBLE,
        ENTER_ANIMATION_END,
        ACTIVATION_ANIMATION_BEGIN,
        ACTIVATED,
        ACTIVATION_ANIMATION_END,
        PASSIVATION_ANIMATION_BEGIN,
        PASSIVATED,
        PASSIVATION_ANIMATION_END,
        EXIT_ANIMATION_BEGIN,
        INVISIBLE,
        EXIT_ANIMATION_END,
        DISMISS_BEGIN,
        DISMISS_END
    }

    public interface IWindow
    {
        event EventHandler VisibilityChanged;

        event EventHandler ActivatedChanged;

        event EventHandler OnDismissed;

        event EventHandler<WindowStateEventArgs> StateChanged;

        string Name { get; set; }

        bool Created { get; }

        bool Dismissed { get; }

        bool Visibility { get; }

        bool Activated { get; }

        IWindowManager WindowManager { get; set; }

        WindowType WindowType { get; set; }

        int WindowPriority { get; set; }

        void Create(IBundle bundle);

        ITransition Show(bool ignoreAnimation = false);

        ITransition Hide(bool ignoreAnimation = false);

        ITransition Dismiss(bool ignoreAnimation = false);
    }

    public class WindowStateEventArgs : EventArgs
    {
        private readonly WindowState oldState;

        private readonly WindowState state;

        private readonly IWindow window;

        public WindowStateEventArgs(IWindow window, WindowState oldState, WindowState newState)
        {
            this.window = window;
            this.oldState = oldState;
            this.state = newState;
        }

        public WindowState OldState { get { return this.oldState; } }
        
        public WindowState State { get { return this.state; } }
        
        public IWindow Window { get { return this.window; } }
    }
}

