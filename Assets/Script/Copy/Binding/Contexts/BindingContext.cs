using System;
using System.Collections.Generic;

using CFM.Log;
using CFM.Framework.Binding.Binders;

namespace CFM.Framework.Binding.Contexts
{
    public class BindingContext : IBindingContext
    {
        public BindingContext(IBinder binder) : this(null, binder, (object)null)
        {
        }

        public BindingContext(object owner, IBinder binder) : this(owner, binder, (object)null)
        {
        }

        public BindingContext(object owner, IBinder binder, object dataContext)
        {
        }

        public object DataContext { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event EventHandler DataContextChanged;

        public void Add(IBinding binding, object key = null)
        {
            throw new NotImplementedException();
        }

        public void Add(IEnumerable<IBinding> bindings, object key = null)
        {
            throw new NotImplementedException();
        }

        public void Add(object target, BindingDescription description, object key = null)
        {
            throw new NotImplementedException();
        }

        public void Add(object target, IEnumerable<BindingDescription> descriptions, object key = null)
        {
            throw new NotImplementedException();
        }

        public void Clear(object key)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

