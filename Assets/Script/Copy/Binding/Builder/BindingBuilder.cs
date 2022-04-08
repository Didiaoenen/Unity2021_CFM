using System;
using System.Linq.Expressions;

using CFM.Log;
using CFM.Framework.Binding.Contexts;
using CFM.Framework.Binding.Converters;
using CFM.Framework.Interactivity;
#if UNITY_2019_1_OR_NEWER
using UnityEngine.UIElements;
#endif

namespace CFM.Framework.Binding.Builder
{
    public class BindingBuilder<TTarget, TSource>: BindingBuilderBase where TTarget: class
    {
        public BindingBuilder(IBindingContext context, TTarget target) : base(context, target)
        {

        }

        public BindingBuilder<TTarget, TSource> For(string targetName)
        {
            return this;
        }

        public BindingBuilder<TTarget, TSource> For(string targetName, string updateTrigger)
        {
            return this;
        }

        public BindingBuilder<TTarget, TSource> For<TResult>(Expression<Func<TTarget, TResult>> memberExpression)
        {
            return this;
        }

        public BindingBuilder<TTarget, TSource> For<TResult>(Expression<Func<TTarget, TResult>> memberExpression, string updateTrigger)
        {
            return this;
        }

        public BindingBuilder<TTarget, TSource> For<TResult, TEvent>(Expression<Func<TTarget, TResult>> memberExpression, Expression<Func<TTarget, TEvent>> updateTriggerExpression)
        {
            return this;
        }

        public BindingBuilder<TTarget, TSource> For(Expression<Func<TTarget, EventHandler<InteractionEventArgs>>> memberExpression)
        {
            return this;
        }

#if UNITY_2019_1_OR_NEWER
        public BindingBuilder<TTarget, TSource> For<TResult>(Expression<Func<TTarget, TResult>> memberExpression, Expression<Func<TTarget, Func<EventCallback<ChangeEvent<TResult>>, bool>>> updateTriggerExpression)
        {
            return this;
        }

        public BindingBuilder<TTarget, TSource> For<TResult>(Expression<Func<TTarget, Func<EventCallback<ChangeEvent<TResult>>, bool>>> memberExpression)
        {
            return this;
        }
#endif

        public BindingBuilder<TTarget, TSource> To(string path)
        {
            return this;
        }

        public BindingBuilder<TTarget, TSource> To<TResult>(Expression<Func<TSource, TResult>> path)
        {
            return this;
        }

        public BindingBuilder<TTarget, TSource> To<TParameter>(Expression<Func<TSource, Action<TParameter>>> path)
        {
            return this;
        }

        public BindingBuilder<TTarget, TSource> To(Expression<Func<TSource, Action>> path)
        {
            return this;
        }

        public BindingBuilder<TTarget, TSource> ToExpression<TResult>(Expression<Func<TSource, TResult>> expression)
        {
            return this;
        }

        public BindingBuilder<TTarget, TSource> TwoWay()
        {
            return this;
        }

        public BindingBuilder<TTarget, TSource> OneWay()
        {
            return this;
        }

        public BindingBuilder<TTarget, TSource> OneWayToSource()
        {
            return this;
        }

        public BindingBuilder<TTarget, TSource> OneTime()
        {
            return this;
        }

        public BindingBuilder<TTarget, TSource> CommandParameter(object parameter)
        {
            return this;
        }

        public BindingBuilder<TTarget, TSource> WithConversion(string converterName)
        {
            return this;
        }

        public BindingBuilder<TTarget, TSource> WithConversion(IConverter converter)
        {
            return this;
        }

        public BindingBuilder<TTarget, TSource> WithScopeKey(object scopeKey)
        {
            return this;
        }
    }

    public class BindingBuilder<TTarget> : BindingBuilderBase where TTarget : class
    {
        public BindingBuilder(IBindingContext context, object target) : base(context, target)
        {
        }

        public BindingBuilder<TTarget> For(string targetPropertyName)
        {
            return this;
        }

        public BindingBuilder<TTarget> For(string targetPropertyName, string updateTrigger)
        {
            return this;
        }

        public BindingBuilder<TTarget> For<TResult>(Expression<Func<TTarget, TResult>> memberExpression)
        {
            return this;
        }

        public BindingBuilder<TTarget> For<TResult>(Expression<Func<TTarget, TResult>> memberExpression, string updateTrigger)
        {
            return this;
        }

        public BindingBuilder<TTarget> For<TResult, TEvent>(Expression<Func<TTarget, TResult>> memberExpression, Expression<Func<TTarget, TResult>> updateTriggerExpression)
        {
            return this;
        }

        public BindingBuilder<TTarget> For(Expression<Func<TTarget, EventHandler<InteractionEventArgs>>> memberExpression)
        {
            return this;
        }

#if UNITY_2019_1_OR_NEWER
        public BindingBuilder<TTarget> For<TResult>(Expression<Func<TTarget, TResult>> memberExpression, Expression<Func<TTarget, Func<EventCallback<ChangeEvent<TResult>>, bool>>> updateTriggerExpression)
        {
            return this;
        }

        public BindingBuilder<TTarget> For<TResult>(Expression<Func<TTarget, Func<EventCallback<ChangeEvent<TResult>>, bool>>> memberExpression)
        {
            return this;
        }
#endif

        public BindingBuilder<TTarget> To(string path)
        {
            return this;
        }

        public BindingBuilder<TTarget> To<TResult>(Expression<Func<TResult>> path)
        {
            return this;
        }

        public BindingBuilder<TTarget> To<TParameter>(Expression<Func<Action<TParameter>>> path)
        {
            return this;
        }

        public BindingBuilder<TTarget> To(Expression<Func<Action>> path)
        {
            return this;
        }

        public BindingBuilder<TTarget> ToValue(object value)
        {
            return this;
        }

        public BindingBuilder<TTarget> ToExpression<TResult>(Expression<Func<TResult>> expression)
        {
            return this;
        }

        public BindingBuilder<TTarget> TwoWay()
        {
            return this;
        }

        public BindingBuilder<TTarget> OneWay()
        {
            return this;
        }

        public BindingBuilder<TTarget> OneWayToSource()
        {
            return this;
        }

        public BindingBuilder<TTarget> OneTime()
        {
            return this;
        }

        public BindingBuilder<TTarget> WidthConversion(string converterName)
        {
            return this;
        }

        public BindingBuilder<TTarget> WidthConversion(IConverter converter)
        {
            return this;
        }

        public BindingBuilder<TTarget> WidthScopeKey(object scopeKey)
        {
            return this;
        }
    }

    public class BindingBuilder : BindingBuilderBase
    {
        public BindingBuilder(IBindingContext context, object target) : base(context, target)
        {
        }

        public BindingBuilder For(string targetName, string updateTrigger = null)
        {
            return this;
        }

        public BindingBuilder To(string path)
        {
            return this;
        }

        public BindingBuilder ToStatic(string path)
        {
            return this;
        }

        public BindingBuilder ToValue(string value)
        {
            return this;
        }

        public BindingBuilder TwoWay()
        {
            return this;
        }

        public BindingBuilder OneWay()
        {
            return this;
        }

        public BindingBuilder OneWayToSource()
        {
            return this;
        }

        public BindingBuilder OneTime()
        {
            return this;
        }

        public BindingBuilder CommandParameter(object parameter)
        {
            return this;
        }

        public BindingBuilder WithConversion(string converterName)
        {
            return this;
        }

        public BindingBuilder WithConversion(IConverter converter)
        {
            return this;
        }

        public BindingBuilder WithScopeKey(object scopeKey)
        {
            return this;
        }
    }
}

