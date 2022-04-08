using System;
using CFM.Framework.Binding.Paths;

namespace CFM.Framework.Binding.Proxy.Sources.Expressions
{
    public class ExpressionSourceProxyFactory: TypedSourceProxyFactory<ExpressionSourceDescription>
    {
        private ISourceProxyFactory factory;

        private IExpressionPathFinder pathFinder;

        public ExpressionSourceProxyFactory(ISourceProxyFactory factory, IExpressionPathFinder pathFinder)
        {

        }

        protected override bool TryCreateProxy(object source, ExpressionSourceDescription description, out ISourceProxy proxy)
        {
            throw new NotImplementedException();
        }
    }
}

