using CFM.Framework.Binding.Proxy.Targets.Universal;
using CFM.Framework.Binding.Reflection;
using CFM.Framework.Commands;
using CFM.Framework.Execution;
using CFM.Log;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Events;

namespace CFM.Framework.Binding.Proxy.Targets.UGUI
{
    public abstract class UnityEventProxyBase<T> : EventTargetProxyBase where T : UnityEventBase
    {
        private bool disposed = false;

        protected ICommand command;/* Command Binding */

        protected IInvoker invoker;/* Method Binding or Lua Function Binding */

        protected Delegate handler;/* Delegate Binding */

        protected IProxyPropertyInfo interactable;

        protected T unityEvent;

        public UnityEventProxyBase(object target, T unityEvent) : base(target)
        {
            if (unityEvent == null)
                throw new ArgumentNullException("unityEvent");

            this.unityEvent = unityEvent;
            BindEvent();
        }

        public override BindingMode DefaultMode { get { return BindingMode.OneWay; } }

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

            //Bind Command
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

            //Bind Method
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

            //Bind Delegate
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

            //Bind Script Function
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
    }

    public class UnityEventProxy : UnityEventProxyBase<UnityEvent>
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UnityEventProxy));

        public UnityEventProxy(object target, UnityEvent unityEvent) : base(target, unityEvent)
        {
        }

        public override Type Type { get { return typeof(UnityEvent); } }

        protected override void BindEvent()
        {
            this.unityEvent.AddListener(OnEvent);
        }

        protected override void UnbindEvent()
        {
            this.unityEvent.RemoveListener(OnEvent);
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
                if (log.IsErrorEnabled)
                    log.ErrorFormat("{0}", e);
            }
        }
    }

    public class UnityEventProxy<T> : UnityEventProxyBase<UnityEvent<T>>
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UnityEventProxy<T>));

        public UnityEventProxy(object target, UnityEvent<T> unityEvent) : base(target, unityEvent)
        {
        }

        public override Type Type { get { return typeof(UnityEvent<T>); } }

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
                if (log.IsErrorEnabled)
                    log.ErrorFormat("{0}", e);
            }
        }
    }

    public class UnityEventProxy<T0, T1> : UnityEventProxyBase<UnityEvent<T0, T1>>
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UnityEventProxy<T0, T1>));

        public UnityEventProxy(object target, UnityEvent<T0, T1> unityEvent) : base(target, unityEvent)
        {
        }

        public override Type Type { get { return typeof(UnityEvent<T0, T1>); } }

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
            if (handler is UnityAction<T0, T1>)
                return true;
            MethodInfo info = handler.Method;
            if (!info.ReturnType.Equals(typeof(void)))
                return false;

            List<Type> parameterTypes = info.GetParameterTypes();
            if (parameterTypes.Count != 2)
                return false;

            return parameterTypes[0].IsAssignableFrom(typeof(T0))
                && parameterTypes[1].IsAssignableFrom(typeof(T1));
        }

        protected override bool IsValid(IProxyInvoker invoker)
        {
            IProxyMethodInfo info = invoker.ProxyMethodInfo;
            if (!info.ReturnType.Equals(typeof(void)))
                return false;

            var parameters = info.Parameters;
            if (parameters == null || parameters.Length != 2)
                return false;

            return parameters[0].ParameterType.IsAssignableFrom(typeof(T0))
                && parameters[1].ParameterType.IsAssignableFrom(typeof(T1));
        }

        protected virtual void OnEvent(T0 t0, T1 t1)
        {
            try
            {
                if (command != null)
                {
                    command.Execute(new object[] { t0, t1 });
                    return;
                }

                if (invoker != null)
                {
                    invoker.Invoke(t0, t1);
                    return;
                }

                if (handler != null)
                {
                    if (handler is UnityAction<T0, T1>)
                        (handler as UnityAction<T0, T1>)(t0, t1);
                    else
                    {
                        handler.DynamicInvoke(t0, t1);
                    }
                    return;
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.ErrorFormat("{0}", e);
            }
        }
    }

    public class UnityEventProxy<T0, T1, T2> : UnityEventProxyBase<UnityEvent<T0, T1, T2>>
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UnityEventProxy<T0, T1, T2>));

        public UnityEventProxy(object target, UnityEvent<T0, T1, T2> unityEvent) : base(target, unityEvent)
        {
        }

        public override Type Type { get { return typeof(UnityEvent<T0, T1, T2>); } }

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
            if (handler is UnityAction<T0, T1, T2>)
                return true;
            MethodInfo info = handler.Method;
            if (!info.ReturnType.Equals(typeof(void)))
                return false;

            List<Type> parameterTypes = info.GetParameterTypes();
            if (parameterTypes.Count != 3)
                return false;

            return parameterTypes[0].IsAssignableFrom(typeof(T0))
                && parameterTypes[1].IsAssignableFrom(typeof(T1))
                && parameterTypes[2].IsAssignableFrom(typeof(T2));

        }

        protected override bool IsValid(IProxyInvoker invoker)
        {
            IProxyMethodInfo info = invoker.ProxyMethodInfo;
            if (!info.ReturnType.Equals(typeof(void)))
                return false;

            var parameters = info.Parameters;
            if (parameters == null || parameters.Length != 3)
                return false;

            return parameters[0].ParameterType.IsAssignableFrom(typeof(T0))
                && parameters[1].ParameterType.IsAssignableFrom(typeof(T1))
                && parameters[2].ParameterType.IsAssignableFrom(typeof(T2));
        }

        protected virtual void OnEvent(T0 t0, T1 t1, T2 t2)
        {
            try
            {
                if (command != null)
                {
                    command.Execute(new object[] { t0, t1, t2 });
                    return;
                }

                if (invoker != null)
                {
                    invoker.Invoke(t0, t1, t2);
                    return;
                }

                if (handler != null)
                {
                    if (handler is UnityAction<T0, T1, T2>)
                        (handler as UnityAction<T0, T1, T2>)(t0, t1, t2);
                    else
                    {
                        handler.DynamicInvoke(t0, t1, t2);
                    }
                    return;
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.ErrorFormat("{0}", e);
            }
        }
    }

    public class UnityEventProxy<T0, T1, T2, T3> : UnityEventProxyBase<UnityEvent<T0, T1, T2, T3>>
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UnityEventProxy<T0, T1, T2, T3>));

        public UnityEventProxy(object target, UnityEvent<T0, T1, T2, T3> unityEvent) : base(target, unityEvent)
        {
        }

        public override Type Type { get { return typeof(UnityEvent<T0, T1, T2, T3>); } }

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
            if (handler is UnityAction<T0, T1, T2, T3>)
                return true;
            MethodInfo info = handler.Method;
            if (!info.ReturnType.Equals(typeof(void)))
                return false;

            List<Type> parameterTypes = info.GetParameterTypes();
            if (parameterTypes.Count != 4)
                return false;

            return parameterTypes[0].IsAssignableFrom(typeof(T0))
                && parameterTypes[1].IsAssignableFrom(typeof(T1))
                && parameterTypes[2].IsAssignableFrom(typeof(T2))
                && parameterTypes[3].IsAssignableFrom(typeof(T3));

        }

        protected override bool IsValid(IProxyInvoker invoker)
        {
            IProxyMethodInfo info = invoker.ProxyMethodInfo;
            if (!info.ReturnType.Equals(typeof(void)))
                return false;

            var parameters = info.Parameters;
            if (parameters == null || parameters.Length != 4)
                return false;

            return parameters[0].ParameterType.IsAssignableFrom(typeof(T0))
                && parameters[1].ParameterType.IsAssignableFrom(typeof(T1))
                && parameters[2].ParameterType.IsAssignableFrom(typeof(T2))
                && parameters[3].ParameterType.IsAssignableFrom(typeof(T3));
        }

        protected virtual void OnEvent(T0 t0, T1 t1, T2 t2, T3 t3)
        {
            try
            {
                if (command != null)
                {
                    command.Execute(new object[] { t0, t1, t2, t3 });
                    return;
                }

                if (invoker != null)
                {
                    invoker.Invoke(t0, t1, t2, t3);
                    return;
                }

                if (handler != null)
                {
                    if (handler is UnityAction<T0, T1, T2, T3>)
                        (handler as UnityAction<T0, T1, T2, T3>)(t0, t1, t2, t3);
                    else
                    {
                        handler.DynamicInvoke(t0, t1, t2, t3);
                    }
                    return;
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.ErrorFormat("{0}", e);
            }
        }
    }
}