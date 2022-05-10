using System;
using System.Reflection;
using System.Linq.Expressions;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Paths
{
    public class UPathParser
    {
        public static Path Parse(string pathText)
        {
            return new TextPathParser(pathText.Replace(" ", "")).Parse();
        }

        public static Path Parse(LambdaExpression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            Path path = new Path();
            var body = expression.Body as MemberExpression;
            if (body != null)
            {
                Parse(body, path);
                return path;
            }

            var method = expression.Body as MethodCallExpression;
            if (method != null)
            {
                Parse(method, path);
                return path;
            }

            var unary = expression.Body as UnaryExpression;
            if (unary != null && unary.NodeType == ExpressionType.Convert)
            {
                Parse(unary, path);
                return path;
            }

            var binary = expression.Body as BinaryExpression;
            if (binary != null && binary.NodeType == ExpressionType.ArrayIndex)
            {
                Parse(binary, path);
                return path;
            }

            throw new ArgumentException(string.Format(""));
        }

        private static void Parse(Expression expression, Path path)
        {
            if (expression == null || !(expression is MemberExpression || expression is MethodCallExpression || expression is BinaryExpression))
                return;

            if (expression is MemberExpression)
            {
                var member = (expression as MemberExpression);
                if (member.Member.IsStatic())
                {
                    path.Prepend(new MemberNode(member.Member));
                    return;
                }
                else
                {
                    path.Prepend(new MemberNode(member.Member));
                    if (member.Expression != null)
                        Parse(member.Expression, path);
                    return;
                }
            }

            if (expression is MethodCallExpression)
            {
                var methodCall = expression as MethodCallExpression;
                if (methodCall.Method.Name.Equals("get_Item") && methodCall.Arguments.Count == 1)
                {
                    var argument = methodCall.Arguments[0];
                    var  constant = argument as ConstantExpression;
                    if (constant == null)
                        argument = ConvertMemberAccessToConstant(argument);

                    object value = (argument as ConstantExpression).Value;
                    if (value is string)
                        path.PrependIndexed((string)value);
                    else if (value is int)
                        path.PrependIndexed((int)value);

                    if (methodCall.Object != null)
                        Parse(methodCall.Object, path);
                    return;
                }

                if (methodCall.Method.Name.Equals("CreateDelegate"))
                {
                    var info = GetDelegateMethodInfo(methodCall);
                    if (info == null)
                        throw new ArgumentException(string.Format(""));

                    if (info.IsStatic)
                    {
                        path.Prepend(new MemberNode(info));
                        return;
                    }
                    else
                    {
                        path.Prepend(new MemberNode(info));
                        if (methodCall.Arguments[1] != null)
                            Parse(methodCall.Arguments[1], path);
                        return;
                    }
                }

                if (methodCall.Method.ReturnType.Equals(typeof(void)))
                {
                    var method = methodCall.Method;
                    if (method.IsStatic)
                    {
                        path.Prepend(new MemberNode(method));
                        return;
                    }
                    else
                    {
                        path.Prepend(new MemberNode(method));
                        if (methodCall.Object != null)
                            Parse(methodCall.Object, path);
                        return;
                    }
                }

                throw new ArgumentException(string.Format(""));
            }

            if (expression is BinaryExpression)
            {
                var binary = expression as BinaryExpression;
                if (binary.NodeType == ExpressionType.ArrayIndex)
                {
                    var left = binary.Left;
                    var right = binary.Right;
                    if (right as ConstantExpression == null)
                        right = ConvertMemberAccessToConstant(right);

                    object value = (right as ConstantExpression).Value;
                    if (value is string)
                        path.PrependIndexed((string)value);
                    else if (value is int)
                        path.PrependIndexed((int)value);

                    if (left != null)
                        Parse(left, path);
                    return;
                }

                throw new ArgumentException(string.Format(""));
            }
        }

        private static Expression ConvertMemberAccessToConstant(Expression argument)
        {
            if (argument == null)
                throw new ArgumentException(nameof(argument));

            if (argument is ConstantExpression)
                return argument;

            var boxed = Expression.Convert(argument, typeof(object));
#if UNITY_IOS || UNITY_ILSCPP
            var func = Expression.Lambda<Func<object>>(boxed).DynamicCompile();
            var constant = func(new object[] { });
#else
            var fun = Expression.Lambda<Func<object>>(boxed).Compile();
            var constant = fun();
#endif
            return Expression.Constant(constant);
        }

        public static string ParseMemberName(string pathText)
        {
            if (pathText == null)
                throw new ArgumentException(nameof(pathText));

            pathText = pathText.Replace(" ", "");
            if (string.IsNullOrEmpty(pathText))
                throw new ArgumentException(nameof(pathText));

            int index = pathText.LastIndexOf('.');
            if (index <= 0)
                throw new ArgumentException(nameof(pathText));

            return pathText.Substring(index + 1);
        }

        public static string ParseMemberName(LambdaExpression expression)
        {
            if (expression == null)
                throw new ArgumentException(nameof(expression));

            var method = expression.Body as MethodCallExpression;
            if (method != null)
                return method.Method.Name;

            var unary = expression.Body as UnaryExpression;
            if (unary != null && unary.NodeType == ExpressionType.Convert)
            {
                var methodCall = unary.Operand as MethodCallExpression;
                if (methodCall != null && methodCall.Method.Name.Equals("CreateDelegate"))
                {
                    var info = GetDelegateMethodInfo(methodCall);
                    if (info != null)
                        return info.Name;
                }
                throw new ArgumentException(string.Format(""));
            }

            var body = expression.Body as MemberExpression;
            if (body != null && body.Expression is ParameterExpression)
            {
                return body.Member.Name;
            }
            throw new ArgumentException(string.Format(""));
        }

        private static MethodInfo GetDelegateMethodInfo(MethodCallExpression expression)
        {
            if (expression == null)
                throw new ArgumentException(nameof(expression));

            var target = expression.Object;
            var arguments = expression.Arguments;
            if (target == null)
            {
                foreach (var expr in arguments)
                {
                    if (expr is ConstantExpression)
                    {
                        var value = (expr as ConstantExpression).Value;
                        if (value is MethodInfo)
                            return value as MethodInfo;
                    }
                }
                return null;
            }
            else if (target is ConstantExpression)
            {
                var value = (target as ConstantExpression).Value;
                if (value is MethodInfo)
                    return value as MethodInfo;
            }
            return null;
        }

        public static Path ParseStaticPath(string pathText)
        {
            string typeName = ParserTypeName(pathText);
            string memberName = ParseMemberName(pathText);
            Type type = TypeFinderUtils.FindType(typeName);

            Path path = new Path();
            path.Append(new MemberNode(type, memberName, true));
            return path;
        }

        public static string ParserTypeName(string pathText)
        {
            if (pathText == null)
                throw new ArgumentException(nameof(pathText));

            pathText = pathText.Replace(" ", "");
            if (string.IsNullOrEmpty(pathText))
                throw new ArgumentException(nameof(pathText));

            int index = pathText.LastIndexOf('.');
            if (index <= 0)
                throw new ArgumentException(nameof(pathText));

            return pathText.Substring(0, index);
        }

        public static Path ParseStaticPath(LambdaExpression expression)
        {
            if (expression == null)
                throw new ArgumentException(nameof(expression));

            var current = expression.Body;
            var unary = current as UnaryExpression;
            if (unary != null)
                current = unary.Operand;

            if (current is MemberExpression)
            {
                Path path = new Path();
                Parse(current, path);
                return path;
            }

            if (current is MethodCallExpression)
            {
                Path path = new Path();
                Parse(current, path);
                return path;
            }

            var binary = current as BinaryExpression;
            if (binary != null && binary.NodeType == ExpressionType.ArrayIndex)
            {
                Path path = new Path();
                Parse(current, path);
                return path;
            }

            throw new ArgumentException(string.Format(""));
        }
    }
}

