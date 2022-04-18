using System;
using System.Reflection;
using System.Runtime.CompilerServices;

using CFM.Log;

namespace CFM.Framework
{
    public static class WeakDelegateExtensions
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static Action AsWeak(this Action action)
        {
            if (!IsCanWeaken(action))
                return action;

            Type type = action.Target.GetType();
            WeakReference targetRef = new WeakReference(action.Target);

            MethodInfo method = action.Method;

            return () =>
            {
                object target = targetRef.Target;
                if (target == null)
                {
                    if (log.IsWarnEnabled)
                        log.WarnFormat("You are trying to invoke a weak reference delegate({0}.{1}), and the target object has been destroyed.", type, method);
                    return;
                }

                method.Invoke(target, null);
            };
        }

        public static Action<T> AsWeak<T>(this Action<T> action)
        {
            if (!IsCanWeaken(action))
                return action;

            Type type = action.Target.GetType();
            WeakReference targetRef = new WeakReference(action.Target);
            MethodInfo method = action.Method;
            return (t) =>
            {
                object target = targetRef.Target;
                if (target == null)
                {
                    if (log.IsWarnEnabled)
                        log.WarnFormat("You are trying to invoke a weak reference delegate({0}.{1}), and the target object has been destroyed.", type, method);
                    return;
                }

                method.Invoke(target, new object[] { t });
            };
        }

        public static Action<T1, T2> AsWeak<T1, T2>(this Action<T1, T2> action)
        {
            if (!IsCanWeaken(action))
                return action;

            Type type = action.Target.GetType();
            WeakReference targetRef = new WeakReference(action.Target);
            MethodInfo method = action.Method;
            return (t1, t2) =>
            {
                object target = targetRef.Target;
                if (target == null)
                {
                    if (log.IsWarnEnabled)
                        log.WarnFormat("You are trying to invoke a weak reference delegate({0}.{1}), and the target object has been destroyed.", type, method);
                    return;
                }

                method.Invoke(target, new object[] { t1, t2 });
            };
        }

        public static Action<T1, T2, T3> AsWeak<T1, T2, T3>(this Action<T1, T2, T3> action)
        {
            if (!IsCanWeaken(action))
                return action;

            Type type = action.Target.GetType();
            WeakReference targetRef = new WeakReference(action.Target);
            MethodInfo method = action.Method;
            return (t1, t2, t3) =>
            {
                object target = targetRef.Target;
                if (target == null)
                {
                    if (log.IsWarnEnabled)
                        log.WarnFormat("You are trying to invoke a weak reference delegate({0}.{1}), and the target object has been destroyed.", type, method);
                    return;
                }

                method.Invoke(target, new object[] { t1, t2, t3 });
            };
        }

        public static Action<T1, T2, T3, T4> AsWeak<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action)
        {
            if (!IsCanWeaken(action))
                return action;

            Type type = action.Target.GetType();
            WeakReference targetRef = new WeakReference(action.Target);
            MethodInfo method = action.Method;
            return (t1, t2, t3, t4) =>
            {
                object target = targetRef.Target;
                if (target == null)
                {
                    if (log.IsWarnEnabled)
                        log.WarnFormat("You are trying to invoke a weak reference delegate({0}.{1}), and the target object has been destroyed.", type, method);
                    return;
                }

                method.Invoke(target, new object[] { t1, t2, t3, t4 });
            };
        }

        public static Func<TResult> AsWeak<TResult>(this Func<TResult> func)
        {
            if (!IsCanWeaken(func))
                return func;

            Type type = func.Target.GetType();
            WeakReference targetRef = new WeakReference(func.Target);
            MethodInfo method = func.Method;
            return () =>
            {
                object target = targetRef.Target;
                if (target == null)
                {
                    if (log.IsWarnEnabled)
                        log.WarnFormat("You are trying to invoke a weak reference delegate({0}.{1}), and the target object has been destroyed.", type, method);

                    throw new Exception(string.Format("You are trying to invoke a weak reference delegate({0}.{1}), and the target object has been destroyed.", type, method));
                }

                return (TResult)method.Invoke(target, null);
            };
        }

        public static Func<T, TResult> AsWeak<T, TResult>(this Func<T, TResult> func)
        {
            if (!IsCanWeaken(func))
                return func;

            Type type = func.Target.GetType();
            WeakReference targetRef = new WeakReference(func.Target);
            MethodInfo method = func.Method;
            return (t) =>
            {
                object target = targetRef.Target;
                if (target == null)
                {
                    if (log.IsWarnEnabled)
                        log.WarnFormat("You are trying to invoke a weak reference delegate({0}.{1}), and the target object has been destroyed.", type, method);

                    throw new Exception(string.Format("You are trying to invoke a weak reference delegate({0}.{1}), and the target object has been destroyed.", type, method));
                }

                return (TResult)method.Invoke(target, new object[] { t });
            };
        }

        public static Func<T1, T2, TResult> AsWeak<T1, T2, TResult>(this Func<T1, T2, TResult> func)
        {
            if (!IsCanWeaken(func))
                return func;

            Type type = func.Target.GetType();
            WeakReference targetRef = new WeakReference(func.Target);
            MethodInfo method = func.Method;
            return (t1, t2) =>
            {
                object target = targetRef.Target;
                if (target == null)
                {
                    if (log.IsWarnEnabled)
                        log.WarnFormat("You are trying to invoke a weak reference delegate({0}.{1}), and the target object has been destroyed.", type, method);

                    throw new Exception(string.Format("You are trying to invoke a weak reference delegate({0}.{1}), and the target object has been destroyed.", type, method));
                }

                return (TResult)method.Invoke(target, new object[] { t1, t2 });
            };
        }

        public static Func<T1, T2, T3, TResult> AsWeak<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func)
        {
            if (!IsCanWeaken(func))
                return func;

            Type type = func.Target.GetType();
            WeakReference targetRef = new WeakReference(func.Target);
            MethodInfo method = func.Method;
            return (t1, t2, t3) =>
            {
                object target = targetRef.Target;
                if (target == null)
                {
                    if (log.IsWarnEnabled)
                        log.WarnFormat("You are trying to invoke a weak reference delegate({0}.{1}), and the target object has been destroyed.", type, method);

                    throw new Exception(string.Format("You are trying to invoke a weak reference delegate({0}.{1}), and the target object has been destroyed.", type, method));
                }

                return (TResult)method.Invoke(target, new object[] { t1, t2, t3 });
            };
        }

        public static Func<T1, T2, T3, T4, TResult> AsWeak<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> func)
        {
            if (!IsCanWeaken(func))
                return func;

            Type type = func.Target.GetType();
            WeakReference targetRef = new WeakReference(func.Target);
            MethodInfo method = func.Method;
            return (t1, t2, t3, t4) =>
            {
                object target = targetRef.Target;
                if (target == null)
                {
                    if (log.IsWarnEnabled)
                        log.WarnFormat("You are trying to invoke a weak reference delegate({0}.{1}), and the target object has been destroyed.", type, method);

                    throw new Exception(string.Format("You are trying to invoke a weak reference delegate({0}.{1}), and the target object has been destroyed.", type, method));
                }

                return (TResult)method.Invoke(target, new object[] { t1, t2, t3, t4 });
            };
        }

        private static bool IsCanWeaken(Delegate del)
        {
            if (del == null || del.Method.IsStatic || del.Target == null || IsClosure(del))
                return false;
            return true;
        }

        private static bool IsClosure(Delegate del)
        {
            if (del == null || del.Method.IsStatic || del.Target == null)
                return false;

            var type = del.Target.GetType();
            var isInvisible = !type.IsVisible;
            var isCompolerGenerated = type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Length != 0;
            var isNested = type.IsNested && type.MemberType == MemberTypes.NestedType;
            return isNested && isCompolerGenerated && isInvisible;
        }
    }
}

