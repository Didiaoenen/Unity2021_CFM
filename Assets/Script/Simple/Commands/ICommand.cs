using System;

namespace Assembly_CSharp.Assets.Script.Simple.Commands
{
    public interface ICommand
    {
        event EventHandler CanExecuteChanged;

        bool CanExecute(object parameter);

        void Execute(object parameter);
    }
}

