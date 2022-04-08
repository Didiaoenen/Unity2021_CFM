using System;
using System.Collections.Generic;

using CFM.Framework.Binding.Contexts;

using CFM.Log;

namespace CFM.Framework.Binding.Builder
{
    public abstract class BindingSetBase: IBindingBuilder
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
            foreach (var builder in this.builder)
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
            this.builder.Clear();
        }
    }

    public class BindingSet<TTarget, TSource>: BindingSetBase where TTarget: class
    {
        private TTarget target;

        public BindingSet(IBindingContext context, TTarget target): base(context)
        {
            this.target = target;
        }

        public virtual BindingBuilder<TTarget, TSource> Bind()
        {
            var builder = new BindingBuilder<TTarget, TSource>(this.context, this.target);
            this.builder.Add(builder);
            return builder;
        }
    }

    public class BindingSet<TTarget>: BindingSetBase where TTarget: class
    {
        private TTarget target;

        public BindingSet(IBindingContext context, TTarget target): base(context)
        {

        }

        public virtual BindingBuilder<TTarget> Bind()
        {
            var builder = new BindingBuilder<TTarget>(this.context, this.target);
            this.builder.Add(builder);
            return builder;
        }

        public virtual BindingBuilder<TChildTarget> Bind<TChildTarget>(TChildTarget target) where TChildTarget: class
        {
            var builder = new BindingBuilder<TChildTarget>(this.context, target);
            this.builder.Add(builder);
            return builder;
        }
    }

    public class BindingSet: BindingSetBase
    {
        private object target;

        public BindingSet(IBindingContext context, object target): base(context)
        {
            this.target = target;
        }

        public virtual BindingBuilder Bind()
        {
            var builder = new BindingBuilder(this.context, this.target);
            this.builder.Add(builder);
            return builder;
        }

        public virtual BindingBuilder Bind(object target)
        {
            var builder = new BindingBuilder(this.context, target);
            this.builder.Add(builder);
            return builder;
        }
    }
}


