using XLua;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources.Expressions
{
    public class LuaExpressionSourceDescription : SourceDescription
    {
        private LuaFunction expression;

        private string[] paths;

        public LuaFunction Expression
        {
            get { return expression; }
            set { expression = value; }
        }

        public string[] Paths
        {
            get { return paths; }
            set { paths = value; }
        }

        public LuaExpressionSourceDescription() : this(false)
        {

        }

        public LuaExpressionSourceDescription(bool isStatic)
        {
            IsStatic = isStatic;
        }

        public override string ToString()
        {
            return expression == null ? "Expression:null" : "Expression:" + expression.ToString();
        }
    }
}

