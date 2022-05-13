using XLua;
using System;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources.Object
{
    public abstract class LuaTableNodeProxy<TKey> : NotifiableSourceProxyBase, IObtainable, IModifiable
    {
        protected static readonly string PROPERTY_CHANGED_EVENT_SUBSCRIBE_NAME = "subscribe";

        protected static readonly string PROPERTY_CHANGED_EVENT_UNSUBSCRIBE_NAME = "unsubscribe";

        private bool disposed = false;

        protected readonly TKey key;

        public override Type Type { get { return typeof(object); } }

        public TKey Key { get { return key; } }

        public LuaTableNodeProxy(LuaTable source, TKey key) : base(source)
        {
            if (source == null)
                throw new ArgumentException(nameof(source));

            this.key = key;
        }

        protected abstract void Subscribe();

        protected abstract void Unsubscribe();

        protected virtual void OnPropertyChanged()
        {
            RaiseValueChanged();
        }

        public virtual object GetValue()
        {
            return (source as LuaTable).Get<TKey, object>(Key);
        }

        public TValue GetValue<TValue>()
        {
            return (source as LuaTable).Get<TKey, TValue>(Key);
        }

        public void SetValue(object value)
        {
            (source as LuaTable).Set(Key, value);
        }

        public void SetValue<TValue>(TValue value)
        {
            (source as LuaTable).Set(Key, value);
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                Unsubscribe();
                disposed = true;
                base.Dispose(disposing);
            }
        }
    }

    public class LuaIntTableNodeProxy : LuaTableNodeProxy<int>
    {
        private Action onPropertyChanged;

        private ILuaObservableObject observableObject;

        public LuaIntTableNodeProxy(LuaTable source, int key) : base(source, key)
        {
        
        }

        protected override void Subscribe()
        {
            if ((source as LuaTable).ContainsKey(PROPERTY_CHANGED_EVENT_SUBSCRIBE_NAME) && (source as LuaTable).ContainsKey(PROPERTY_CHANGED_EVENT_UNSUBSCRIBE_NAME))
            {
                observableObject = (source as LuaTable).Cast<ILuaObservableObject>();
                onPropertyChanged = OnPropertyChanged;
                observableObject.subscribe(Key, onPropertyChanged);
            }
            else
            {

            }
        }

        protected override void Unsubscribe()
        {
            if (observableObject != null)
                observableObject.unsubscribe(Key, onPropertyChanged);
        }
    }

    public class LuaStringTableNodeProxy : LuaTableNodeProxy<string>
    {
        private Action onPropertyChanged;

        private ILuaObservableObject observableObject;

        public LuaStringTableNodeProxy(LuaTable source, string key) : base(source, key)
        {

        }

        protected override void Subscribe()
        {
            if ((source as LuaTable).ContainsKey(PROPERTY_CHANGED_EVENT_SUBSCRIBE_NAME) && (source as LuaTable).ContainsKey(PROPERTY_CHANGED_EVENT_UNSUBSCRIBE_NAME))
            {
                observableObject = (source as LuaTable).Cast<ILuaObservableObject>();
                onPropertyChanged = OnPropertyChanged;
                observableObject.subscribe(Key, onPropertyChanged);
            }
            else
            {

            }
        }

        protected override void Unsubscribe()
        {
            if (observableObject != null)
                observableObject.unsubscribe(Key, onPropertyChanged);
        }
    }
}
