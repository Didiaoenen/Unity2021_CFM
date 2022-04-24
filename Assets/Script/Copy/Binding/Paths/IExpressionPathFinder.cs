using System.Linq.Expressions;
using System.Collections.Generic;

namespace CFM.Framework.Binding.Paths
{
    public interface IExpressionPathFinder
    {
        List<Path> FindPaths(LambdaExpression expression);
    }
}

