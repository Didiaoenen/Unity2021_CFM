using System;

namespace CFM.Framework.Views.Animations
{
    public interface IAnimation
    {
        IAnimation OnStart(Action onStart);

        IAnimation OnEnd(Action onEnd);

        IAnimation Play();
    }
}

