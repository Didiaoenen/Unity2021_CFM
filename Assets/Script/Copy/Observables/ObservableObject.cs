using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using System.Reflection;

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
            add { lock (_lock) { propertyChanged += value; } }
            remove { lock (_lock) { propertyChanged -= value; } }
        }

        [Conditional("DEBUG")]
        protected void VerifyPropertyName(string propertyName)
        {
            var type = GetType();
            if (!string.IsNullOrEmpty(propertyName) && type.GetProperty(propertyName) == null)
                throw new ArgumentException("Property not found", propertyName);
        }

        protected virtual void RaisePropertyChanged(string propertyName = null)
        {
            RaisePropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void PaisePropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            try
            {
                VerifyPropertyName(eventArgs.PropertyName);

                if (propertyChanged != null)
                    propertyChanged(this, eventArgs);
            }
            catch (Exception e)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("Set property '{0}', raise PropertyChanged failure.Exception:{1}", eventArgs.PropertyName, e);
            }
        }

        protected virtual void RaisePropertyChanged(params PropertyChangedEventArgs[] eventArgs)
        {
            foreach (var args in eventArgs)
            {
                try
                {
                    VerifyPropertyName(args.PropertyName);

                    if (propertyChanged != null)
                        propertyChanged(this, args);
                }
                catch (Exception e)
                {
                    if (log.IsWarnEnabled)
                        log.WarnFormat("Set property '{0}', raise PropertyChanged failure.Exception:{1}", args.PropertyName, e);
                }
            }
        }

        protected virtual string ParserPropertyName(LambdaExpression propertyExpression)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException("propertyExpression");

            var body = propertyExpression.Body as MemberExpression;
            if (body == null)
                throw new ArgumentException("Invalid argument", "propertyExpression");

            var property = body.Member as PropertyInfo;
            if (property == null)
                throw new ArgumentException("Argument is not a property", "propertyExpression");

            return property.Name;
        }

        protected bool Set<T>(ref T field, T newValue, Expression<Func<T>> propertyExpression)
        {
            if (Equals(field, newValue))
                return false;

            field = newValue;
            var propertyName = ParserPropertyName(propertyExpression);
            RaisePropertyChanged(propertyName);
            return true;
        }

        protected bool Set<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, newValue))
                return false;

            field = newValue;
            RaisePropertyChanged(propertyName);
            return true;
        }

        protected bool Set<T>(ref T field, T newValue, PropertyChangedEventArgs eventArgs)
        {
            if (Equals(field, newValue))
                return false;

            field = newValue;
            RaisePropertyChanged(eventArgs);
            return true;
        }
    }
}

