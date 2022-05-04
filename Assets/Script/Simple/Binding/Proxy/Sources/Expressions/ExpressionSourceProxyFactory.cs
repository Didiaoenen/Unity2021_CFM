using Assembly_CSharp.Assets.Script.Simple.Binding.Expressions;
using Assembly_CSharp.Assets.Script.Simple.Binding.Paths;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources.Object;
using System;
using System.Collections.Generic;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources.Expressions
{
    public class ExpressionSourceProxyFactory : TypedSourceProxyFactory<ExpressionSourceDescription>
    {
        private ISourceProxyFactory factory;

        private IExpressionPathFinder pathFinder;

        public ExpressionSourceProxyFactory(ISourceProxyFactory factory, IExpressionPathFinder pathFinder)
        {
            this.factory = factory;
            this.pathFinder = pathFinder;
        }

        protected override bool TryCreateProxy(object source, ExpressionSourceDescription description, out ISourceProxy proxy)
        {
            proxy = null;
            var expression = description.Expression;
            List<ISourceProxy> list = new List<ISourceProxy>();
            List<Path> paths = pathFinder.FindPaths(expression);
            foreach (var path in paths)
            {
                if (!path.IsStatic)
                {
                    if (source == null)
                        continue;

                    MemberNode memberNode = path[0] as MemberNode;
                    if (memberNode != null && memberNode.MemberInfo != null && !memberNode.MemberInfo.DeclaringType.IsAssignableFrom(source.GetType()))
                        continue;
                }

                ISourceProxy innerProxy = factory.CreateProxy(source, new ObjectSourceDescription() { Path = path });
                if (innerProxy != null)
                    list.Add(innerProxy);
            }

            try
            {
                var del = expression.Compile();
                Type returnType = del.ReturnType();
                Type parameterType = del.ParameterType();
                if (parameterType != null)
                {
                    proxy = (ISourceProxy)Activator.CreateInstance(typeof(ExpressionSourceProxy<,>).MakeGenericType(parameterType, returnType), source, del, list);
                }
                else
                {
                    proxy = (ISourceProxy)Activator.CreateInstance(typeof(ExpressionSourceProxy<>).MakeGenericType(returnType), del, list);
                }
            }
            catch (Exception)
            {
                Func<object[], object> del = expression.DynamicCompile();
                proxy = new ExpressionSourceProxy(description.IsStatic ? null : source, del, description.ReturnType, list);
            }

            if (proxy != null)
                return true;

            return false;
        }
    }
}
