using System.Linq.Expressions;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Paths
{
    public interface IPathParser
    {
        Path Parse(LambdaExpression expression);

        Path Parse(string pathText);

        Path ParseStaticPath(LambdaExpression expression);

        Path ParseStaticPath(string pathText);

        string ParseMemberName(LambdaExpression expression);
    }
}

