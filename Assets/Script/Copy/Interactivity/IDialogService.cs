using CFM.Framework.ViewModels;
using CFM.Framework.Asynchronous;

namespace CFM.Framework.Interactivity
{
    public interface IDialogService
    {
        IAsyncResult<int> ShowDialog(string title, string message);

        IAsyncResult<int> ShowDialog(string title, string message, string buttonText);

        IAsyncResult<int> ShowDialog(string title, string message, string confirmButtonText, string cancelButtonText);

        IAsyncResult<int> ShowDialog(string title, string message, string confirmButtonText, string cancelButtonText, string neutralButtonText);

        IAsyncResult<int> ShowDialog(string title, string message, string confirmButtonText, string cancelButtonText, string neutralButtonText, bool canceledOnTouchOutside);

        IAsyncResult<IViewModel> ShowDialog(string viewName, IViewModel viewModel);

        IAsyncResult<TViewModel> ShowDialog<TViewModel>(string viewName, TViewModel viewModel) where TViewModel : IViewModel;
    }
}

