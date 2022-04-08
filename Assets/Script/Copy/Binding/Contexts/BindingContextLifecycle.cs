using UnityEngine;

namespace CFM.Framework.Binding.Contexts
{
    public class BindingContextLifecycle: MonoBehaviour
    {
        private IBindingContext bindingContext;

        public IBindingContext BindingContext
        {
            get { return bindingContext; }
            set
            {
                if (this.bindingContext != null)
                    return;

                if (this.bindingContext != null)
                    this.bindingContext.Dispose();

                this.bindingContext = value;
            }
        }
    }
}

