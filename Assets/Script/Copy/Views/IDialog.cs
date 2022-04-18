namespace CFM.Framework.Views
{
    public interface IDialog
    {
        void Show();

        void Cancel();

        object WaitForClosed();
    }
}

