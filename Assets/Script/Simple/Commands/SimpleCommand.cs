using System;

namespace Assembly_CSharp.Assets.Script.Simple.Commands
{
    public class SimpleCommand : CommandBase
    {
        private bool enabled = true;

        public bool Enabled
        {
            get { return enabled; }
            set
            {
                if (enabled == value)
                    return;

                enabled = value;
                RaiseCanExecuteChanged();
            }
        }

        private readonly Action execute;

        public SimpleCommand(Action execute, bool keepStrongRef = false)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            this.execute = keepStrongRef ? execute : execute.AsWeak();
        }

        public override bool CanExecute(object parameter)
        {
            return Enabled;
        }

        public override void Execute(object parameter)
        {
            if (CanExecute(parameter) && execute != null)
                execute();
        }
    }

    public class SimpleCommand<T> : CommandBase
    {
        private bool enabled = true;

        public bool Enabled
        {
            get { return enabled; }
            set
            {
                if (enabled == value)
                    return;

                enabled = value;
                RaiseCanExecuteChanged();
            }
        }

        private readonly Action<T> execute;

        public SimpleCommand(Action<T> execute, bool keepStrongRef = false)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            this.execute = keepStrongRef ? execute : execute.AsWeak();
        }

        public override bool CanExecute(object parameter)
        {
            return Enabled;
        }

        public override void Execute(object parameter)
        {
            if (CanExecute(parameter) && execute != null)
                execute((T)Convert.ChangeType(parameter, typeof(T)));
        }
    }
}

