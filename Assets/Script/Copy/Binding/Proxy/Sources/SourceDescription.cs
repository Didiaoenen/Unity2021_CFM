namespace CFM.Framework.Binding.Proxy.Sources
{
    public class SourceDescription
    {
        private bool isStatic = false;

        public virtual bool IsStatic
        {
            get { return isStatic; }
            set { isStatic = value; }
        }
    }
}

