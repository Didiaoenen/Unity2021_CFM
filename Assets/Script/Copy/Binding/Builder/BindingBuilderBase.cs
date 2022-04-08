using System;
using System.Linq.Expressions;

using CFM.Framework.Contexts;
using CFM.Framework.Binding.Paths;
using CFM.Framework.Binding.Contexts;
using CFM.Framework.Binding.Converters;
using CFM.Framework.Binding.Proxy.Sources;

namespace CFM.Framework.Binding.Builder
{
    public class BindingBuilderBase: IBindingBuilder
    {
        private bool builder = false;

        private object acopeKey;

        private object target;

        private IBindingContext context;

        protected BindingDescription description;

        private IPathParser pathParser;

        private IConverterRegistry converterRegisery;

        protected IPathParser PathParser { get { return pathParser ?? (this.pathParser = ContextManager.GetApplicationContext().GetService<IPathParser>()); } }

        protected IConverterRegistry ConverterRegistry { get { return this.converterRegisery ?? (this.converterRegisery = ContextManager.GetApplicationContext().GetService<IConverterRegistry>()); } }

        public BindingBuilderBase(IBindingContext context, object target)
        {

        }

        protected  void SetLiterral(object value)
        {

        }

        protected void SetMode(BindingMode mode)
        {

        }

        protected void SetScopeKey(object scopeKey)
        {

        }

        protected void SetMemberPath(string pathText)
        {

        }

        protected void SetMemberPath(Path path)
        {

        }

        protected void SetStaticMemberPath(string pathText)
        {

        }

        protected void SetExpression<TResult>(Expression<Func<TResult>> expression)
        {

        }

        protected void SetExpression<T, TResult>(Expression<Func<T, TResult>> expression)
        {

        }

        protected void SetExpression(LambdaExpression expression)
        {

        }

        protected void SetCommandParameter(object parameter)
        {

        }

        protected void SetSourceDescription(SourceDescription source)
        {

        }

        public void SetDescription(BindingDescription bindingDescription)
        {

        }

        protected IConverter ConverterByName(string name)
        {
            return null;
        }

        protected void CheckBindingDescription()
        {

        }

        public void Build()
        {
            throw new System.NotImplementedException();
        }
    }
}

