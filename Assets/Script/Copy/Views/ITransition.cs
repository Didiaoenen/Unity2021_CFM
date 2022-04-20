using System;

using CFM.Framework.Asynchronous;

namespace CFM.Framework.Views
{
    public enum ActionType
    {
        None,
        Hide,
        Dismiss
    }

    public interface ITransition
    {
        bool IsDone{ get; }

        object WaitForDone();

#if NET_STANDARD_2_0
        IAwaiter GetAwaiter();
#endif

        ITransition DisableAnimation(bool disable);

        ITransition AtLayer(int layer);

        ITransition Overlay(Func<IWindow, IWindow, ActionType> policy);

        ITransition OnStart(Action callback);

        ITransition OnStateChanged(Action<IWindow, WindowState> callback);

        ITransition OnFinish(Action callback);
    }
}

