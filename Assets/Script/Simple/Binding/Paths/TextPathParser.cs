using System.Collections;
using System.Collections.Generic;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Paths
{
    public class TextPathParser : IEnumerator<char>
    {
        protected string text;

        protected int total = 0;

        protected int pos = -1;

        protected Path path;

        public TextPathParser(string text)
        {

        }

        public char Current => throw new System.NotImplementedException();

        object IEnumerator.Current => throw new System.NotImplementedException();

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public bool MoveNext()
        {
            throw new System.NotImplementedException();
        }

        public void Reset()
        {
            throw new System.NotImplementedException();
        }

        public Path Parse()
        {
            return new Path();
        }
    }
}

