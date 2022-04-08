using System.Linq.Expressions;
using System.Reflection;

namespace CFM.Framework.Binding.Paths
{
    public class PathParser : IPathParser
    {
        public Path Parse(LambdaExpression expression)
        {
            throw new System.NotImplementedException();
        }

        public virtual Path Parse(string pathText)
        {
            return null;
        }

        private MethodInfo GetDelegateMothodInfo(MethodCallExpression expression)
        {
            return null;
        }

        private void Parse(Expression expression, Path path)
        {

        }

        private static Expression ConvertMemberAccessToConstant(Expression argument)
        {
            return null;
        }

        public virtual Path ParseStaticPath(string pathText)
        {
            return null;
        }

        protected string ParserTypeName(string pathText)
        {
            return null;
        }

        protected string ParserMemberName(string pathText)
        {
            return null;
        }

        public virtual string ParseMemberName(LambdaExpression expression)
        {
            return null;
        }
    }
}

