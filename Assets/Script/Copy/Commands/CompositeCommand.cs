using System;
using System.Linq;
using System.Collections.Generic;

using CFM.Log;

namespace CFM.Framework.Commands
{
    public class CompositeCommand : CommandBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CompositeCommand));

        private readonly List<ICommand> commands = new List<ICommand>();

        private readonly bool monitorCommandActivity;

        private readonly EventHandler onCanExecuteChangedHandler;

        public CompositeCommand() : this(false)
        {
        }

        public CompositeCommand(bool monitorCommandActivity)
        {
            this.monitorCommandActivity = monitorCommandActivity;
            onCanExecuteChangedHandler = new EventHandler(OnCanExecuteChanged);
        }

        public virtual void RegisterCommand(ICommand command)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            if (command == this)
                throw new ArgumentException("Cannot register a CompositeCommand in itself.");

            lock (commands)
            {
                if (commands.Contains(command))
                    throw new InvalidOperationException("Cannot register the same command twice in the same CompositeCommand.");

                commands.Add(command);
            }

            command.CanExecuteChanged += onCanExecuteChangedHandler;
            RaiseCanExecuteChanged();

            if (monitorCommandActivity)
            {
                var activeAwareCommand = command as IActiveAware;
                if (activeAwareCommand != null)
                    activeAwareCommand.IsActiveChanged += OnIsActiveChanged;
            }
        }

        public virtual void UnregisterCommand(ICommand command)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            lock (commands)
            {
                if (!commands.Remove(command))
                    return;
            }

            command.CanExecuteChanged -= onCanExecuteChangedHandler;
            RaiseCanExecuteChanged();

            if (monitorCommandActivity)
            {
                var activeAwareCommand = command as IActiveAware;
                if (activeAwareCommand != null)
                    activeAwareCommand.IsActiveChanged -= OnIsActiveChanged;
            }
        }

        private void OnCanExecuteChanged(object sender, EventArgs e)
        {
            RaiseCanExecuteChanged();
        }

        private void OnIsActiveChanged(object sender, EventArgs e)
        {
            RaiseCanExecuteChanged();
        }

        public override bool CanExecute(object parameter)
        {
            ICommand[] commandList;
            lock (commands)
            {
                commandList = commands.ToArray();
            }

            if (commandList.Length <= 0)
                return false;

            foreach (ICommand command in commandList)
            {
                if (!ShouldExecute(command))
                    continue;

                if (!command.CanExecute(parameter))
                    return false;
            }

            return true;
        }

        public override void Execute(object parameter)
        {
            Queue<ICommand> commands;
            lock (this.commands)
            {
                commands = new Queue<ICommand>(this.commands.Where(ShouldExecute).ToList());
            }

            while (commands.Count > 0)
            {
                try
                {
                    ICommand command = commands.Dequeue();
                    command.Execute(parameter);
                }
                catch (Exception e)
                {
                    if (log.IsWarnEnabled)
                        log.Warn(e);
                }
            }
        }

        protected virtual bool ShouldExecute(ICommand command)
        {
            if (!monitorCommandActivity)
                return true;

            var activeAwareCommand = command as IActiveAware;
            if (activeAwareCommand == null)
                return true;

            return activeAwareCommand.IsActive;
        }

        public IList<ICommand> RegisteredCommands
        {
            get
            {
                IList<ICommand> commandList;
                lock (commands)
                {
                    commandList = commands.ToList();
                }

                return commandList;
            }
        }
    }
}

