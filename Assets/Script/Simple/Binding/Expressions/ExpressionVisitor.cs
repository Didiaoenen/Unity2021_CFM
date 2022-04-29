using System.Linq.Expressions;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Expressions
{
    public class ExpressionVisitor
    {
        public virtual Expression Visit(Expression expr)
        {
            return null;
        }
    }
}

