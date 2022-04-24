using System;
using System.Collections.Generic;

using CFM.Log;
using CFM.Framework.Binding.Binders;

namespace CFM.Framework.Binding.Contexts
{
    public class BindingContext : IBindingContext
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(BindingContext));

        private readonly string DEFAULT_KEY = "_KEY_";

        private readonly Dictionary<object, List<IBinding>> bindings = new Dictionary<object, List<IBinding>>();

        private IBinder binder;

        private object owner;

        private object dataContext;

        private readonly object _lock = new object();

        private EventHandler dataContextChanged;

        public event EventHandler DataContextChanged
        {
            add { lock (_lock) { dataContextChanged += value; } }
            remove { lock (_lock) { dataContextChanged -= value; } }
        }

        public BindingContext(IBinder binder) : this(null, binder, (object)null)
        {
        }

        public BindingContext(object owner, IBinder binder) : this(owner, binder, (object)null)
        {
        }

        public BindingContext(object owner, IBinder binder, object dataContext)
        {
            this.owner = owner;
            this.binder = binder;
            DataContext = dataContext;
        }

        public BindingContext(object owner, IBinder binder, IDictionary<object, IEnumerable<BindingDescription>> firstBindings) : this(owner, binder, null, firstBindings)
        {
        }

        public BindingContext(object owner, IBinder binder, object dataContext, IDictionary<object, IEnumerable<BindingDescription>> firstBindings)
        {
            this.owner = owner;
            this.binder = binder;
            DataContext = dataContext;

            if (firstBindings != null && firstBindings.Count > 0)
            {
                foreach (var kvp in firstBindings)
                {
                    Add(kvp.Key, kvp.Value);
                }
            }
        }

        protected IBinder Binder
        {
            get { return binder; }
        }

        public object Owner { get { return this.owner; } }

        public object DataContext
        {
            get { return dataContext; }
            set
            {
                if (dataContext == value)
                    return;

                dataContext = value;
                this.OnDataContextChanged();
                this.RaiseDataContextChanged();
            }
        }

        protected void RaiseDataContextChanged()
        {
            try
            {
                var handler = dataContextChanged;
                if (handler != null)
                    handler(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                if (log.IsWarnEnabled)
                    log.Warn(e);
            }
        }

        protected virtual void OnDataContextChanged()
        {
            try
            {
                foreach (var kv in bindings)
                {
                    foreach (var binding in kv.Value)
                    {
                        binding.DataContext = DataContext;
                    }
                }
            }
            catch (Exception e)
            {
                if (log.IsWarnEnabled)
                    log.Warn(e);
            }
        }

        protected List<IBinding> GetOrCreateList(object key)
        {
            if (key == null)
                key = DEFAULT_KEY;

            List<IBinding> list;
            if (bindings.TryGetValue(key, out list))
                return list;

            list = new List<IBinding>();
            bindings.Add(key, list);
            return list;
        }

        public void Add(IBinding binding, object key = null)
        {
            if (binding == null)
                return;

            List<IBinding> list = GetOrCreateList(key);
            binding.BindingContext = this;
            list.Add(binding);
        }

        public void Add(IEnumerable<IBinding> bindings, object key = null)
        {
            if (bindings == null)
                return;

            List<IBinding> list = GetOrCreateList(key);
            foreach (IBinding binding in bindings)
            {
                binding.BindingContext = this;
                list.Add(binding);
            }
        }

        public void Add(object target, BindingDescription description, object key = null)
        {
            IBinding binding = Binder.Bind(this, DataContext, target, description);
            Add(binding, key);
        }

        public void Add(object target, IEnumerable<BindingDescription> descriptions, object key = null)
        {
            IEnumerable<IBinding> bindings = Binder.Bind(this, DataContext, target, descriptions);
            Add(bindings, key);
        }

        public void Clear(object key)
        {
            if (key == null)
                return;

            List<IBinding> list = bindings[key];
            if (list != null && list.Count > 0)
            {
                foreach (IBinding binding in list)
                {
                    binding.Dispose();
                }
            }
            bindings.Remove(key);
        }

        public void Clear()
        {
            foreach (var kv in bindings)
            {
                foreach (var binding in kv.Value)
                {
                    binding.Dispose();
                }
            }
            bindings.Clear();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Clear();
                    owner = null;
                    binder = null;
                }
                disposed = true;
            }
        }

        ~BindingContext()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

