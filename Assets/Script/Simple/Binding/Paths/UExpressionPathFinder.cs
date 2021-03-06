using System;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Paths
{
    public class UExpressionPathFinder
    {
        private readonly List<Path> list = new List<Path>();

        public List<Path> Paths { get { return list; } }

        public static List<Path> FindPaths(LambdaExpression expression)
        {
            UExpressionPathFinder visitor = new UExpressionPathFinder();
            visitor.Visit(expression);
            return visitor.Paths;
        }

        public virtual Expression Visit(Expression expression)
        {
            if (expression == null)
                return null;

            var bin = expression as BinaryExpression;
            if (bin != null)
                return VisitBinary(bin);

            var cond = expression as ConditionalExpression;
            if (cond != null)
                return VisitConditional(cond);

            var constant = expression as ConstantExpression;
            if (constant != null)
                return VisitConstant(constant);

            var lambda = expression as LambdaExpression;
            if (lambda != null)
                return VisitLambda(lambda);

            var listInit = expression as ListInitExpression;
            if (listInit != null)
                return VisitListInit(listInit);

            var member = expression as MemberExpression;
            if (member != null)
                return VisitMember(member);

            var memberInit = expression as MemberInitExpression;
            if (memberInit != null)
                return VisitMemberInit(memberInit);

            var methodCall = expression as MethodCallExpression;
            if (methodCall != null)
                return VisitMethodCall(methodCall);

            var newExpr = expression as NewExpression;
            if (newExpr != null)
                return VisitNew(newExpr);

            var newArrayExpr = expression as NewArrayExpression;
            if (newArrayExpr != null)
                return VisitNewArray(newArrayExpr);

            var param = expression as ParameterExpression;
            if (param != null)
                return VisitParameter(param);

            var typeBinary = expression as TypeBinaryExpression;
            if (typeBinary != null)
                return VisitTypeBinary(typeBinary);

            var unary = expression as UnaryExpression;
            if (unary != null)
                return VisitUnary(unary);

            var invocation = expression as InvocationExpression;
            if (invocation != null)
                return VisitInvocation(invocation);

            throw new NotSupportedException("Expressions of type " + expression.Type + " are not supports.");
        }

        protected virtual void Visit(IList<Expression> nodes)
        {
            if (nodes == null || nodes.Count <= 0)
                return;

            foreach (Expression expression in nodes)
                Visit(expression);
        }

        protected virtual Expression VisitBinary(BinaryExpression node)
        {
            if (node.NodeType == ExpressionType.ArrayIndex)
            {
                Visit(ParseMemberPath(node, null, list));
            }
            else
            {
                List<Expression> expressionList = new List<Expression>() { node.Left, node.Right, node.Conversion };
                Visit(expressionList);
            }
            return null;
        }

        protected virtual Expression VisitConditional(ConditionalExpression node)
        {
            List<Expression> expressionList = new List<Expression>() { node.IfFalse, node.IfTrue, node.Test };
            Visit(expressionList);
            return null;
        }

        protected virtual Expression VisitConstant(ConstantExpression node)
        {
            return null;
        }

        protected virtual Expression VisitLambda(LambdaExpression node)
        {
            if (node.Parameters != null)
                Visit(node.Parameters.Select(p => p as Expression).ToList());
            return Visit(node.Body);
        }

        protected virtual Expression VisitListInit(ListInitExpression node)
        {
            if (node.Initializers != null)
            {
                foreach (ElementInit init in node.Initializers)
                {
                    VisitElementInit(init);
                }
            }
            return Visit(node.NewExpression);
        }

        protected virtual void VisitElementInit(ElementInit init)
        {
            if (init == null)
                return;

            Visit(init.Arguments);
        }

        protected virtual Expression VisitMember(MemberExpression node)
        {
            Visit(ParseMemberPath(node, null, list));
            return null;
        }

        protected virtual Expression VisitMemberInit(MemberInitExpression node)
        {
            return Visit(node.NewExpression);
        }

        protected virtual Expression VisitMethodCall(MethodCallExpression node)
        {
            Visit(ParseMemberPath(node, null, list));
            return null;
        }

        protected virtual Expression VisitNew(NewExpression node)
        {
            Visit(node.Arguments);
            return null;
        }

        protected virtual Expression VisitNewArray(NewArrayExpression node)
        {
            Visit(node.Expressions);
            return null;
        }

        protected virtual Expression VisitParameter(ParameterExpression node)
        {
            return null;
        }

        protected virtual Expression VisitTypeBinary(TypeBinaryExpression node)
        {
            return Visit(node.Expression);
        }

        protected virtual Expression VisitUnary(UnaryExpression node)
        {
            return Visit(node.Operand);
        }

        protected virtual Expression VisitInvocation(InvocationExpression node)
        {
            Visit(node.Arguments);
            return Visit(node.Expression);
        }

        private static Expression ConvertMemberAccessToConstant(Expression argument)
        {
            if (argument is ConstantExpression)
                return argument;

            var boxed = Expression.Convert(argument, typeof(object));
#if UNITY_IOS || ENABLE_IL2CPP
            var fun = (Func<object[], object>)Expression.Lambda<Func<object>>(boxed).DynamicCompile();
            var constant = fun(new object[] { });
#else
            var fun = Expression.Lambda<Func<object>>(boxed).Compile();
            var constant = fun();
#endif

            return Expression.Constant(constant);
        }

        private IList<Expression> ParseMemberPath(Expression expression, Path path, IList<Path> list)
        {
            if (expression.NodeType != ExpressionType.MemberAccess && expression.NodeType != ExpressionType.Call && expression.NodeType != ExpressionType.ArrayIndex)
                throw new Exception();

            List<Expression> result = new List<Expression>();

            Expression current = expression;
            while (current != null && (current is MemberExpression || current is MethodCallExpression || current is BinaryExpression || current is ParameterExpression || current is ConstantExpression))
            {
                if (current is MemberExpression)
                {
                    if (path == null)
                    {
                        path = new Path();
                        list.Add(path);
                    }

                    var me = current as MemberExpression;
                    var field = me.Member as FieldInfo;
                    if (field != null)
                        path.Prepend(new MemberNode(field));

                    var property = me.Member as PropertyInfo;
                    if (property != null)
                        path.Prepend(new MemberNode(property));

                    current = me.Expression;
                }
                else if (current is MethodCallExpression)
                {
                    var mc = current as MethodCallExpression;
                    if (mc.Method.Name.Equals("get_Item") && mc.Arguments.Count == 1)
                    {
                        if (path == null)
                        {
                            path = new Path();
                            list.Add(path);
                        }

                        var argument = mc.Arguments[0];
                        if (argument as ConstantExpression == null)
                            argument = ConvertMemberAccessToConstant(argument);

                        object value = (argument as ConstantExpression).Value;
                        if (value is string)
                            path.PrependIndexed((string)value);
                        else if (value is int)
                            path.PrependIndexed((int)value);

                        current = mc.Object;
                    }
                    else
                    {
                        current = null;
                        result.AddRange(mc.Arguments);
                        result.Add(mc.Object);
                    }
                }
                else if (current is BinaryExpression)
                {
                    var binary = current as BinaryExpression;
                    if (binary.NodeType == ExpressionType.ArrayIndex)
                    {
                        if (path == null)
                        {
                            path = new Path();
                            list.Add(path);
                        }

                        var left = binary.Left;
                        var right = binary.Right;
                        if (right as ConstantExpression == null)
                            right = ConvertMemberAccessToConstant(right);

                        object value = (right as ConstantExpression).Value;
                        if (value is string)
                            path.PrependIndexed((string)value);
                        else if (value is int)
                            path.PrependIndexed((int)value);

                        current = left;
                    }
                    else
                    {
                        current = null;
                    }
                }
                else if (current is ParameterExpression)
                {
                    current = null;
                }
                else if (current is ConstantExpression)
                {
                    current = null;
                }
            }

            if (current != null)
                result.Add(current);

            return result;
        }
    }
}

