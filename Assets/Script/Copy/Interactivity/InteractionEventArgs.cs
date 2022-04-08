using System;

namespace CFM.Framework.Interactivity
{
    public class InteractionEventArgs : EventArgs
    {
        private object context;

        private Action callback;

        public InteractionEventArgs(object context, Action callback)
        {
            this.context = context;
            this.callback = callback;
        }

        public object Context { get { return context; } }

        public Action Callback { get { return callback; } }
    }
}

