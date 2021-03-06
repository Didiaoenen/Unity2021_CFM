using System;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Paths
{
    public interface IPathNode
    {
        bool IsStatic { get; }

        void AppendTo(StringBuilder output);
    }

    [Serializable]
    public abstract class IndexedNode : IPathNode
    {
        private object value;

        public object Value
        {
            get { return value; }
            private set { this.value = value; }
        }

        public bool IsStatic { get { return false; } }

        public IndexedNode(object value)
        {
            this.value = value;
        }

        public abstract void AppendTo(StringBuilder output);

        public override string ToString()
        {
            return "IndexedNode:" + (value == null ? "null" : value.ToString());
        }
    }

    [Serializable]
    public abstract class IndexedNode<T> : IndexedNode, IPathNode
    {
        public IndexedNode(T value) : base(value)
        {
        }

        public new T Value { get { return (T)base.Value; } }
    }

    [Serializable]
    public class IntegerIndexedNode : IndexedNode<int>
    {
        public IntegerIndexedNode(int indexValue) : base(indexValue)
        {

        }

        public override void AppendTo(StringBuilder output)
        {
            output.AppendFormat("[{0}]", Value);
        }
    }

    [Serializable]
    public class StringIndexNode : IndexedNode<string>
    {
        public StringIndexNode(string indexValue) : base(indexValue)
        {

        }

        public override void AppendTo(StringBuilder output)
        {
            output.AppendFormat("[\"{0}\"]", Value);
        }
    }

    [Serializable]
    public class MemberNode : IPathNode
    {
        private readonly MemberInfo memberInfo;

        private readonly string name;

        private readonly Type type;

        private readonly bool isStatic;

        public MemberInfo MemberInfo { get { return memberInfo; } }

        public string Name { get { return name; } }

        public Type Type { get { return type; } }

        public bool IsStatic { get { return isStatic; } }

        public MemberNode(string name) : this(null, name, false)
        {

        }

        public MemberNode(Type type, string name, bool isStatic)
        {
            this.type = type;
            this.name = name;
            this.isStatic = isStatic;
        }

        public MemberNode(MemberInfo memberInfo)
        {
            this.memberInfo = memberInfo;
            name = memberInfo.Name;
            type = memberInfo.DeclaringType;
            isStatic = memberInfo.IsStatic();
        }

        public void AppendTo(StringBuilder output)
        {
            if (output.Length > 0)
                output.Append(".");
            if (IsStatic)
                output.Append(type.FullName).Append(".");
            output.Append(Name);
        }

        public override string ToString()
        {
            return "MemberNode:" + (Name == null ? "null" : Name);
        }
    }

    [Serializable]
    public class Path : DisposableBase, IEnumerator<IPathNode>
    {
        private bool disposed = false;

        private readonly object _lock = new object();

        private List<IPathNode> nodes = new List<IPathNode>();

        private PathToken token;

        private int index = -1;

        public IPathNode this[int index] { get { return nodes[index]; } }

        public int Count { get { return nodes.Count; } }

        public bool IsStatic { get { return nodes.Exists(n => n.IsStatic); } }

        public IPathNode Current { get { return nodes[index]; } }

        object IEnumerator.Current { get { return nodes[index]; } }

        public Path() : this(null)
        {

        }

        public Path(IPathNode root)
        {
            if (root != null)
                Prepend(root);
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    nodes.Clear();
                    index = -1;
                }
                disposed = true;
            }
        }

        public bool MoveNext()
        {
            index++;
            return index >= 0 && index < nodes.Count;
        }

        public void Reset()
        {
            index = -1;
        }

        public PathToken AsPathToken()
        {
            if (token != null)
                return token;

            lock(_lock)
            {
                if (token != null)
                    return token;

                if (nodes.Count <= 0)
                    throw new InvalidOperationException("The path node is empty");

                token = new PathToken(this, 0);
                return token;
            }
        }

        public void AppendIndexed(int indexValue)
        {
            Append(new IntegerIndexedNode(indexValue));
        }

        public void AppendIndexed(string indexValue)
        {
            Append(new StringIndexNode(indexValue));
        }

        public List<IPathNode> ToList()
        {
            return new List<IPathNode>(nodes);
        }

        public void Append(IPathNode node)
        {
            nodes.Add(node);
        }

        public void Prepend(IPathNode node)
        {
            nodes.Insert(0, node);
        }

        public void PrependIndexed(string indexValue)
        {
            Prepend(new StringIndexNode(indexValue));
        }

        public void PrependIndexed(int indexValue)
        {
            Prepend(new IntegerIndexedNode(indexValue));
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }

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

