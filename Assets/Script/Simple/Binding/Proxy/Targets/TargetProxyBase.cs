using System;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Targets
{
    public abstract class TargetProxyBase : BindingProxyBase, ITargetProxy
    {
        private readonly WeakReference target;

        protected TypeCode typeCode = TypeCode.Empty;

        protected readonly string targetName;

        public TargetProxyBase(object target)
        {
            if (target != null)
            {
                this.target = new WeakReference(target, false);
                targetName = target.ToString();
            }
        }

        public abstract Type Type { get; }

        public virtual TypeCode TypeCode
        {
            get
            {
                if (typeCode == TypeCode.Empty)
                    typeCode = Type.GetTypeCode(Type);
                return typeCode;
            }
        }

        public virtual object Target
        {
            get
            {
                var target = this.target != null ? this.target.Target : null;
                return IsAlive(target) ? target : null;
            }
        }

        public virtual BindingMode DefaultMode { get { return BindingMode.OneWay; } }

        private bool IsAlive(object target)
        {
            try
            {
                if (target is UIBehaviour)
                {
                    if ((target as UIBehaviour).IsDestroyed())
                        return false;
                    return true;
                }

                if (target is Object)
                {
                    var name = (target as Object).name;
                    return true;
                }

                return target != null;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    public abstract class ValueTargetProxyBase : TargetProxyBase, IModifiable, IObtainable, INotifiable
    {
        private bool disposed = false;

        private bool subscribed = false;

        protected readonly object _lock = new object();

        protected EventHandler valueChanged;

        public event EventHandler ValueChanged
        {
            add
            {
                lock (_lock)
                {
                    valueChanged += value;

                    if (valueChanged != null && !subscribed)
                        Subscribe();
                }
            }

            remove
            {
                lock (_lock)
                {
                    valueChanged -= value;

                    if (valueChanged == null && subscribed)
                        Unsubscribe();
                }
            }
        }

        public ValueTargetProxyBase(object target) : base(target)
        {

        }

        public abstract object GetValue();

        public abstract TValue GetValue<TValue>();

        public abstract void SetValue(object value);

        public abstract void SetValue<TValue>(TValue value);

        protected void Subscribe()
        {
            try
            {
                if (subscribed)
                    return;

                var target = Target;
                if (target == null)
                    return;

                subscribed = true;
                DoSubscribeForValueChange(target);
            }
            catch (Exception e)
            {
            }
        }

        protected virtual void DoSubscribeForValueChange(object target)
        {
        }

        protected void Unsubscribe()
        {
            try
            {
                if (!subscribed)
                    return;

                var target = Target;
                if (target == null)
                    return;

                subscribed = false;
                DoUnsubscribeForValueChange(target);
            }
            catch (Exception e)
            {
            }
        }
        protected virtual void DoUnsubscribeForValueChange(object target)
        {
        }

        protected void RaiseValueChanged()
        {
            try
            {
                var handler = valueChanged;
                if (handler != null)
                    handler(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;
                lock (_lock)
                {
                    Unsubscribe();
                }
                base.Dispose(disposing);
            }
        }
    }

    public abstract class EventTargetProxyBase : TargetProxyBase, IModifiable
    {
        protected EventTargetProxyBase(object target) : base(target)
        {
        }

        public abstract void SetValue(object value);

        public abstract void SetValue<TValue>(TValue value);
    }
}

