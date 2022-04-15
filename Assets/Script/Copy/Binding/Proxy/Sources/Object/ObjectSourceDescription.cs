using System;

using CFM.Framework.Binding.Paths;

namespace CFM.Framework.Binding.Proxy.Sources.Object
{
    [Serializable]
    public class ObjectSourceDescription : SourceDescription
    {
        private Path path;

        public ObjectSourceDescription()
        {
            IsStatic = false;
        }

        public virtual Path Path
        {
            get { return path; }
            set
            {
                path = value;
                if (path != null)
                    IsStatic = path.IsStatic;
            }
        }

        public override string ToString()
        {
            return path == null ? "Path:null" : "Path:" + path.ToString();
        }
    }
}

