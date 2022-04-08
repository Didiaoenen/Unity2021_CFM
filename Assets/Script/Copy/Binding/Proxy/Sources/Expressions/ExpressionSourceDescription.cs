using System;
using System.Linq.Expressions;

namespace CFM.Framework.Binding.Proxy.Sources.Expressions
{
    public class ExpressionSourceDescription : SourceDescription
    {
        private LambdaExpression expression;

        private Type returnType;

        public ExpressionSourceDescription()
        {

        }

        public LambdaExpression Expression
        {
            get { return this.expression; }
        }

        public Type ReturnType { get { return this.returnType; } }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}

