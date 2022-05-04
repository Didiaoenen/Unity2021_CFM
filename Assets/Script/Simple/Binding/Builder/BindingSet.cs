using System;
using System.Collections.Generic;
using Assembly_CSharp.Assets.Script.Simple.Binding.Contexts;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Builder
{
    public class BindingSetBase : IBindingBuilder
    {
        protected IBindingContext context;

        protected List<IBindingBuilder> builders = new List<IBindingBuilder>();

        public BindingSetBase(IBindingContext context)
        {
            this.context = context;
        }

        public virtual void Build()
        {
            foreach (var builder in builders)
            {
                try
                {
                    builder.Build();
                }
                catch (Exception e)
                {

                }
            }
            builders.Clear();
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
            builders.Add(builder);
            return builder;
        }

        public virtual BindingBuilder Bind(object target)
        {
            var builder = new BindingBuilder(context, target);
            builders.Add(builder);
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
            builders.Add(builder);
            return builder;
        }

        public virtual BindingBuilder<TChildTarget> Bind<TChildTarget>(TChildTarget target) where TChildTarget : class
        {
            var builder = new BindingBuilder<TChildTarget>(context, target);
            builders.Add(builder);
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
            builders.Add(builder);
            return builder;
        }

        public virtual BindingBuilder<TChildTarget, TSource> Bind<TChildTarget>(TChildTarget target) where TChildTarget : class
        {
            var builder = new BindingBuilder<TChildTarget, TSource>(context, target);
            builders.Add(builder);
            return builder;
        }
    }
}

