using System;
using System.Reflection;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Reflection
{
    public class ProxyMethodInfo : IProxyMethodInfo
    {
        protected bool isValueType = false;

        protected MethodInfo methodInfo;

        public virtual Type ReturnType { get { return methodInfo.ReturnType; } }

        public virtual ParameterInfo[] Parameters { get { return methodInfo.GetParameters(); } }

        public virtual ParameterInfo ReturnParameter { get { return methodInfo.ReturnParameter; } }

        public virtual Type DeclaringType { get { return methodInfo.DeclaringType; } }

        public virtual string Name { get { return methodInfo.Name; } }

        public virtual bool IsStatic { get { return methodInfo.IsStatic; } }

        public ProxyMethodInfo(MethodInfo methodInfo)
        {
            if (methodInfo == null)
                throw new ArgumentNullException(nameof(methodInfo));

            this.methodInfo = methodInfo;
            isValueType = methodInfo.DeclaringType.IsValueType;
        }

        public virtual object Invoke(object target, params object[] args)
        {
            return methodInfo.Invoke(target, args);
        }
    }

    public class ProxyFuncInfo<T, TResult> : ProxyMethodInfo, IProxyFuncInfo<T, TResult>
    {
        private Func<T, TResult> function;

        public override Type DeclaringType { get { return typeof(T); } }


        public ProxyFuncInfo(string methodName) : this(typeof(T).GetMethod(methodName), null)
        {
        }

        public ProxyFuncInfo(string methodName, Type[] parameterTypes) : this(typeof(T).GetMethod(methodName, parameterTypes), null)
        {
        }

        public ProxyFuncInfo(MethodInfo info) : this(info, null)
        {
        }

        public ProxyFuncInfo(string methodName, Func<T, TResult> function) : this(typeof(T).GetMethod(methodName), function)
        {
        }

        public ProxyFuncInfo(string methodName, Type[] parameterTypes, Func<T, TResult> function) : this(typeof(T).GetMethod(methodName, parameterTypes), function)
        {
        }

        public ProxyFuncInfo(MethodInfo info, Func<T, TResult> function) : base(info)
        {
            if (IsStatic)
                throw new ArgumentException("The method is static!");

            if (!typeof(TResult).Equals(methodInfo.ReturnType) || !methodInfo.DeclaringType.IsAssignableFrom(typeof(T)))
                throw new ArgumentException("The method types do not match!");

            this.function = function;
            if (this.function == null)
                this.function = MakeFunc(methodInfo);
        }

        private Func<T, TResult> MakeFunc(MethodInfo methodInfo)
        {
            try
            {
                if (isValueType)
                    return null;

                return methodInfo.CreateDelegate(typeof(Func<T, TResult>)) as Func<T, TResult>;
            }
            catch (Exception e)
            {
            }
            return null;
        }

        public virtual TResult Invoke(T target)
        {
            if (function != null)
                return function(target);

            return (TResult)methodInfo.Invoke(target, null);
        }

        public override object Invoke(object target, params object[] args)
        {
            return Invoke((T)target);
        }
    }

    public class ProxyFuncInfo<T, P1, TResult> : ProxyMethodInfo, IProxyFuncInfo<T, P1, TResult>
    {
        private Func<T, P1, TResult> function;

        public ProxyFuncInfo(string methodName) : this(typeof(T).GetMethod(methodName), null)
        {
        }

        public ProxyFuncInfo(string methodName, Type[] parameterTypes) : this(typeof(T).GetMethod(methodName, parameterTypes), null)
        {
        }

        public ProxyFuncInfo(MethodInfo info) : this(info, null)
        {
        }

        public ProxyFuncInfo(string methodName, Type[] parameterTypes, Func<T, P1, TResult> function) : this(typeof(T).GetMethod(methodName, parameterTypes), function)
        {
        }

        public ProxyFuncInfo(string methodName, Func<T, P1, TResult> function) : this(typeof(T).GetMethod(methodName), function)
        {
        }


        public ProxyFuncInfo(MethodInfo info, Func<T, P1, TResult> function) : base(info)
        {
            if (IsStatic)
                throw new ArgumentException("The method is static!");

            if (!typeof(TResult).Equals(methodInfo.ReturnType) || !methodInfo.DeclaringType.IsAssignableFrom(typeof(T)))
                throw new ArgumentException("The method types do not match!");

            ParameterInfo[] parameters = methodInfo.GetParameters();
            if (parameters.Length != 1 || !typeof(P1).Equals(parameters[0].ParameterType))
                throw new ArgumentException("The method types do not match!");

            this.function = function;
            if (this.function == null)
                this.function = MakeFunc(methodInfo);
        }

        public override Type DeclaringType { get { return typeof(T); } }

        private Func<T, P1, TResult> MakeFunc(MethodInfo methodInfo)
        {
            try
            {
                if (isValueType)
                    return null;

                return methodInfo.CreateDelegate(typeof(Func<T, P1, TResult>)) as Func<T, P1, TResult>;
            }
            catch (Exception e)
            {
            }
            return null;
        }

        public virtual TResult Invoke(T target, P1 p1)
        {
            if (function != null)
                return function(target, p1);

            return (TResult)methodInfo.Invoke(target, new object[] { p1 });
        }

        public override object Invoke(object target, params object[] args)
        {
            return Invoke((T)target, (P1)args[0]);
        }
    }

    public class ProxyFuncInfo<T, P1, P2, TResult> : ProxyMethodInfo, IProxyFuncInfo<T, P1, P2, TResult>
    {
        private Func<T, P1, P2, TResult> function;

        public ProxyFuncInfo(string methodName) : this(typeof(T).GetMethod(methodName), null)
        {
        }

        public ProxyFuncInfo(string methodName, Type[] parameterTypes) : this(typeof(T).GetMethod(methodName, parameterTypes), null)
        {
        }


        public ProxyFuncInfo(MethodInfo info) : this(info, null)
        {
        }

        public ProxyFuncInfo(string methodName, Func<T, P1, P2, TResult> function) : this(typeof(T).GetMethod(methodName), function)
        {
        }

        public ProxyFuncInfo(string methodName, Type[] parameterTypes, Func<T, P1, P2, TResult> function) : this(typeof(T).GetMethod(methodName, parameterTypes), function)
        {
        }

        public ProxyFuncInfo(MethodInfo info, Func<T, P1, P2, TResult> function) : base(info)
        {
            if (IsStatic)
                throw new ArgumentException("The method is static!");

            if (!typeof(TResult).Equals(methodInfo.ReturnType) || !methodInfo.DeclaringType.IsAssignableFrom(typeof(T)))
                throw new ArgumentException("The method types do not match!");

            ParameterInfo[] parameters = methodInfo.GetParameters();
            if (parameters.Length != 2 || !typeof(P1).Equals(parameters[0].ParameterType) || !typeof(P2).Equals(parameters[1].ParameterType))
                throw new ArgumentException("The method types do not match!");

            this.function = function;
            if (this.function == null)
                this.function = MakeFunc(methodInfo);
        }

        public override Type DeclaringType { get { return typeof(T); } }

        private Func<T, P1, P2, TResult> MakeFunc(MethodInfo methodInfo)
        {
            try
            {
                if (isValueType)
                    return null;

                return methodInfo.CreateDelegate(typeof(Func<T, P1, P2, TResult>)) as Func<T, P1, P2, TResult>;
            }
            catch (Exception e)
            {
            }
            return null;
        }

        public virtual TResult Invoke(T target, P1 p1, P2 p2)
        {
            if (function != null)
                return function(target, p1, p2);

            return (TResult)methodInfo.Invoke(target, new object[] { p1, p2 });
        }

        public override object Invoke(object target, params object[] args)
        {
            return Invoke((T)target, (P1)args[0], (P2)args[1]);
        }
    }

    public class ProxyFuncInfo<T, P1, P2, P3, TResult> : ProxyMethodInfo, IProxyFuncInfo<T, P1, P2, P3, TResult>
    {
        private Func<T, P1, P2, P3, TResult> function;

        public override Type DeclaringType { get { return typeof(T); } }


        public ProxyFuncInfo(string methodName) : this(typeof(T).GetMethod(methodName), null)
        {
        }

        public ProxyFuncInfo(string methodName, Type[] parameterTypes) : this(typeof(T).GetMethod(methodName, parameterTypes), null)
        {

        }
        public ProxyFuncInfo(MethodInfo info) : this(info, null)
        {
        }

        public ProxyFuncInfo(string methodName, Func<T, P1, P2, P3, TResult> function) : this(typeof(T).GetMethod(methodName), function)
        {
        }

        public ProxyFuncInfo(string methodName, Type[] parameterTypes, Func<T, P1, P2, P3, TResult> function) : this(typeof(T).GetMethod(methodName, parameterTypes), function)
        {
        }

        public ProxyFuncInfo(MethodInfo info, Func<T, P1, P2, P3, TResult> function) : base(info)
        {
            if (IsStatic)
                throw new ArgumentException("The method is static!");

            if (!typeof(TResult).Equals(methodInfo.ReturnType) || !methodInfo.DeclaringType.IsAssignableFrom(typeof(T)))
                throw new ArgumentException("The method types do not match!");

            ParameterInfo[] parameters = methodInfo.GetParameters();
            if (parameters.Length != 3 || !typeof(P1).Equals(parameters[0].ParameterType) || !typeof(P2).Equals(parameters[1].ParameterType) || !typeof(P3).Equals(parameters[2].ParameterType))
                throw new ArgumentException("The method types do not match!");

            this.function = function;
            if (this.function == null)
                this.function = MakeFunc(methodInfo);
        }

        private Func<T, P1, P2, P3, TResult> MakeFunc(MethodInfo methodInfo)
        {
            try
            {
                if (isValueType)
                    return null;

                return methodInfo.CreateDelegate(typeof(Func<T, P1, P2, P3, TResult>)) as Func<T, P1, P2, P3, TResult>;
            }
            catch (Exception e)
            {
            }
            return null;
        }

        public virtual TResult Invoke(T target, P1 p1, P2 p2, P3 p3)
        {
            if (function != null)
                return function(target, p1, p2, p3);

            return (TResult)methodInfo.Invoke(target, new object[] { p1, p2, p3 });
        }

        public override object Invoke(object target, params object[] args)
        {
            return Invoke((T)target, (P1)args[0], (P2)args[1], (P3)args[2]);
        }
    }

    public class ProxyActionInfo<T> : ProxyMethodInfo, IProxyActionInfo<T>
    {
        private Action<T> action;

        public override Type DeclaringType { get { return typeof(T); } }


        public ProxyActionInfo(string methodName) : this(typeof(T).GetMethod(methodName), null)
        {
        }

        public ProxyActionInfo(string methodName, Type[] parameterTypes) : this(typeof(T).GetMethod(methodName, parameterTypes), null)
        {
        }

        public ProxyActionInfo(MethodInfo info) : this(info, null)
        {
        }

        public ProxyActionInfo(string methodName, Action<T> action) : this(typeof(T).GetMethod(methodName), action)
        {
        }

        public ProxyActionInfo(string methodName, Type[] parameterTypes, Action<T> action) : this(typeof(T).GetMethod(methodName, parameterTypes), action)
        {
        }

        public ProxyActionInfo(MethodInfo info, Action<T> action) : base(info)
        {
            if (IsStatic)
                throw new ArgumentException("The method is static!");

            if (!typeof(void).Equals(methodInfo.ReturnType) || !methodInfo.DeclaringType.IsAssignableFrom(typeof(T)))
                throw new ArgumentException("The method types do not match!");

            this.action = action;
            if (this.action == null)
                this.action = MakeAction(methodInfo);
        }

        private Action<T> MakeAction(MethodInfo methodInfo)
        {
            try
            {
                if (isValueType)
                    return null;

                return methodInfo.CreateDelegate(typeof(Action<T>)) as Action<T>;
            }
            catch (Exception e)
            {
            }
            return null;
        }

        public virtual void Invoke(T target)
        {
            if (action != null)
            {
                action(target);
                return;
            }

            methodInfo.Invoke(target, null);
        }

        public override object Invoke(object target, params object[] args)
        {
            Invoke((T)target);
            return null;
        }
    }

    public class ProxyActionInfo<T, P1> : ProxyMethodInfo, IProxyActionInfo<T, P1>
    {
        private Action<T, P1> action;

        public override Type DeclaringType { get { return typeof(T); } }


        public ProxyActionInfo(string methodName) : this(typeof(T).GetMethod(methodName), null)
        {
        }

        public ProxyActionInfo(string methodName, Type[] parameterTypes) : this(typeof(T).GetMethod(methodName, parameterTypes), null)
        {
        }

        public ProxyActionInfo(MethodInfo info) : this(info, null)
        {
        }

        public ProxyActionInfo(string methodName, Action<T, P1> action) : this(typeof(T).GetMethod(methodName), action)
        {
        }

        public ProxyActionInfo(string methodName, Type[] parameterTypes, Action<T, P1> action) : this(typeof(T).GetMethod(methodName, parameterTypes), action)
        {
        }

        public ProxyActionInfo(MethodInfo info, Action<T, P1> action) : base(info)
        {
            if (IsStatic)
                throw new ArgumentException("The method is static!");

            if (!typeof(void).Equals(methodInfo.ReturnType) || !methodInfo.DeclaringType.IsAssignableFrom(typeof(T)))
                throw new ArgumentException("The method types do not match!");

            ParameterInfo[] parameters = methodInfo.GetParameters();
            if (parameters.Length != 1 || !typeof(P1).Equals(parameters[0].ParameterType))
                throw new ArgumentException("The method types do not match!");

            this.action = action;
            if (this.action == null)
                this.action = MakeAction(methodInfo);
        }

        private Action<T, P1> MakeAction(MethodInfo methodInfo)
        {
            try
            {
                if (isValueType)
                    return null;

                return methodInfo.CreateDelegate(typeof(Action<T, P1>)) as Action<T, P1>;
            }
            catch (Exception e)
            {
            }
            return null;
        }

        public virtual void Invoke(T target, P1 p1)
        {
            if (action != null)
            {
                action(target, p1);
                return;
            }

            methodInfo.Invoke(target, new object[] { p1 });
        }

        public override object Invoke(object target, params object[] args)
        {
            Invoke((T)target, (P1)args[0]);
            return null;
        }
    }

    public class ProxyActionInfo<T, P1, P2> : ProxyMethodInfo, IProxyActionInfo<T, P1, P2>
    {
        private Action<T, P1, P2> action;

        public override Type DeclaringType { get { return typeof(T); } }

        public ProxyActionInfo(string methodName) : this(typeof(T).GetMethod(methodName), null)
        {
        }

        public ProxyActionInfo(string methodName, Type[] parameterTypes) : this(typeof(T).GetMethod(methodName, parameterTypes), null)
        {
        }

        public ProxyActionInfo(MethodInfo info) : this(info, null)
        {
        }

        public ProxyActionInfo(string methodName, Action<T, P1, P2> action) : this(typeof(T).GetMethod(methodName), action)
        {
        }

        public ProxyActionInfo(string methodName, Type[] parameterTypes, Action<T, P1, P2> action) : this(typeof(T).GetMethod(methodName, parameterTypes), action)
        {
        }

        public ProxyActionInfo(MethodInfo info, Action<T, P1, P2> action) : base(info)
        {
            if (IsStatic)
                throw new ArgumentException("The method is static!");

            if (!typeof(void).Equals(methodInfo.ReturnType) || !methodInfo.DeclaringType.IsAssignableFrom(typeof(T)))
                throw new ArgumentException("The method types do not match!");

            ParameterInfo[] parameters = methodInfo.GetParameters();
            if (parameters.Length != 2 || !typeof(P1).Equals(parameters[0].ParameterType) || !typeof(P2).Equals(parameters[1].ParameterType))
                throw new ArgumentException("The method types do not match!");

            this.action = action;
            if (this.action == null)
                this.action = MakeAction(methodInfo);
        }

        private Action<T, P1, P2> MakeAction(MethodInfo methodInfo)
        {
            try
            {
                if (isValueType)
                    return null;

                return methodInfo.CreateDelegate(typeof(Action<T, P1, P2>)) as Action<T, P1, P2>;
            }
            catch (Exception e)
            {
            }
            return null;
        }

        public virtual void Invoke(T target, P1 p1, P2 p2)
        {
            if (action != null)
            {
                action(target, p1, p2);
                return;

            }

            methodInfo.Invoke(target, new object[] { p1, p2 });
        }

        public override object Invoke(object target, params object[] args)
        {
            Invoke((T)target, (P1)args[0], (P2)args[1]);
            return null;
        }
    }

    public class ProxyActionInfo<T, P1, P2, P3> : ProxyMethodInfo, IProxyActionInfo<T, P1, P2, P3>
    {
        private Action<T, P1, P2, P3> action;

        public override Type DeclaringType { get { return typeof(T); } }


        public ProxyActionInfo(string methodName) : this(typeof(T).GetMethod(methodName), null)
        {
        }

        public ProxyActionInfo(string methodName, Type[] parameterTypes) : this(typeof(T).GetMethod(methodName, parameterTypes), null)
        {
        }

        public ProxyActionInfo(MethodInfo info) : this(info, null)
        {
        }

        public ProxyActionInfo(string methodName, Action<T, P1, P2, P3> action) : this(typeof(T).GetMethod(methodName), action)
        {
        }
        public ProxyActionInfo(string methodName, Type[] parameterTypes, Action<T, P1, P2, P3> action) : this(typeof(T).GetMethod(methodName, parameterTypes), action)
        {
        }

        public ProxyActionInfo(MethodInfo info, Action<T, P1, P2, P3> action) : base(info)
        {
            if (IsStatic)
                throw new ArgumentException("The method is static!");

            if (!typeof(void).Equals(methodInfo.ReturnType) || !methodInfo.DeclaringType.IsAssignableFrom(typeof(T)))
                throw new ArgumentException("The method types do not match!");

            ParameterInfo[] parameters = methodInfo.GetParameters();
            if (parameters.Length != 3 || !typeof(P1).Equals(parameters[0].ParameterType) || !typeof(P2).Equals(parameters[1].ParameterType) || !typeof(P3).Equals(parameters[2].ParameterType))
                throw new ArgumentException("The method types do not match!");

            this.action = action;
            if (this.action == null)
                this.action = MakeAction(methodInfo);

        }

        private Action<T, P1, P2, P3> MakeAction(MethodInfo methodInfo)
        {
            try
            {
                if (isValueType)
                    return null;

                return methodInfo.CreateDelegate(typeof(Action<T, P1, P2, P3>)) as Action<T, P1, P2, P3>;
            }
            catch (Exception e)
            {
            }
            return null;
        }

        public virtual void Invoke(T target, P1 p1, P2 p2, P3 p3)
        {
            if (action != null)
            {
                action(target, p1, p2, p3);
                return;
            }

            methodInfo.Invoke(target, new object[] { p1, p2, p3 });
        }

        public override object Invoke(object target, params object[] args)
        {
            Invoke((T)target, (P1)args[0], (P2)args[1], (P3)args[2]);
            return null;
        }
    }
}

