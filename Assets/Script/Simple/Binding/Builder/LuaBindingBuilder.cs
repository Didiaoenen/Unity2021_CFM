using XLua;
using System;
using Assembly_CSharp.Assets.Script.Simple.Binding.Contexts;
using Assembly_CSharp.Assets.Script.Simple.Binding.Converters;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources.Expressions;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Builder
{
    public class LuaBindingBuilder : BindingBuilderBase
    {
        public LuaBindingBuilder(IBindingContext context, object target) : base(context, target)
        {

        }

        public LuaBindingBuilder For(string targetName, string updateTrigger = null)
        {
            description.TargetName = targetName;
            description.UpdateTrigger = updateTrigger;
            return this;
        }

        public LuaBindingBuilder To(string path)
        {
            SetMemberPath(path);
            return this;
        }

        public LuaBindingBuilder ToExpression(LuaFunction expression, params string[] paths)
        {
            if (description.Source != null)
                throw new Exception();

            description.Source = new LuaExpressionSourceDescription()
            {
                Expression = expression,
                Paths = paths
            };

            return this;
        }

        public LuaBindingBuilder ToStatic(string path)
        {
            SetStaticMemberPath(path);
            return this;
        }

        public LuaBindingBuilder ToValue(object value)
        {
            SetLiteral(value);
            return this;
        }

        public LuaBindingBuilder OneWay()
        {
            SetMode(BindingMode.OneWay);
            return this;
        }

        public LuaBindingBuilder TwoWay()
        {
            SetMode(BindingMode.TwoWay);
            return this;
        }

        public LuaBindingBuilder OneWayToSource()
        {
            SetMode(BindingMode.OneWayToSource);
            return this;
        }

        public LuaBindingBuilder OneTime()
        {
            SetMode(BindingMode.OneTime);
            return this;
        }

        public LuaBindingBuilder CommandParameter(object parameter)
        {
            SetCommandParameter(parameter);
            return this;
        }

        public LuaBindingBuilder WithConversion(string converterName)
        {
            var converter = ConverterByName(converterName);
            return WithConversion(converter);
        }

        public LuaBindingBuilder WithConversion(IConverter converter)
        {
            description.Converter = converter;
            return this;
        }

        public LuaBindingBuilder WithScopeKey(object scopeKey)
        {
            SetScopeKey(scopeKey);
            return this;
        }
    }
}

