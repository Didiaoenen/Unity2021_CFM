using System;
using System.Linq.Expressions;
using Assembly_CSharp.Assets.Script.Simple.Binding.Paths;
using Assembly_CSharp.Assets.Script.Simple.Binding.Contexts;
using Assembly_CSharp.Assets.Script.Simple.Binding.Converters;
using Assembly_CSharp.Assets.Script.Simple.Binding.Parameters;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources.Text;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources.Object;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources.Expressions;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Builder
{
    public class BindingBuilderBase : IBindingBuilder
    {
        private bool builded = false;

        private object scopeKey;

        private object target;

        private IBindingContext context;

        protected BindingDescription description;

        private IPathParser pathParser;

        private IConverterRegistry converterRegistry;

        public IPathParser PathParser { get; set; }

        public IConverterRegistry ConverterRegistry { get; set; }

        public BindingBuilderBase(IBindingContext context, object target)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            if (context.ToString() == null)
                throw new ArgumentNullException("context");

            this.context = context;
            this.target = target;

            description = new BindingDescription();
            description.Mode = BindingMode.Default;
        }

        protected void SetLiteral(object value)
        {
            if (description.Source != null)
                return;

            description.Source = new LiteralSourceDescription()
            {
                Literal = value
            };
        }

        protected void SetMode(BindingMode mode)
        {
            description.Mode = mode;
        }

        protected void SetScopeKey(object scopeKey)
        {
            this.scopeKey = scopeKey; 
        }

        protected void SetMemberPath(string pathText)
        {
            Path path = PathParser.Parse(pathText);
            SetMemberPath(path);
        }

        protected void SetMemberPath(Path path)
        {
            if (description.Source != null)
                return;

            if (path == null)
                return;

            if (path.IsStatic)
                return;

            description.Source = new ObjectSourceDescription()
            {
                Path = path
            };
        }

        protected void SetStaticMemberPath(string pathText)
        {
            Path path = PathParser.ParseStaticPath(pathText);
            SetStaticMemberPath(path);
        }

        protected void SetStaticMemberPath(Path path)
        {
            if (description.Source != null)
                return;

            if (path == null)
                return;

            if (!path.IsStatic)
                return;

            description.Source = new ObjectSourceDescription()
            {
                Path = path
            };
        }

        protected void SetExpression<TResult>(Expression<Func<TResult>> expression)
        {
            if (description.Source != null)
                return;

            description.Source = new ExpressionSourceDescription()
            {
                Expression = expression
            };
        }

        protected void SetExpression<T, TResult>(Expression<Func<T, TResult>> expression)
        {
            if (description.Source != null)
                return;

            description.Source = new ExpressionSourceDescription()
            {
                Expression = expression
            };
        }

        protected void SetExpression(LambdaExpression expression)
        {
            if (description.Source != null)
                return;

            description.Source = new ExpressionSourceDescription()
            {
                Expression = expression
            };
        }

        protected void SetCommandParameter(object parameter)
        {
            description.CommandParameter = parameter;
            description.Converter = new ParameterWrapConverter(description.CommandParameter);
        }

        protected void SetSourceDescription(SourceDescription source)
        {
            if (description.Source != null)
                return;

            description.Source = source;
        }

        public void SetDescription(BindingDescription bindingDescription)
        {
            description.Mode = bindingDescription.Mode;
            description.TargetName = bindingDescription.TargetName;
            description.UpdateTrigger = bindingDescription.UpdateTrigger;
            description.Converter = bindingDescription.Converter;
            description.Source = bindingDescription.Source;
        }

        protected IConverter ConverterByName(string name)
        {
            return ConverterRegistry.Find(name);
        }

        public void Build()
        {
            try
            {
                if (builded)
                    return;

                CheckBindingDescription();
                context.Add(target, description, scopeKey);
                builded = true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected void CheckBindingDescription()
        {
            if (string.IsNullOrEmpty(description.TargetName))
                return;

            if (description.Source == null)
                return;
        }
    }
}

