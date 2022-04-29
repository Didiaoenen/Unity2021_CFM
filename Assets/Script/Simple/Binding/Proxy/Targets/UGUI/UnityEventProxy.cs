using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine.Events;
using Assembly_CSharp.Assets.Script.Simple.Execution;
using Assembly_CSharp.Assets.Script.Simple.Commands;
using Assembly_CSharp.Assets.Script.Simple.Binding.Reflection;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Targets.UGUI
{
    public abstract class UnityEventProxyBase<T> : EventTargetProxyBase where T : UnityEventBase
    {
        private bool disposed = false;

        protected ICommand command;

        protected IInvoker invoker;

        protected Delegate handler;

        protected IProxyPropertyInfo interactable;

        protected T unityEvent;

        public override BindingMode DefaultMode { get { return BindingMode.OneWay; } }

        protected UnityEventProxyBase(object target, T unityEvent) : base(target)
        {
            if (unityEvent == null)
                throw new ArgumentNullException("unityEvent");

            this.unityEvent = unityEvent;
            this.BindEvent();
        }

        protected abstract void BindEvent();

        protected abstract void UnbindEvent();

        protected abstract bool IsValid(Delegate handler);

        protected abstract bool IsValid(IProxyInvoker invoker);

        public override void SetValue(object value)
        {
            var target = Target;
            if (target == null)
                return;

            if (this.command != null)
            {
                UnbindCommand(this.command);
                this.command = null;
            }

            if (this.invoker != null)
                this.invoker = null;

            if (this.handler != null)
                this.handler = null;

            if (value == null)
                return;

            ICommand command = value as ICommand;
            if (command != null)
            {
                if (interactable == null)
                {
                    var interactablePropertyInfo = target.GetType().GetProperty("interactable");
                    if (interactablePropertyInfo != null)
                        interactable = interactablePropertyInfo.AsProxy();
                }

                this.command = command;
                BindCommand(this.command);
                UpdateTargetInteractable();
                return;
            }

            IProxyInvoker proxyInvoker = value as IProxyInvoker;
            if (proxyInvoker != null)
            {
                if (IsValid(proxyInvoker))
                {
                    this.invoker = proxyInvoker;
                    return;
                }

                throw new ArgumentException("Bind method failed.the parameter types do not match.");
            }

            Delegate handler = value as Delegate;
            if (handler != null)
            {
                if (IsValid(handler))
                {
                    this.handler = handler;
                    return;
                }

                throw new ArgumentException("Bind method failed.the parameter types do not match.");
            }

            IInvoker invoker = value as IInvoker;
            if (invoker != null)
            {
                this.invoker = invoker;
            }
        }

        public override void SetValue<TValue>(TValue value)
        {
            SetValue((object)value);
        }

        protected virtual void OnCanExecuteChanged(object sender, EventArgs e)
        {
            Executors.RunOnMainThread(UpdateTargetInteractable);
        }

        protected virtual void UpdateTargetInteractable()
        {
            var target = Target;
            if (interactable == null || target == null)
                return;

            bool value = command == null ? false : command.CanExecute(null);
            if (interactable is IProxyPropertyInfo<bool>)
            {
                (interactable as IProxyPropertyInfo<bool>).SetValue(target, value);
                return;
            }

            interactable.SetValue(target, value);
        }

        protected virtual void BindCommand(ICommand command)
        {
            if (command == null)
                return;

            command.CanExecuteChanged += OnCanExecuteChanged;
        }

        protected virtual void UnbindCommand(ICommand command)
        {
            if (command == null)
                return;

            command.CanExecuteChanged -= OnCanExecuteChanged;
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                UnbindCommand(command);
                UnbindEvent();
                disposed = true;
                base.Dispose(disposing);
            }
        }
    }

    public class UnityEventProxy : UnityEventProxyBase<UnityEvent>
    {
        public override Type Type { get { return typeof(UnityEvent); } }

        public UnityEventProxy(object target, UnityEvent unityEvent) : base(target, unityEvent)
        {
        }

        protected override void BindEvent()
        {
            unityEvent.AddListener(OnEvent);
        }

        protected override void UnbindEvent()
        {
            unityEvent.RemoveListener(OnEvent);
        }

        protected override bool IsValid(Delegate handler)
        {
            if (handler is UnityAction)
                return true;

            MethodInfo info = handler.Method;
            if (!info.ReturnType.Equals(typeof(void)))
                return false;

            List<Type> parameterTypes = info.GetParameterTypes();
            if (parameterTypes.Count != 0)
                return false;
            return true;
        }

        protected override bool IsValid(IProxyInvoker invoker)
        {
            IProxyMethodInfo info = invoker.ProxyMethodInfo;
            if (!info.ReturnType.Equals(typeof(void)))
                return false;

            var parameters = info.Parameters;
            if (parameters != null && parameters.Length != 0)
                return false;
            return true;
        }

        protected virtual void OnEvent()
        {
            try
            {
                if (command != null)
                {
                    command.Execute(null);
                    return;
                }

                if (invoker != null)
                {
                    invoker.Invoke();
                    return;
                }

                if (handler != null)
                {
                    if (handler is UnityAction)
                        (handler as UnityAction)();
                    else
                    {
                        handler.DynamicInvoke();
                    }
                    return;
                }
            }
            catch (Exception e)
            {
            }
        }
    }

    public class UnityEventProxy<T> : UnityEventProxyBase<UnityEvent<T>>
    {
        public override Type Type { get { return typeof(UnityEvent<T>); } }

        public UnityEventProxy(object target, UnityEvent<T> unityEvent) : base(target, unityEvent)
        {
        }

        protected override void BindEvent()
        {
            unityEvent.AddListener(OnEvent);
        }

        protected override void UnbindEvent()
        {
            unityEvent.RemoveListener(OnEvent);
        }

        protected override bool IsValid(Delegate handler)
        {
            if (handler is UnityAction<T>)
                return true;

            MethodInfo info = handler.Method;
            if (!info.ReturnType.Equals(typeof(void)))
                return false;

            List<Type> parameterTypes = info.GetParameterTypes();
            if (parameterTypes.Count != 1)
                return false;

            return parameterTypes[0].IsAssignableFrom(typeof(T));
        }

        protected override bool IsValid(IProxyInvoker invoker)
        {
            IProxyMethodInfo info = invoker.ProxyMethodInfo;
            if (!info.ReturnType.Equals(typeof(void)))
                return false;

            var parameters = info.Parameters;
            if (parameters == null || parameters.Length != 1)
                return false;

            return parameters[0].ParameterType.IsAssignableFrom(typeof(T));
        }

        protected virtual void OnEvent(T parameter)
        {
            try
            {
                if (command != null)
                {
                    command.Execute(parameter);
                    return;
                }

                if (invoker != null)
                {
                    invoker.Invoke(parameter);
                    return;
                }

                if (handler != null)
                {
                    if (handler is UnityAction<T>)
                        (handler as UnityAction<T>)(parameter);
                    else
                    {
                        handler.DynamicInvoke(parameter);
                    }
                    return;
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}

