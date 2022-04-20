using System;
using System.Collections.Generic;

using CFM.Log;
using CFM.Framework.Binding.Contexts;

namespace CFM.Framework.Binding.Builder
{
    public abstract class BindingSetBase : IBindingBuilder
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(BindingSetBase));

        protected IBindingContext context;

        protected readonly List<IBindingBuilder> builder = new List<IBindingBuilder>();

        protected BindingSetBase(IBindingContext context)
        {
            this.context = context;
        }

        public void Build()
        {
            foreach (var builder in builder)
            {
                try
                {
                    builder.Build();
                }
                catch (Exception e)
                {
                    if (log.IsErrorEnabled)
                        log.ErrorFormat("{0}", e);
                }
            }
            builder.Clear();
        }
    }

    public class BindingSet : BindingSetBase
    {
        private object target;

        public BindingSet(IBindingContext context, object target) : base(context)
        {
            this.target = target;
        }

        public virtual BindingBuilder Bind()
        {
            var builder = new BindingBuilder(context, target);
            this.builder.Add(builder);
            return builder;
        }

        public virtual BindingBuilder Bind(object target)
        {
            var builder = new BindingBuilder(context, target);
            this.builder.Add(builder);
            return builder;
        }
    }

    public class BindingSet<TTarget> : BindingSetBase where TTarget : class
    {
        private TTarget target;

        public BindingSet(IBindingContext context, TTarget target) : base(context)
        {
            this.target = target;
        }

        public virtual BindingBuilder<TTarget> Bind()
        {
            var builder = new BindingBuilder<TTarget>(context, target);
            this.builder.Add(builder);
            return builder;
        }

        public virtual BindingBuilder<TChildTarget> Bind<TChildTarget>(TChildTarget target) where TChildTarget : class
        {
            var builder = new BindingBuilder<TChildTarget>(context, target);
            this.builder.Add(builder);
            return builder;
        }
    }

    public class BindingSet<TTarget, TSource> : BindingSetBase where TTarget : class
    {
        private TTarget target;

        public BindingSet(IBindingContext context, TTarget target) : base(context)
        {
            this.target = target;
        }

        public virtual BindingBuilder<TTarget, TSource> Bind()
        {
            var builder = new BindingBuilder<TTarget, TSource>(context, target);
            this.builder.Add(builder);
            return builder;
        }

        public virtual BindingBuilder<TChildTarget, TSource> Bind<TChildTarget>(TChildTarget target) where TChildTarget : class
        {
            var builder = new BindingBuilder<TChildTarget, TSource>(context, target);
            this.builder.Add(builder);
            return builder;
        }
    }
}


