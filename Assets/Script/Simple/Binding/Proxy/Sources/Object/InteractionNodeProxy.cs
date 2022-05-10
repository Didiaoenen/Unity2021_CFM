using System;
using System.Reflection;
using System.Collections.Generic;
using Assembly_CSharp.Assets.Script.Simple.Interactivity;
using Assembly_CSharp.Assets.Script.Simple.Binding.Reflection;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources.Object
{
    public class InteractionNodeProxy : SourceProxyBase, IModifiable
    {
        private readonly IInteractionRequest request;

        private bool disposed = false;

        protected IProxyInvoker invoker;

        protected Delegate handler;

        protected IScriptInvoker scriptInvoker;

        public override Type Type { get { return typeof(EventHandler<InteractionEventArgs>); } }

        public InteractionNodeProxy(IInteractionRequest request) : this(null, request)
        {

        }

        public InteractionNodeProxy(object source, IInteractionRequest request) : base(source)
        {
            this.request = request;
            BindEvent();
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
                        handler.DynamicInvoke(sender, args);
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

            }
        }

        public virtual void SetValue(object value)
        {
            if (value != null && !(value is IProxyInvoker || value is Delegate || value is IScriptInvoker))
                throw new ArgumentException();

            if (invoker != null)
                invoker = null;

            if (handler != null)
                handler = null;

            if (scriptInvoker != null)
                scriptInvoker = null;

            if (value == null)
                return;

            var _invoker = value as IProxyInvoker;
            if (_invoker != null)
            {
                if (IsValid(_invoker))
                {
                    invoker = _invoker;
                    return;
                }

                throw new ArgumentException();
            }

            var _handler = value as Delegate;
            if (_handler != null)
            {
                if (IsValid(_handler))
                {
                    handler = _handler;
                    return;
                }

                throw new ArgumentException();
            }

            var _scriptInvoker = value as IScriptInvoker;
            if (_scriptInvoker != null)
            {
                scriptInvoker = _scriptInvoker;
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

        public virtual void SetValue<TValue>(TValue value)
        {
            SetValue(value as object);
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

