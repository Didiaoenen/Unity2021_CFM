using System.Collections.Generic;
using System.Linq.Expressions;

namespace CFM.Framework.Binding.Paths
{
    public class PathExpressionVisitor
    {
        private readonly List<Path> list = new List<Path>();

        public List<Path> Paths { get { return list; } }

        public virtual Expression Visit(Expression expression)
        {
            return null;
        }

        protected virtual void Visit(IList<Expression> nodes)
        {

        }

        protected virtual Expression VisitLambda(LambdaExpression node)
        {
            return Visit(node.Body);
        }

        protected virtual Expression VisitBinary(BinaryExpression node)
        {
            return null;
        }

        protected virtual Expression VisitConditional(ConditionalExpression node)
        {
            return null;
        }

        protected virtual Expression VisitConatant(ConstantExpression node)
        {
            return null;
        }

        protected virtual void VisitElementInit(ElementInit init)
        {

        }

        protected virtual Expression VisitListInit(ListInitExpression node)
        {
            return Visit(node.NewExpression);
        }

        protected virtual Expression VisitMember(MemberExpression node)
        {
            return null;
        }

        protected virtual Expression VisitInvocation(InvocationExpression node)
        {
            return null;
        }

        protected virtual MemberListBinding VisitMemberListBinding(MemberListBinding binding)
        {
            return null;
        }

        protected virtual MemberMemberBinding VisitMenberMemberBinding(MemberMemberBinding binding)
        {
            return null;
        }

        protected virtual Expression VisitMethodCall(MethodCallExpression node)
        {
            return null;
        }

        protected virtual Expression VisitNew(NewExpression expr)
        {
            return null;
        }

        protected virtual Expression VisitNewArray(NewArrayExpression node)
        {
            return null;
        }

        protected virtual Expression VisitTypeBinary(TypeBinaryExpression node)
        {
            return null;
        }
        protected virtual Expression VisitUnary(UnaryExpression node)
        {
            return null;
        }

        private static Expression ConvertMemberAccessToConstant(Expression argument)
        {
            return null;
        }

        private IList<Expression> ParseMemberPath(Expression expression, Path path, IList<Path> list)
        {
            return null;
        }
    }
}

