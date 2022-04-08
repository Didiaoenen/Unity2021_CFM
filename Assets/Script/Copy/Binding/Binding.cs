using System;

using CFM.Log;
using CFM.Framework.Binding.Contexts;
using CFM.Framework.Binding.Converters;
using CFM.Framework.Binding.Proxy;
using CFM.Framework.Binding.Proxy.Sources;
using CFM.Framework.Binding.Proxy.Targets;

namespace CFM.Framework.Binding
{
    public class Binding : AbstractBinding
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Binding));

        private readonly ISourceProxyFactory sourceProxyFactory;

        private readonly ITargetProxyFactory targetProxyFactory;

        private bool disposed = false;

        private BindingMode bindingMode = BindingMode.Default;

        private BindingDescription bindingDescription;

        private ISourceProxy sourceProxy;

        private ITargetProxy targetProxy;

        private EventHandler sourceValueChangedHandler;

        private EventHandler targetValueChangedHandler;

        private IConverter converter;

        private object _lock = new object();

        private bool isUpdatingSource;

        private bool isUpdatingTarget;

        private string targetTypeName;

        public Binding(IBindingContext bindingContext, object source, object target, BindingDescription bindingDescription, ISourceProxyFactory sourceProxyFactory, ITargetProxyFactory targetProxyFactory): base(bindingContext ,source, target)
        {

        }

        protected virtual string GetViewName()
        {
            return null;
        }

        protected override void OnDataContextChanged()
        {
            throw new NotImplementedException();
        }

        protected BindingMode BindingMode
        {
            get
            {
                if (this.bindingMode != BindingMode.Default)
                    return this.bindingMode;

                this.bindingMode = this.bindingDescription.Mode;
                if (this.bindingMode == BindingMode.Default)
                    this.bindingMode = this.targetProxy.DefaultMode;

                if (this.bindingMode == BindingMode.Default & log.IsWarnEnabled)
                    log.WarnFormat("");

                return this.bindingMode;
            }
        }

        protected void UpdateDataOnBind()
        {
            try
            {
                if (this.UpdateTargetOnFirstBind(this.BindingMode) && this.sourceProxy != null)
                {
                    this.UpdateTargetFromSource();
                }

                if (this.UpdateSourceOnFirstBind(this.BindingMode) && this.targetProxy != null && this.targetProxy is IObtainable)
                {
                    this.UpdateSourceFormTarget();
                }
            }
            catch (Exception e)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("");
            }
        }

        protected void CreateSourceProxy(object source, SourceDescription description)
        {

        }

        protected void DisposeSourceProxy()
        {

        }

        protected void CreateTargetProxy(object target, BindingDescription description)
        {

        }

        protected void DisposeTargetProxy()
        {

        }

        protected virtual void UpdateTargetFromSource()
        {

        }

        protected virtual void UpdateSourceFormTarget()
        {

        }

        protected void SetTargetValue<T>(IModifiable modifiable, T value)
        {

        }

        protected void SetSourceValue<T>(IModifiable modifiable, T value)
        {

        }

        protected bool IsSubscribeSourceValueChanged(BindingMode bindingMode)
        {
            return false;
        }

        protected bool IsSubscribeTargetValueChanged(BindingMode bindingMode)
        {
            return true;
        }

        protected bool UpdateTargetOnFirstBind(BindingMode bindingMode)
        {
            return false;
        }

        protected bool UpdateSourceOnFirstBind(BindingMode bindingMode)
        {
            return false;
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                this.DisposeSourceProxy();
                this.DisposeTargetProxy();
                this.bindingDescription = null;
                disposed = true;
                base.Dispose(disposing);
            }
        }
    }
}

