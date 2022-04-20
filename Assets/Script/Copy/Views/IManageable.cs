using IAsyncResult = CFM.Framework.Asynchronous.IAsyncResult;

namespace CFM.Framework.Views
{
    public interface IManageable : IWindow
    {
        IAsyncResult Activate(bool ignoreAnimation);

        IAsyncResult Passivate(bool ignoreAnimation);

        IAsyncResult DoShow(bool ignoreAnimation = false);

        IAsyncResult DoHide(bool ignoreAnimation = false);

        void DoDismiss();
    }
}

