using System.Linq.Expressions;

namespace CFM.Framework.Binding.Paths
{
    public interface IPathParser
    {
        Path Parse(LambdaExpression expression);
    }
}

