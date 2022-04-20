using System;

namespace CFM.Framework.ViewModels.UI
{
    public class AlertDialogViewModel : ViewModelBase
    {
        protected string title;

        protected string message;

        protected string confirmButtonText;

        protected string neutralButtonText;

        protected string cancelButtonText;

        protected bool canceledOnTouchOutside;

        protected bool closed;

        protected int result;
        
        protected Action<int> click;

        public virtual string Title
        {
            get { return title; }
            set { Set(ref title, value, "Title"); }
        }

        public virtual string Message
        {
            get { return message; }
            set { Set(ref message, value, "Message"); }
        }

        public virtual string ConfirmButtonText
        {
            get { return confirmButtonText; }
            set { Set(ref confirmButtonText, value, "ConfirmButtonText"); }
        }

        public virtual string NeutralButtonText
        {
            get { return neutralButtonText; }
            set { Set(ref neutralButtonText, value, "NeutralButtonText"); }
        }

        public virtual string CancelButtonText
        {
            get { return cancelButtonText; }
            set { Set(ref cancelButtonText, value, "CancelButtonText"); }
        }

        public virtual bool CanceledOnTouchOutside
        {
            get { return canceledOnTouchOutside; }
            set { Set(ref canceledOnTouchOutside, value, "CanceledOnTouchOutside"); }
        }

        public virtual Action<int> Click
        {
            get { return click; }
            set { Set(ref click, value, "Click"); }
        }

        public virtual bool Closed
        {
            get { return closed; }
            protected set { Set(ref closed, value, "Closed"); }
        }

        public virtual int Result
        {
            get { return result; }
        }

        public virtual void OnClick(int which)
        {
            try
            {
                result = which;
                var click = Click;
                if (click != null)
                    click(which);
            }
            catch (Exception) { }
            finally
            {
                Closed = true;
            }
        }
    }
}

