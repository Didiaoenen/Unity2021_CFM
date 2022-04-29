using UnityEngine;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Contexts
{
    public class BindingContextLifecycle : MonoBehaviour
    {
        private IBindingContext bindingContext;

        public IBindingContext BindingContext
        {
            get { return bindingContext; }
            set
            {
                if (bindingContext == value)
                    return;

                if (bindingContext != null)
                    bindingContext.Dispose();

                bindingContext = value;
            }
        }

        public virtual void OnDestroy()
        {
            if (bindingContext != null)
            {
                bindingContext.Dispose();
                bindingContext = null;
            }
        }
    }
}

