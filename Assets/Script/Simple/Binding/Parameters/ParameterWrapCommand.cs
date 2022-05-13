using System;
using Assembly_CSharp.Assets.Script.Simple.Commands;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Parameters
{
    public class ParameterWrapCommand : ICommand
    {
        private readonly object _lock = new object();

        private readonly ICommand wrappedCommand;

        private readonly object commandParameter;

        public ParameterWrapCommand(ICommand wrappedCommand, object commandParameter)
        {
            if (wrappedCommand == null)
                throw new ArgumentNullException(nameof(wrappedCommand));

            if (commandParameter == null)
                throw new ArgumentNullException(nameof(commandParameter));

            this.wrappedCommand = wrappedCommand;
            this.commandParameter = commandParameter;
        }

        public event EventHandler CanExecuteChanged
        {
            add { lock(_lock) { wrappedCommand.CanExecuteChanged += value; } }
            remove { lock(_lock) { wrappedCommand.CanExecuteChanged -= value; } }
        }

        public bool CanExecute(object parameter)
        {
            return wrappedCommand.CanExecute(commandParameter);
        }

        public void Execute(object parameter)
        {
            if (CanExecute(commandParameter))
                wrappedCommand.Execute(commandParameter);
        }
    }
}

