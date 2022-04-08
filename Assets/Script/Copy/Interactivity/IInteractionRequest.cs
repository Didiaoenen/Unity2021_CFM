using System;

namespace CFM.Framework.Interactivity
{
    public interface IInteractionRequest
    {
        event EventHandler<InteractionEventArgs> Raised;
    }
}

