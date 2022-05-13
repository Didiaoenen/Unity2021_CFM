using System;
using UnityEngine.EventSystems;
using Assembly_CSharp.Assets.Script.Simple.Binding.Contexts;
using Object = UnityEngine.Object;

namespace Assembly_CSharp.Assets.Script.Simple.Binding
{
    public abstract class AbstractBinding : DisposableBase, IBinding
    {
        private IBindingContext bindingContext;

        private WeakReference target;

        private object dataContext;

        private bool disposed = false;

        public virtual object Target
        {
            get
            {
                var target = this.target != null ? this.target.Target : null;
                return IsAlive(target) ? target : null;
            }
        }

        public virtual object DataContext
        {
            get { return dataContext; }
            set
            {
                if (dataContext == value)
                    return;

                dataContext = value;
                OnDataContextChanged();
            }
        }

        public virtual IBindingContext BindingContext
        {
            get { return bindingContext; }
            set { bindingContext = value; }
        }

        public AbstractBinding(IBindingContext bindingContext, object dataContext, object target)
        {
            this.bindingContext = bindingContext;
            this.target = new WeakReference(target, false);
            this.dataContext = dataContext;
        }

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

        protected abstract void OnDataContextChanged();

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                bindingContext = null;
                dataContext = null;
                target = null;
                disposed = true;
                base.Dispose(disposing);
            }
        }
    }
}

