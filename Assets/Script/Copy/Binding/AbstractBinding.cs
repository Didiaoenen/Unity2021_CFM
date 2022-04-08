using System;
using UnityEngine.EventSystems;
using CFM.Framework.Binding.Contexts;

namespace CFM.Framework.Binding
{
    public abstract class AbstractBinding: IBinding
    {
        private IBindingContext bindingContext;

        private WeakReference target;

        private object dataContext;

        public AbstractBinding(IBindingContext bindingContext, object dataContext, object target)
        {

        }

        public virtual IBindingContext BindingContext
        {
            get { return this.bindingContext; }
            set { this.bindingContext = value; }
        }

        public virtual object Target 
        {
            get
            {
                var target = this.target != null ? this.target.Target : null;
                return IsAlive(target) ? target : null;
            }
        }

        private bool IsAlive(object target)
        {
            try
            {
                if (target is UIBehaviour)
                {
                    if (((UIBehaviour)target).IsDestroyed())
                        return false;
                    return true;
                }

                if (target is UnityEngine.Object)
                {
                    var name = ((UnityEngine.Object)target).name;
                    return true;
                }

                return target != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual object DataContext
        {
            get { return this.dataContext; }
            set
            {
                if (this.dataContext == value)
                    return;

                this.dataContext = value;
                this.OnDataContextChanged();
            }
        }

        protected abstract void OnDataContextChanged();

        protected virtual void Dispose(bool disposing)
        {
            this.bindingContext = null;
            this.dataContext = null;
            this.target = null;
        }

        ~AbstractBinding()
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

