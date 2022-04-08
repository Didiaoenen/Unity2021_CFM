using System;

namespace CFM.Framework.Interactivity
{
    public class InteractionRequest : IInteractionRequest
    {
        private static readonly InteractionEventArgs emptyEventArgs = new InteractionEventArgs(null, null);

        private object sender;

        public InteractionRequest() : this(null)
        {

        }

        public InteractionRequest(object sender)
        {
            this.sender = sender != null ? sender : this;
        }

        public event EventHandler<InteractionEventArgs> Raised;
    
        public void Raise()
        {
            Raise(null);
        }

        public void Raise(Action callback)
        {
            var handler = Raised;
            if (handler != null)
                handler(sender, callback == null ? emptyEventArgs : new InteractionEventArgs(null, () => { if (callback != null) callback(); }));
        }
    }

    public class InteractionRequest<T> : IInteractionRequest
    {
        private static readonly InteractionEventArgs emptyEventArgs = new InteractionEventArgs(null, null);

        private object sender;

        public InteractionRequest() : this(null)
        {

        }

        public InteractionRequest(object sender)
        {
            this.sender = sender != null ? sender : this;
        }

        public event EventHandler<InteractionEventArgs> Raised;

        public void Raise(T context)
        {
            Raise(context, null);
        }

        public void Raise(T context, Action<T> callback)
        {
            var handler = Raised;
            if (handler != null)
                handler(sender, (context == null && callback == null) ? emptyEventArgs : new InteractionEventArgs(context, () => { if (callback != null) callback(context); }));
        }
    }
}

