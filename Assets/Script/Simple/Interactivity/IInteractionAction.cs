namespace Assembly_CSharp.Assets.Script.Simple.Interactivity
{
    public interface IInteractionAction
    {
        void OnRequest(object sender, InteractionEventArgs args);
    }
}

