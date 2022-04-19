using System;
using System.Linq.Expressions;

using CFM.Framework.Contexts;
using CFM.Framework.Binding.Paths;
using CFM.Framework.Binding.Contexts;
using CFM.Framework.Binding.Converters;
using CFM.Framework.Binding.Parameters;
using CFM.Framework.Binding.Proxy.Sources;
using CFM.Framework.Binding.Proxy.Sources.Text;
using CFM.Framework.Binding.Proxy.Sources.Object;
using CFM.Framework.Binding.Proxy.Sources.Expressions;

namespace CFM.Framework.Binding.Builder
{
    public class BindingBuilderBase: IBindingBuilder
    {
        private bool builded = false;

        private object scopeKey;

        private object target;

        private IBindingContext context;

        protected BindingDescription description;

        private IPathParser pathParser;

        private IConverterRegistry converterRegisery;

        protected IPathParser PathParser { get { return pathParser ?? (this.pathParser = ContextManager.GetApplicationContext().GetService<IPathParser>()); } }

        protected IConverterRegistry ConverterRegistry { get { return this.converterRegisery ?? (this.converterRegisery = ContextManager.GetApplicationContext().GetService<IConverterRegistry>()); } }

        public BindingBuilderBase(IBindingContext context, object target)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            if (context == null)
                throw new ArgumentNullException("context");

            this.context = context;
            this.target = target;

            description = new BindingDescription();
            description.Mode = BindingMode.Default;
        }

        protected  void SetLiteral(object value)
        {
            if (description.Source != null)
                throw new BindingException("You cannot set the source path of a Fluent binding more than once");

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
                throw new BindingException("You cannot set the source path of a Fluent binding more than once");

            if (path == null)
                throw new ArgumentException("the path is null.");

            if (path.IsStatic)
                throw new ArgumentException("Need a non-static path in here.");

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
                throw new BindingException("You cannot set the source path of a Fluent binding more than once");

            if (path == null)
                throw new ArgumentException("the path is null.");

            if (!path.IsStatic)
                throw new ArgumentException("Need a static path in here.");

            description.Source = new ObjectSourceDescription()
            {
                Path = path
            };
        }

        protected void SetExpression<TResult>(Expression<Func<TResult>> expression)
        {
            if (description.Source != null)
                throw new BindingException("You cannot set the source path of a Fluent binding more than once");

            description.Source = new ExpressionSourceDescription()
            {
                Expression = expression
            };
        }

        protected void SetExpression<T, TResult>(Expression<Func<T, TResult>> expression)
        {
            if (description.Source != null)
                throw new BindingException("You cannot set the source path of a Fluent binding more than once");

            description.Source = new ExpressionSourceDescription()
            {
                Expression = expression
            };
        }

        protected void SetExpression(LambdaExpression expression)
        {
            if (description.Source != null)
                throw new BindingException("You cannot set the source path of a Fluent binding more than once");

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
                throw new BindingException("You cannot set the source path of a Fluent binding more than once");
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

        protected void CheckBindingDescription()
        {
            if (string.IsNullOrEmpty(description.TargetName))
                throw new BindingException("TargetName is null!");

            if (description.Source == null)
                throw new BindingException("Source description is null!");
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
            catch (BindingException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new BindingException(e, "An exception occurred while building the data binding for {0}.", description.ToString());
            }
        }
    }
}

