namespace CFM.Framework.Interactivity
{
    public class Notification
    {
        private string title;

        private string message;

        public Notification(string message) : this(null, message)
        {

        }

        public Notification(string title, string message)
        {
            this.title = title;
            this.message = message;
        }

        public string Title
        {
            get { return title; }
        }

        public string Message
        {
            get { return message; }
        }
    }
}

