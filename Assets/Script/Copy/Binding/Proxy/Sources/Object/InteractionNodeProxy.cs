using System;
using System.Reflection;
using System.Collections.Generic;

using CFM.Framework.Interactivity;
using CFM.Framework.Binding.Reflection;

using CFM.Log;

namespace CFM.Framework.Binding.Proxy.Sources.Object
{
    public class InteractionNodeProxy : SourceProxyBase, IModifiable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(InteractionNodeProxy));

        private readonly IInteractionRequest request;

        private bool disposed = false;

        protected IProxyInvoker invoker;/* Method Binding */

        protected Delegate handler;/* Delegate Binding */

        protected IScriptInvoker scriptInvoker;/* Script Function Binding  */

        public InteractionNodeProxy(IInteractionRequest request) : this(null, request)
        {
        }

        public InteractionNodeProxy(object source, IInteractionRequest request) : base(source)
        {
            this.request = request;
            BindEvent();
        }

        public override Type Type { get { return typeof(EventHandler<InteractionEventArgs>); } }

        public virtual void SetValue<TValue>(TValue value)
        {
            SetValue((object)value);
        }

        public virtual void SetValue(object value)
        {
            if (value != null && !(value is IProxyInvoker || value is Delegate || value is IScriptInvoker))
                throw new ArgumentException("Binding object to InteractionRequest failed, unsupported object type", "value");

            if (this.invoker != null)
                this.invoker = null;

            if (this.handler != null)
                this.handler = null;

            if (this.scriptInvoker != null)
                this.scriptInvoker = null;

            if (value == null)
                return;

            //Bind Method
            IProxyInvoker invoker = value as IProxyInvoker;
            if (invoker != null)
            {
                if (IsValid(invoker))
                {
                    this.invoker = invoker;
                    return;
                }

                throw new ArgumentException("Binding the IProxyInvoker to InteractionRequest failed, mismatched parameter type.");
            }

            //Bind Delegate
            Delegate handler = value as Delegate;
            if (handler != null)
            {
                if (IsValid(handler))
                {
                    this.handler = handler;
                    return;
                }

                throw new ArgumentException("Binding the Delegate to InteractionRequest failed, mismatched parameter type.");
            }

            //Bind Script Function
            IScriptInvoker scriptInvoker = value as IScriptInvoker;
            if (scriptInvoker != null)
            {
                this.scriptInvoker = scriptInvoker;
            }
        }

        protected virtual bool IsValid(Delegate handler)
        {
            if (handler is EventHandler<InteractionEventArgs>)
                return true;

            MethodInfo info = handler.Method;
            if (!info.ReturnType.Equals(typeof(void)))
                return false;

            List<Type> parameterTypes = info.GetParameterTypes();
            if (parameterTypes.Count != 2)
                return false;

            return parameterTypes[0].IsAssignableFrom(typeof(object)) && parameterTypes[1].IsAssignableFrom(typeof(InteractionEventArgs));
        }

        protected virtual bool IsValid(IProxyInvoker invoker)
        {
            IProxyMethodInfo info = invoker.ProxyMethodInfo;
            if (!info.ReturnType.Equals(typeof(void)))
                return false;

            ParameterInfo[] parameters = info.Parameters;
            if (parameters == null || parameters.Length != 2)
                return false;

            return parameters[0].ParameterType.IsAssignableFrom(typeof(object)) && parameters[1].ParameterType.IsAssignableFrom(typeof(InteractionEventArgs));
        }

        protected virtual void BindEvent()
        {
            if (request != null)
                request.Raised += OnRaised;
        }

        protected virtual void UnbindEvent()
        {
            if (request != null)
                request.Raised -= OnRaised;
        }

        protected virtual void OnRaised(object sender, InteractionEventArgs args)
        {
            try
            {
                if (invoker != null)
                {
                    invoker.Invoke(sender, args);
                    return;
                }

                if (handler != null)
                {
                    if (handler is EventHandler<InteractionEventArgs>)
                        (handler as EventHandler<InteractionEventArgs>)(sender, args);
                    else
                    {
                        handler.DynamicInvoke(sender, args);
                    }
                    return;
                }

                if (scriptInvoker != null)
                {
                    scriptInvoker.Invoke(sender, args);
                    return;
                }
            }
            catch (Exception e)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("{0}", e);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                UnbindEvent();
                handler = null;
                scriptInvoker = null;
                invoker = null;
                disposed = true;
                base.Dispose(disposing);
            }
        }
    }
}
