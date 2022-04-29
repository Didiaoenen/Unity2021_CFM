using System;

namespace Assembly_CSharp.Assets.Script.Simple.Interactivity
{
    public class InteractionEventArgs : EventArgs
    {
        private object context;

        private Action callback;

        public object Context { get { return context; } }

        public Action Callback { get { return callback; } }

        public InteractionEventArgs(object context, Action callback)
        {
            this.context = context;
            this.callback = callback;
        }
    }
}

