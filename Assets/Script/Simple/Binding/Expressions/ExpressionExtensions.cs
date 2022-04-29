using System;
using System.Linq.Expressions;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Expressions
{
    public static class ExpressionExtensions
    {
        public static Func<object[], object> DynamicCompile(this LambdaExpression expr)
        {
            return (Func<object[], object>)((ConstantExpression)new EvalutingVisitor().Visit(expr)).Value;
        }

        public static Func<object[], object> DynamicCompile<T>(this Expression<T> expr)
        {
            return DynamicCompile((LambdaExpression)expr);
        }
    }
}

