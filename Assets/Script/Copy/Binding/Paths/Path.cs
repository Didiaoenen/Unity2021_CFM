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
                this.Prepend(root);
        }

        public IPathNode this[int index]
        {
            get { return this.nodes[index]; }
        }

        public int Count
        {
            get { return this.nodes.Count; }
        }

        public bool IsStatic { get { return nodes.Exists(n => n.IsStatic); } }

        public List<IPathNode> ToList()
        {
            return new List<IPathNode>(nodes);
        }

        public void Prepend(IPathNode node)
        {
            this.nodes.Insert(0, node);
        }

        public void PrependIndexed(string indexValue)
        {
            this.Prepend(new StringIndexedNode(indexValue));
        }

        public void PrependIndexed(int indexValue)
        {
            this.Prepend(new IntegerIndexedNode(indexValue));
        }

        public PathToken AsPathToken()
        {
            if (this.token != null)
                return this.token;

            lock (_lock)
            {
                if (this.token != null)
                    return this.token;

                if (this.nodes.Count <= 0)
                    throw new InvalidOperationException("");

                this.token = new PathToken(this, 0);
                return this.token;
            }
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            foreach (var node in this.nodes)
            {
                node.AppendTo(buf);
            }
            return buf.ToString();
        }

        private int index = -1;

        public IPathNode Current
        {
            get { return this.nodes[index]; }
        }

        object IEnumerator.Current
        {
            get { return this.nodes[index]; }
        }

        public bool MoveNext()
        {
            this.index++;
            return this.index >= 0 && index < this.nodes.Count;
        }

        public void Reset()
        {
            this.index = -1;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    this.nodes.Clear();
                    this.index = -1;
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
            this.name=memberInfo.Name;
            this.type = memberInfo.DeclaringType;
            this.isStatic = memberInfo.IsStatic();
        }

        public bool IsStatic { get { return this.isStatic; } }

        public Type Type { get { return this.type; } }

        public string Name { get { return this.name; } }

        public MemberInfo MemberInfo { get { return this.memberInfo; } }

        public void AppendTo(StringBuilder output)
        {
            if (output.Length > 0)
                output.Append('.');

            if (IsStatic)
                output.Append(this.type.FullName).Append('.');
            
            output.Append(this.Name);
        }

        public override string ToString()
        {
            return "MemberNode" + (this.Name == null ? "null" : this.Name);
        }
    }

    [Serializable]
    public abstract class IndexedNode: IPathNode
    {
        private object _value;

        public IndexedNode(object value)
        {
            this._value = value;
        }

        public bool IsStatic { get { return false; } }

        public object Value { 
            get { return this._value; }
            private set { this._value = value; }
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
            output.AppendFormat("[\"{0}\"]", this.Value);
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
            output.AppendFormat("[{0}]", this.Value);
        }
    }
}

