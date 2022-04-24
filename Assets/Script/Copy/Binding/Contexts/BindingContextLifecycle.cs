using UnityEngine;

namespace CFM.Framework.Binding.Contexts
{
    public class BindingContextLifecycle : MonoBehaviour
    {
        private IBindingContext bindingContext;

        public IBindingContext BindingContext
        {
            get { return bindingContext; }
            set
            {
                if (bindingContext != null)
                    return;

                if (bindingContext != null)
                    bindingContext.Dispose();

                bindingContext = value;
            }
        }

        protected virtual void OnDestroy()
        {
            if (bindingContext != null)
            {
                bindingContext.Dispose();
                bindingContext = null;
            }
        }
    }
}

