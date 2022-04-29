using System.Linq.Expressions;
using System.Collections.Generic;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Paths
{
    public interface IExpressionPathFinder
    {
        List<Path> FindPaths(LambdaExpression expression);
    }
}

