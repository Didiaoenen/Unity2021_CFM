namespace CFM.Framework.Interactivity
{
    public class ToastNotification
    {
        private readonly float duration;

        private readonly string message;

        public ToastNotification(string message) : this(message, 3f)
        {

        }

        public ToastNotification(string message, float duration)
        {
            this.message = message;
            this.duration = duration;
        }

        public float Duration
        {
            get { return duration; }
        }

        public string Message
        {
            get { return message; }
        }
    }
}

