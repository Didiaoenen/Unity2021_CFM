using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CFM.Framework.Utilities
{
    public interface IExecute
    {
        object Execute(params object[] parameters);
    }

    public abstract class WeakBase<TDelegate> : IExecute where TDelegate : class
    {
        private readonly bool isStatic;

        private int hashCode = 0;

        protected TDelegate del;

        protected WeakReference targetReference;

        protected MethodInfo targetMethod;

        public WeakBase(TDelegate del) : this(null, del)
        {
        }

        public WeakBase(object target, TDelegate del)
        {
            hashCode = del.GetHashCode();
            var dd = del as Delegate;

            isStatic = dd.Method.IsStatic;
            if (isStatic || (target != null && !target.Equals(dd.Target)) || this.IsClosure(dd))
            {
                this.del = del;
                if (target != null)
                    targetReference = new WeakReference(target);
            }
            else
            {
                targetMethod = dd.Method;
                targetReference = new WeakReference(dd.Target);
            }
        }

        protected bool IsStatic { get { return isStatic; } }

        public bool IsAlive
        {
            get
            {
                if (del != null)
                {
                    if (targetReference != null && !targetReference.IsAlive)
                    {
                        targetReference = null;
                        del = null;
                        return false;
                    }
                    return true;
                }

                if (targetReference != null)
                    return targetReference.IsAlive;

                return false;
            }
        }

        protected bool IsClosure(Delegate del)
        {
            if (del == null || del.Method.IsStatic || del.Target == null)
                return false;

            var type = del.Target.GetType();
            var isInvisible = !type.IsVisible;
            var isCompilerGenerated = type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Length != 0;
            var isNested = type.IsNested && type.MemberType == MemberTypes.NestedType;
            return isNested && isCompilerGenerated && isInvisible;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;

            if (obj == null || !(obj is WeakBase<TDelegate>))
                return false;

            WeakBase<TDelegate> other = (WeakBase<TDelegate>)obj;
            if (isStatic != other.isStatic)
                return false;

            if (del != null)
            {
                if ((targetReference == null && other.targetReference == null) || (targetReference != null && other.targetReference != null && targetReference.Target == other.targetReference.Target))
                    return del.Equals(other.del);

                return false;
            }

            return targetMethod.Equals(other.targetMethod) && targetReference.Target == other.targetReference.Target;
        }

        public override int GetHashCode()
        {
            return hashCode;
        }

        public abstract object Execute(params object[] parameters);
    }
}

