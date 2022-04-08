using System.Runtime.CompilerServices;
using System;
using System.Diagnostics;
using System.ComponentModel;
using System.Linq.Expressions;

using CFM.Log;

namespace CFM.Framework.Observables
{
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ObservableObject));

        private readonly object _lock = new object();

        private PropertyChangedEventHandler propertyChanged;

        public event PropertyChangedEventHandler PropertyChanged
        {
            add { lock (_lock) { this.propertyChanged += value; } }
            remove { lock (_lock) { this.propertyChanged -= value; } }
        }

        [Conditional("DEBUG")]
        protected void VerifyPropertyName(string propertyName)
        {

        }

        protected virtual void RaisePropertyChanged(string propertyName = null)
        {

        }

        protected virtual void PaisePropertyChanged(PropertyChangedEventArgs eventArgs)
        {

        }

        protected virtual void RaisePropertyChanged(params PropertyChangedEventArgs[] eventArgs)
        {

        }

        protected virtual string ParserPropertyName(LambdaExpression propertyExpression)
        {
            return null;
        }

        protected bool Set<T>(ref T field, T newValue, Expression<Func<T>> propertyExpression)
        {
            return true;
        }

        protected bool Set<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            return true;
        }

        protected bool Set<T>(ref T field, T newValue, PropertyChangedEventArgs eventArgs)
        {
            return true;
        }
    }
}

