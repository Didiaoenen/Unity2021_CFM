using System;
using System.Linq;
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

        public char Current { get { return text[pos]; } }

        object IEnumerator.Current { get { return text[pos]; } }

        public TextPathParser(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException();

            text = text.Replace(" ", "");
            if (string.IsNullOrEmpty(text) || text.StartsWith("."))
                throw new ArgumentException();

            total = text.Length;
            pos = -1;
        }

        public void Dispose()
        {
            text = null;
            pos = -1;
        }

        public bool MoveNext()
        {
            if (pos++ < total - 1)
                return true;
            return false;
        }

        public void Reset()
        {
            pos = -1;
        }

        protected bool IsEOF()
        {
            return pos > total;
        }

        public Path Parse()
        {
            return new Path();
        }

        protected void ReadIndex()
        {

        }

        protected void ParseMemberName()
        {

        }

        protected uint ReadUnsignedInteger()
        {
            return 0;
        }

        protected string ReadQuotedString()
        {
            return "";
        }

        protected void SkipWhiteSpace()
        {

        }

        protected bool IsWhiteSpaceOrCharacter(char ch, params char[] characters)
        {
            return char.IsWhiteSpace(ch) || characters.Contains(ch);
        }

        protected void SkipWgiteSpaceAndCharacters(params char[] characters)
        {

        }
    }
}

