using System.Collections.Generic;
using System.Linq.Expressions;
using CFM.Log;

namespace CFM.Framework.Binding.Paths
{
    public class ExpressionPathFinder : IExpressionPathFinder
    {
        public List<Path> FindPath(LambdaExpression expression)
        {
            PathExpressionVisitor visitor = new PathExpressionVisitor();
            visitor.Visit(expression);
            return visitor.Paths;
        }
    }
}

