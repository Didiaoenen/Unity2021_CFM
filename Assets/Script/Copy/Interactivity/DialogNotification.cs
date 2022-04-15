namespace CFM.Framework.Interactivity
{
    public class DialogNotification : Notification
    {
        private string confirmButtonText;

        private string neutralButtonText;

        private string cancelButtonText;

        private bool canceledOnTouchOutside;

        private int dialogResult;

        public DialogNotification(string title, string message, string confirmButtonText, bool canceledOnTouchOutside = true) : this(title, message, confirmButtonText, null, null, canceledOnTouchOutside)
        {

        }

        public DialogNotification(string title, string message, string confirmButtonText, string cancelButtonText, bool canceledOnTouchOutside = true) : this(title, message, confirmButtonText, null, cancelButtonText, canceledOnTouchOutside)
        {

        }

        public DialogNotification(string title, string message, string confirmButtonText, string neutralButtonText, string cancelButtonText, bool canceledOnTouchOutside) : base(title, message)
        {
            this.confirmButtonText = confirmButtonText;
            this.neutralButtonText = neutralButtonText;
            this.cancelButtonText = cancelButtonText;
            this.canceledOnTouchOutside = canceledOnTouchOutside;
        }

        public string ConfirmButtonText
        {
            get { return confirmButtonText; }
        }

        public string NeutralButtonText
        {
            get { return neutralButtonText; }
        }

        public string CancelButtonText
        {
            get { return cancelButtonText; }
        }

        public bool CanceledOnTouchOutside
        {
            get { return canceledOnTouchOutside; }
        }

        public int DialogResult
        {
            get { return dialogResult; }
            set { dialogResult = value; }
        }
    }
}

