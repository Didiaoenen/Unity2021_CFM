using System;

namespace Assembly_CSharp.Assets.Script.Simple.Commands
{
    public abstract class CommandBase : ICommand
    {
        private readonly object _lock = new object();

        private EventHandler canExecuteChanged;

        public event EventHandler CanExecutChanged
        {
            add { lock(_lock) { canExecuteChanged += value; } }
            remove { lock(_lock) { canExecuteChanged -= value; } }
        }

        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);

        public virtual void RaiseCanExecuteChanged()
        {
            var handler = canExecuteChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}

