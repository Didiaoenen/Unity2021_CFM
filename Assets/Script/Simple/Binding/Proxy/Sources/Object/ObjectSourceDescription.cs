using Assembly_CSharp.Assets.Script.Simple.Binding.Paths;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources.Object
{
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

