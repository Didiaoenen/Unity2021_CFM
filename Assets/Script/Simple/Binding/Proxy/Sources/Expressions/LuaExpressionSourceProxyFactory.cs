using Assembly_CSharp.Assets.Script.Simple.Binding.Paths;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources.Object;
using System.Collections.Generic;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources.Expressions
{
    public class LuaExpressionSourceProxyFactory : TypedSourceProxyFactory<LuaExpressionSourceDescription>
    {
        private ISourceProxyFactory factory;

        public LuaExpressionSourceProxyFactory(ISourceProxyFactory factory)
        {
            this.factory = factory;
        }

        private List<Path> FindPaths(string[] textPaths)
        {
            List<Path> paths = new List<Path>();
            if (textPaths == null)
                return paths;

            for (int i = 0; i < textPaths.Length; i++)
            {
                Path path = UPathParser.Parse(textPaths[i]);
                if (path != null && !paths.Contains(path))
                    paths.Add(path);
            }

            return paths;
        }

        protected override bool TryCreateProxy(object source, LuaExpressionSourceDescription description, out ISourceProxy proxy)
        {
            proxy = null;
            if (source == null && !description.IsStatic)
            {
                proxy = new EmptySourceProxy(description);
                return true;
            }

            List<ISourceProxy> list = new List<ISourceProxy>();
            if (!description.IsStatic)
            {
                List<Path> paths = FindPaths(description.Paths);
                foreach (Path path in paths)
                {
                    ISourceProxy innerProxy = factory.CreateProxy(source, new ObjectSourceDescription() { Path = path });
                    if (innerProxy != null)
                        list.Add(innerProxy);
                }
            }
            proxy = new LuaExpressionSourceProxy(source, description.Expression, list);
            return true;
        }
    }
}

