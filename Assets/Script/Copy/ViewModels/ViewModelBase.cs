using System.ComponentModel;
using System.Linq.Expressions;
using System;

using CFM.Log;
using CFM.Framework.Messaging;
using CFM.Framework.Observables;

namespace CFM.Framework.ViewModels
{
    public class ViewModelBase: ObservableObject, IViewModel
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ViewModelBase));

        private IMessenger messenger;

        public ViewModelBase(): this(null)
        {

        }

        public ViewModelBase(IMessenger messenger)
        {

        }

        public virtual IMessenger Messenger
        {
            get { return this.messenger; }
            set { this.messenger = value; }
        }

        protected void Broadcast<T>(T oldValue, T newValue, string propertyName)
        {

        }

        protected bool Set<T>(ref T field, T newValue, Expression<Func<T>> propertyExpression, bool broadcast = false)
        {
            return true;
        }

        protected bool Set<T>(ref T field, T newValue, string propertyName, bool broadcast = false)
        {
            return true;
        }

        protected bool Set<T>(ref T field, T newValue, PropertyChangedEventArgs eventArgs, bool broadcast = false)
        {
            return true;
        }

        ~ViewModelBase()
        {
            this.Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {

        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

