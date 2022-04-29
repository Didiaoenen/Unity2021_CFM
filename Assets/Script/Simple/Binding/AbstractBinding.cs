using System;
using UnityEngine.EventSystems;
using Assembly_CSharp.Assets.Script.Simple.Binding.Contexts;

namespace Assembly_CSharp.Assets.Script.Simple.Binding
{
    public abstract class AbstractBinding : IBinding
    {
        private IBindingContext bindingContext;

        private WeakReference target;

        private object dataContext;

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

        protected abstract void OnDataContextChanged();

        public void Dispose(bool disposing)
        {
            bindingContext = null;
            dataContext = null;
            target = null;
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

