using System;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace CFM.Framework.Binding.Paths
{
    public class Path : IEnumerator<IPathNode>
    {

        private readonly object _lock = new object();

        private List<IPathNode> nodes = new List<IPathNode>();

        private PathToken token;

        public Path() : this(null)
        {

        }

        public Path(IPathNode root)
        {
            if (root != null)
                Prepend(root);
        }

        public IPathNode this[int index]
        {
            get { return nodes[index]; }
        }

        public int Count
        {
            get { return nodes.Count; }
        }

        public bool IsStatic { get { return nodes.Exists(n => n.IsStatic); } }

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
            Prepend(new StringIndexedNode(indexValue));
        }

        public void PrependIndexed(int indexValue)
        {
            Prepend(new IntegerIndexedNode(indexValue));
        }

        public void AppendIndexed(string indexValue)
        {
            Append(new StringIndexedNode(indexValue));
        }

        public void AppendIndexed(int indexValue)
        {
            Append(new IntegerIndexedNode(indexValue));
        }

        public PathToken AsPathToken()
        {
            if (token != null)
                return token;

            lock (_lock)
            {
                if (token != null)
                    return token;

                if (nodes.Count <= 0)
                    throw new InvalidOperationException("");

                token = new PathToken(this, 0);
                return token;
            }
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            foreach (var node in nodes)
            {
                node.AppendTo(buf);
            }
            return buf.ToString();
        }

        private int index = -1;

        public IPathNode Current
        {
            get { return nodes[index]; }
        }

        object IEnumerator.Current
        {
            get { return nodes[index]; }
        }

        public bool MoveNext()
        {
            index++;
            return index >= 0 && index < this.nodes.Count;
        }

        public void Reset()
        {
            index = -1;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
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

        ~Path()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    public interface IPathNode
    {
        bool IsStatic { get; }

        void AppendTo(StringBuilder output);
    }

    [Serializable]
    public class MemberNode: IPathNode
    {
        private readonly MemberInfo memberInfo;

        private readonly string name;

        private readonly Type type;

        private readonly bool isStatic;

        public MemberNode(string name): this(null, name, false)
        {

        }

        public MemberNode(Type type, string name, bool isStatic)
        {
            this.name = name;
            this.type = type;
            this.isStatic = isStatic;
        }

        public MemberNode(MemberInfo memberInfo)
        {
            this.memberInfo = memberInfo;
            name=memberInfo.Name;
            type = memberInfo.DeclaringType;
            isStatic = memberInfo.IsStatic();
        }

        public bool IsStatic { get { return isStatic; } }

        public Type Type { get { return type; } }

        public string Name { get { return name; } }

        public MemberInfo MemberInfo { get { return memberInfo; } }

        public void AppendTo(StringBuilder output)
        {
            if (output.Length > 0)
                output.Append('.');

            if (IsStatic)
                output.Append(type.FullName).Append('.');
            
            output.Append(Name);
        }

        public override string ToString()
        {
            return "MemberNode" + (Name == null ? "null" : Name);
        }
    }

    [Serializable]
    public abstract class IndexedNode : IPathNode
    {
        private object _value;

        public IndexedNode(object value)
        {
            _value = value;
        }

        public bool IsStatic { get { return false; } }

        public object Value { 
            get { return _value; }
            private set { _value = value; }
        }

        public abstract void AppendTo(StringBuilder output);

        public override string ToString()
        {
            return base.ToString();
        }
    }

    [Serializable]
    public abstract class IndexedNode<T>: IndexedNode, IPathNode
    {
        public IndexedNode(T value): base(value)
        {

        }

        public new T Value { get { return (T)base.Value; } }
    }

    [Serializable]
    public class StringIndexedNode: IndexedNode<string>
    {
        public StringIndexedNode(string indexValue): base(indexValue)
        {

        }

        public override void AppendTo(StringBuilder output)
        {
            output.AppendFormat("[\"{0}\"]", Value);
        }
    }

    [Serializable]
    public class IntegerIndexedNode: IndexedNode<int>
    {
        public IntegerIndexedNode(int indexValue): base(indexValue)
        {

        }

        public override void AppendTo(StringBuilder output)
        {
            output.AppendFormat("[{0}]", Value);
        }
    }
}

