using System;

namespace Assembly_CSharp.Assets.Script.Simple.Interactivity
{
    public interface IInteractionRequest
    {
        event EventHandler<InteractionEventArgs> Raised;
    }
}

