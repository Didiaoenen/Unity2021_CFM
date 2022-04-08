namespace CFM.Framework.Binding.Paths
{
    public class PathToken
    {
        private Path path;

        private int pathIndex;

        private PathToken nextToken;

        public PathToken(Path path, int pathIndex)
        {
            this.path = path;
            this.pathIndex = pathIndex;
        }

        public Path Path
        {
            get { return path; }
        }

        public int Index { get { return pathIndex; } }

        public IPathNode Current
        {
            get { return path[pathIndex]; }
        }

        public bool HasNext()
        {
            return true;
        }

        public PathToken NextToken()
        {
            return this.nextToken;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}

