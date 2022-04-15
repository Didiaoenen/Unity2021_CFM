using System;

namespace CFM.Framework.Messaging
{
    public class MessageBase : EventArgs
    {
        public MessageBase(object sender)
        {
            Sender = sender;
        }

        public object Sender { get; protected set; }
    }
}

