using System;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources
{
    [Serializable]
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

