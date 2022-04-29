using System.Linq.Expressions;
using System.Collections.Generic;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Paths
{
    public class ExpressionPathFinder : IExpressionPathFinder
    {
        public List<Path> FindPaths(LambdaExpression expression)
        {
            PathExpressionVisitor visitor = new PathExpressionVisitor();
            visitor.Visit(expression);
            return visitor.Paths;
        }
    }
}

