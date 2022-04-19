using System.ComponentModel;
using System.Linq.Expressions;
using System;

using CFM.Log;
using CFM.Framework.Messaging;
using CFM.Framework.Observables;

namespace CFM.Framework.ViewModels
{
    public class ViewModelBase : ObservableObject, IViewModel
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ViewModelBase));

        private IMessenger messenger;

        public ViewModelBase(): this(null)
        {

        }

        public ViewModelBase(IMessenger messenger)
        {
            this.messenger = messenger;
        }

        public virtual IMessenger Messenger
        {
            get { return messenger; }
            set { messenger = value; }
        }

        protected void Broadcast<T>(T oldValue, T newValue, string propertyName)
        {
            try
            {
                var messenger = Messenger;
                if (messenger != null)
                    messenger.Publish(new PropertyChangedMessage<T>(this, oldValue, newValue, propertyName));
            }
            catch (Exception e)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("Set property '{0}', broadcast messages failure.Exception:{1}", propertyName, e);
            }
        }

        protected bool Set<T>(ref T field, T newValue, Expression<Func<T>> propertyExpression, bool broadcast = false)
        {
            if (Equals(field, newValue))
                return false;

            var oldValue = field;
            field = newValue;
            var propertyName = ParserPropertyName(propertyExpression);
            RaisePropertyChanged(propertyName);

            if (broadcast)
                Broadcast(oldValue, newValue, propertyName);
            return true;
        }

        protected bool Set<T>(ref T field, T newValue, string propertyName, bool broadcast = false)
        {
            if (Equals(field, newValue))
                return false;

            var oldValue = field;
            field = newValue;
            RaisePropertyChanged(propertyName);

            if (broadcast)
                Broadcast(oldValue, newValue, propertyName);
            return true;
        }

        protected bool Set<T>(ref T field, T newValue, PropertyChangedEventArgs eventArgs, bool broadcast = false)
        {
            if (Equals(field, newValue))
                return false;

            var oldValue = field;
            field = newValue;
            RaisePropertyChanged(eventArgs);

            if (broadcast)
                Broadcast(oldValue, newValue, eventArgs.PropertyName);
            return true;
        }

        ~ViewModelBase()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

