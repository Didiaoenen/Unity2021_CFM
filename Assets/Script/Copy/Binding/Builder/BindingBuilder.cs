using System;
using System.Linq.Expressions;

#if UNITY_2019_1_OR_NEWER
using UnityEngine.UIElements;
#endif

using CFM.Framework.Interactivity;
using CFM.Framework.Binding.Contexts;
using CFM.Framework.Binding.Converters;

namespace CFM.Framework.Binding.Builder
{
    public class BindingBuilder<TTarget, TSource> : BindingBuilderBase where TTarget : class
    {
        public BindingBuilder(IBindingContext context, TTarget target) : base(context, target)
        {

        }

        public BindingBuilder<TTarget, TSource> For(string targetName)
        {
            description.TargetName = targetName;
            description.UpdateTrigger = null;
            return this;
        }

        public BindingBuilder<TTarget, TSource> For(string targetName, string updateTrigger)
        {
            description.TargetName = targetName;
            description.UpdateTrigger = updateTrigger;
            return this;
        }

        public BindingBuilder<TTarget, TSource> For<TResult>(Expression<Func<TTarget, TResult>> memberExpression)
        {
            string targetName = PathParser.ParseMemberName(memberExpression);
            description.TargetName = targetName;
            description.UpdateTrigger = null;
            return this;
        }

        public BindingBuilder<TTarget, TSource> For<TResult>(Expression<Func<TTarget, TResult>> memberExpression, string updateTrigger)
        {
            string targetName = PathParser.ParseMemberName(memberExpression);
            description.TargetName = targetName;
            description.UpdateTrigger = updateTrigger;
            return this;
        }

        public BindingBuilder<TTarget, TSource> For<TResult, TEvent>(Expression<Func<TTarget, TResult>> memberExpression, Expression<Func<TTarget, TEvent>> updateTriggerExpression)
        {
            string targetName = PathParser.ParseMemberName(memberExpression);
            string updateTrigger = PathParser.ParseMemberName(updateTriggerExpression);
            description.TargetName = targetName;
            description.UpdateTrigger = updateTrigger;
            return this;
        }

        public BindingBuilder<TTarget, TSource> For(Expression<Func<TTarget, EventHandler<InteractionEventArgs>>> memberExpression)
        {
            string targetName = PathParser.ParseMemberName(memberExpression);
            description.TargetName = targetName;
            description.UpdateTrigger = null;
            OneWayToSource();
            return this;
        }

#if UNITY_2019_1_OR_NEWER
        public BindingBuilder<TTarget, TSource> For<TResult>(Expression<Func<TTarget, TResult>> memberExpression, Expression<Func<TTarget, Func<EventCallback<ChangeEvent<TResult>>, bool>>> updateTriggerExpression)
        {
            string targetName = PathParser.ParseMemberName(memberExpression);
            string updateTrigger = PathParser.ParseMemberName(updateTriggerExpression);
            description.TargetName = targetName;
            description.UpdateTrigger = updateTrigger;
            return this;
        }

        public BindingBuilder<TTarget, TSource> For<TResult>(Expression<Func<TTarget, Func<EventCallback<ChangeEvent<TResult>>, bool>>> memberExpression)
        {
            string targetName = PathParser.ParseMemberName(memberExpression);
            description.TargetName = targetName;
            description.UpdateTrigger = null;
            OneWayToSource();
            return this;
        }
#endif

        public BindingBuilder<TTarget, TSource> To(string path)
        {
            SetMemberPath(path);
            return this;
        }

        public BindingBuilder<TTarget, TSource> To<TResult>(Expression<Func<TSource, TResult>> path)
        {
            SetMemberPath(PathParser.Parse(path));
            return this;
        }

        public BindingBuilder<TTarget, TSource> To<TParameter>(Expression<Func<TSource, Action<TParameter>>> path)
        {
            SetMemberPath(PathParser.Parse(path));
            return this;
        }

        public BindingBuilder<TTarget, TSource> To(Expression<Func<TSource, Action>> path)
        {
            SetMemberPath(PathParser.Parse(path));
            return this;
        }

        public BindingBuilder<TTarget, TSource> ToExpression<TResult>(Expression<Func<TSource, TResult>> expression)
        {
            SetExpression(expression);
            OneWay();
            return this;
        }

        public BindingBuilder<TTarget, TSource> TwoWay()
        {
            SetMode(BindingMode.TwoWay);
            return this;
        }

        public BindingBuilder<TTarget, TSource> OneWay()
        {
            SetMode(BindingMode.OneWay);
            return this;
        }

        public BindingBuilder<TTarget, TSource> OneWayToSource()
        {
            SetMode(BindingMode.OneWayToSource);
            return this;
        }

        public BindingBuilder<TTarget, TSource> OneTime()
        {
            SetMode(BindingMode.OneTime);
            return this;
        }

        public BindingBuilder<TTarget, TSource> CommandParameter(object parameter)
        {
            SetCommandParameter(parameter);
            return this;
        }

        public BindingBuilder<TTarget, TSource> WithConversion(string converterName)
        {
            var converter = ConverterByName(converterName);
            return WithConversion(converter);
        }

        public BindingBuilder<TTarget, TSource> WithConversion(IConverter converter)
        {
            description.Converter = converter;
            return this;
        }

        public BindingBuilder<TTarget, TSource> WithScopeKey(object scopeKey)
        {
            SetScopeKey(scopeKey);
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
            description.TargetName = targetPropertyName;
            description.UpdateTrigger = null;
            return this;
        }

        public BindingBuilder<TTarget> For(string targetPropertyName, string updateTrigger)
        {
            description.TargetName = targetPropertyName;
            description.UpdateTrigger = updateTrigger;
            return this;
        }

        public BindingBuilder<TTarget> For<TResult>(Expression<Func<TTarget, TResult>> memberExpression)
        {
            string targetName = PathParser.ParseMemberName(memberExpression);
            description.TargetName = targetName;
            description.UpdateTrigger = null;
            return this;
        }

        public BindingBuilder<TTarget> For<TResult>(Expression<Func<TTarget, TResult>> memberExpression, string updateTrigger)
        {
            string targetName = PathParser.ParseMemberName(memberExpression);
            description.TargetName = targetName;
            description.UpdateTrigger = updateTrigger;
            return this;
        }

        public BindingBuilder<TTarget> For<TResult, TEvent>(Expression<Func<TTarget, TResult>> memberExpression, Expression<Func<TTarget, TResult>> updateTriggerExpression)
        {
            string targetName = PathParser.ParseMemberName(memberExpression);
            string updateTrigger = PathParser.ParseMemberName(updateTriggerExpression);
            description.TargetName = targetName;
            description.UpdateTrigger = updateTrigger;
            return this;
        }

        public BindingBuilder<TTarget> For(Expression<Func<TTarget, EventHandler<InteractionEventArgs>>> memberExpression)
        {
            string targetName = PathParser.ParseMemberName(memberExpression);
            description.TargetName = targetName;
            description.UpdateTrigger = null;
            OneWayToSource();
            return this;
        }

#if UNITY_2019_1_OR_NEWER
        public BindingBuilder<TTarget> For<TResult>(Expression<Func<TTarget, TResult>> memberExpression, Expression<Func<TTarget, Func<EventCallback<ChangeEvent<TResult>>, bool>>> updateTriggerExpression)
        {
            string targetName = PathParser.ParseMemberName(memberExpression);
            string updateTrigger = PathParser.ParseMemberName(updateTriggerExpression);
            description.TargetName = targetName;
            description.UpdateTrigger = updateTrigger;
            return this;
        }

        public BindingBuilder<TTarget> For<TResult>(Expression<Func<TTarget, Func<EventCallback<ChangeEvent<TResult>>, bool>>> memberExpression)
        {
            string targetName = PathParser.ParseMemberName(memberExpression);
            description.TargetName = targetName;
            description.UpdateTrigger = null;
            OneWayToSource();
            return this;
        }
#endif

        public BindingBuilder<TTarget> To(string path)
        {
            SetStaticMemberPath(path);
            OneWay();
            return this;
        }

        public BindingBuilder<TTarget> To<TResult>(Expression<Func<TResult>> path)
        {
            SetStaticMemberPath(PathParser.ParseStaticPath(path));
            OneWay();
            return this;
        }

        public BindingBuilder<TTarget> To<TParameter>(Expression<Func<Action<TParameter>>> path)
        {
            SetStaticMemberPath(PathParser.ParseStaticPath(path));
            return this;
        }

        public BindingBuilder<TTarget> To(Expression<Func<Action>> path)
        {
            SetStaticMemberPath(PathParser.ParseStaticPath(path));
            return this;
        }

        public BindingBuilder<TTarget> ToValue(object value)
        {
            SetLiteral(value);
            return this;
        }

        public BindingBuilder<TTarget> ToExpression<TResult>(Expression<Func<TResult>> expression)
        {
            SetExpression(expression);
            OneWay();
            return this;
        }

        public BindingBuilder<TTarget> TwoWay()
        {
            SetMode(BindingMode.TwoWay);
            return this;
        }

        public BindingBuilder<TTarget> OneWay()
        {
            SetMode(BindingMode.OneWay);
            return this;
        }

        public BindingBuilder<TTarget> OneWayToSource()
        {
            SetMode(BindingMode.OneWayToSource);
            return this;
        }

        public BindingBuilder<TTarget> OneTime()
        {
            SetMode(BindingMode.OneTime);
            return this;
        }

        public BindingBuilder<TTarget> CommandParameter(object parameter)
        {
            SetCommandParameter(parameter);
            return this;
        }

        public BindingBuilder<TTarget> WithConversion(string converterName)
        {
            var converter = ConverterByName(converterName);
            return WithConversion(converter);
        }

        public BindingBuilder<TTarget> WithConversion(IConverter converter)
        {
            description.Converter = converter;
            return this;
        }

        public BindingBuilder<TTarget> WidthScopeKey(object scopeKey)
        {
            SetScopeKey(scopeKey);
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
            description.TargetName = targetName;
            description.UpdateTrigger = updateTrigger;
            return this;
        }

        public BindingBuilder To(string path)
        {
            SetMemberPath(path);
            return this;
        }

        public BindingBuilder ToStatic(string path)
        {
            SetStaticMemberPath(path);
            return this;
        }

        public BindingBuilder ToValue(string value)
        {
            SetLiteral(value);
            return this;
        }

        public BindingBuilder TwoWay()
        {
            SetMode(BindingMode.TwoWay);
            return this;
        }

        public BindingBuilder OneWay()
        {
            SetMode(BindingMode.OneWay);
            return this;
        }

        public BindingBuilder OneWayToSource()
        {
            SetMode(BindingMode.OneWayToSource);
            return this;
        }

        public BindingBuilder OneTime()
        {
            SetMode(BindingMode.OneTime);
            return this;
        }

        public BindingBuilder CommandParameter(object parameter)
        {
            SetCommandParameter(parameter);
            return this;
        }

        public BindingBuilder WithConversion(string converterName)
        {
            var converter = this.ConverterByName(converterName);
            return this;
        }

        public BindingBuilder WithConversion(IConverter converter)
        {
            description.Converter = converter;
            return this;
        }

        public BindingBuilder WithScopeKey(object scopeKey)
        {
            SetScopeKey(scopeKey);
            return this;
        }
    }
}
