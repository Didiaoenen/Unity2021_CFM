namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources.Text
{
    public class LiteralSourceDescription : SourceDescription
    {
        public object Literal { get; set; }

        public LiteralSourceDescription()
        {
            IsStatic = false;
        }

        public override string ToString()
        {
            return Literal == null ? "Literal:null" : "Literal:" + Literal.ToString();
        }
    }
}

