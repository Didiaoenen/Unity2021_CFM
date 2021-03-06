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
            BindEvent();
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

            if (command != null)
            {
                UnbindCommand(command);
                command = null;
            }

            if (invoker != null)
                invoker = null;

            if (handler != null)
                handler = null;

            if (value == null)
                return;

            var _command = value as ICommand;
            if (_command != null)
            {
                if (interactable == null)
                {
                    var interactablePropertyInfo = target.GetType().GetProperty("interactable");
                    if (interactablePropertyInfo != null)
                        interactable = interactablePropertyInfo.AsProxy();
                }

                command = _command;
                BindCommand(command);
                UpdateTargetInteractable();
                return;
            }

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

            var _iInvoker = value as IInvoker;
            if (_iInvoker != null)
            {
                invoker = _iInvoker;
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

