using System;

namespace CFM.Framework.Observables
{
    [Serializable]
    public class ObservablePropertyBase<T>
    {
        private readonly object _lock = new object();

        private EventHandler valueChanged;

        protected T _value;

        public event EventHandler ValueChanged
        {
            add { lock (_lock) { this.valueChanged += value; } }
            remove { lock (_lock) { this.valueChanged -= value; } }
        }

        public ObservablePropertyBase() : this(default(T))
        {

        }

        public ObservablePropertyBase(T value)
        {
            this._value = value;
        }

        public virtual Type Type { get { return typeof(T); } }

        protected void RaiseValueChanged()
        {
            var handle = this.valueChanged;
            if (handle != null)
                handle(this, EventArgs.Empty);
        }

        protected virtual bool Equals(T x, T y)
        {
            if (x != null)
            {
                if (y != null)
                    return x.Equals(y);
                return false;
            }

            if (y != null)
                return false;

            return true;
        }
    }

    [Serializable]
    public class ObservableProperty : ObservablePropertyBase<object>, IObservableProperty
    {
        public ObservableProperty() : this(null)
        {

        }

        public ObservableProperty(object value) : base(value)
        {

        }

        public override Type Type { get { return this._value != null ? this._value.GetType() : typeof(object); } }

        public object Value
        {
            get { return this._value; }
            set
            {
                if (this.Equals(this._value, value))
                    return;

                this._value = value;
                this.RaiseValueChanged();
            }
        }

        public override string ToString()
        {
            var v = this.Value;
            if (v == null)
                return "";

            return v.ToString();
        }
    }

    [Serializable]
    public class ObservableProperty<T>: ObservablePropertyBase<T>, IObservableProperty
    {
        public ObservableProperty(): this(default(T))
        {

        }


        public ObservableProperty(T value): base(value)
        {

        }

        public virtual T Value
        {
            get {  return this._value; }
            set
            {
                if (this.Equals(this._value, value))
                    return;

                this._value= value; ;
                this.RaiseValueChanged();
            }
        }

        object IObservableProperty.Value
        { 
            get { return this.Value; }
            set { this.Value = (T)value; }
        }

        public override string ToString()
        {
            var v = this.Value;
            if (v == null)
                return "";

            return v.ToString();
        }

        public static implicit operator T(ObservableProperty<T> data)
        {
            return data.Value;
        }

        public static implicit operator ObservableProperty<T>(T data)
        {
            return new ObservableProperty<T>(data);
        }
    }
}

