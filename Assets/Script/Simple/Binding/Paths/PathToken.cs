using System;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Paths
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

        public Path Path { get { return path; } }

        public int Index { get { return pathIndex; } }

        public IPathNode Current { get { return path[pathIndex]; } }

        public bool HasNext()
        {
            if (path.Count <= 0 || pathIndex >= path.Count - 1)
                return false;
            return true;
        }

        public PathToken NextToken()
        {
            if (!HasNext())
                throw new IndexOutOfRangeException();

            if (nextToken == null)
                nextToken = new PathToken(path, pathIndex + 1);

            return nextToken;
        }
    }
}

