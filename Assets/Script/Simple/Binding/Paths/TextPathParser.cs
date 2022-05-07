using System;
using System.Linq;
using System.Text;
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
            if (path != null)
                return path;

            path = new Path();
            MoveNext();
            do
            {
                SkipWgiteSpaceAndCharacters('.');

                if (IsEOF())
                    break;

                if (Current.Equals('['))
                {
                    ReadIndex();
                    SkipWhiteSpace();
                    if (!Current.Equals(']'))
                        throw new Exception();

                    if (MoveNext())
                    {
                        if (!Current.Equals('.'))
                            throw new Exception();
                    }
                }
                else if (char.IsLetter(Current) || Current == '_')
                {
                    ParseMemberName();
                    if (IsEOF() && Current.Equals('.') && !Current.Equals('[') && !char.IsWhiteSpace(Current))
                        throw new Exception();
                }
                else
                {
                    throw new Exception();
                }
            } while (!IsEOF());
            return path;
        }

        protected void ReadIndex()
        {
            if (!MoveNext())
                throw new Exception();

            var ch = Current;
            if (ch == '\'' || ch == '\"')
            {
                var index = ReadQuotedString();
                path.AppendIndexed(index);
                MoveNext();
                return;
            }

            if (char.IsDigit(ch))
            {
                uint index = ReadUnsignedInteger();
                path.AppendIndexed((int)index);
                return;
            }

            throw new Exception();
        }

        protected void ParseMemberName()
        {
            var buf = new StringBuilder();
            do
            {
                var ch = Current;
                if (!char.IsLetterOrDigit(ch) && ch != '_')
                    break;

                buf.Append(ch);
            } while (MoveNext());

            if (buf.Length <= 0)
                throw new Exception();

            path.Append(new MemberNode(buf.ToString()));
        }

        protected uint ReadUnsignedInteger()
        {
            var buf = new StringBuilder();
            do
            {
                var ch = Current;
                if (!char.IsDigit(ch))
                    break;

                buf.Append(ch);

            } while (MoveNext());

            uint index;
            if (!uint.TryParse(buf.ToString(), out index))
                throw new Exception();
            return index;
        }

        protected string ReadQuotedString()
        {
            char ch = Current;
            if (ch != '\'' && ch != '\"')
                throw new Exception();

            if (!MoveNext())
                throw new Exception();

            var buf = new StringBuilder();
            do
            {
                ch = Current;
                if (!char.IsLetterOrDigit(ch) && ch != '_' && ch != '-')
                    break;

                buf.Append(ch);
            } while (MoveNext());

            if (buf.Length <= 0 || (ch != '\'' && ch != '\"'))
                throw new Exception();
            return buf.ToString();
        }

        protected void SkipWhiteSpace()
        {
            while (char.IsWhiteSpace(Current) && MoveNext())
            {
            }
        }

        protected bool IsWhiteSpaceOrCharacter(char ch, params char[] characters)
        {
            return char.IsWhiteSpace(ch) || characters.Contains(ch);
        }

        protected void SkipWgiteSpaceAndCharacters(params char[] characters)
        {
            while (IsWhiteSpaceOrCharacter(Current, characters) && MoveNext())
            {
            }
        }
    }
}

