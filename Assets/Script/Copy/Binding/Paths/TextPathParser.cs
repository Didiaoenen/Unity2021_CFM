using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace CFM.Framework.Binding.Paths
{
    public class TextPathParser : IEnumerator<char>
    {
        protected string text;

        protected int total = 0;
        
        protected int pos = -1;
        
        protected Path path = null;

        public TextPathParser(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("Invalid argument", "text");

            this.text = text.Replace(" ", "");
            if (string.IsNullOrEmpty(this.text) || this.text.StartsWith("."))
                throw new ArgumentException("Invalid argument", "text");

            total = this.text.Length;
            pos = -1;
        }

        public char Current
        {
            get { return text[pos]; }
        }

        object IEnumerator.Current
        {
            get { return text[pos]; }
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
            return pos >= total;
        }

        public Path Parse()
        {
            if (path != null)
                return path;

            path = new Path();
            MoveNext();
            do
            {
                SkipWhiteSpaceAndCharacters('.');

                if (IsEOF())
                    break;

                if (Current.Equals('['))
                {
                    //parse index
                    ReadIndex();
                    SkipWhiteSpace();
                    if (!Current.Equals(']'))
                        throw new BindingException("Error parsing indexer , unterminated in text {0}", text);

                    if (MoveNext())
                    {
                        if (!Current.Equals('.'))
                            throw new BindingException("Error parsing path , unterminated in text {0}", text);
                    }
                }
                else if (char.IsLetter(Current) || Current == '_')
                {
                    //parse member name
                    ParseMemberName();
                    if (!IsEOF() && !Current.Equals('.') && !Current.Equals('[') && !char.IsWhiteSpace(Current))
                        throw new BindingException("Error parsing path , unterminated in text {0}", text);
                }
                else
                {
                    throw new BindingException("Error parsing path , unterminated in text {0}", text);
                }
            } while (!IsEOF());
            return path;
        }

        protected void ReadIndex()
        {
            if (!MoveNext())
                throw new BindingException("Error parsing string indexer , unterminated in text {0}", text);

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

            throw new BindingException("Error parsing indexer , unterminated in text {0}", text);
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
                throw new BindingException("Error parsing member name , unterminated in text {0}", text);

            path.Append(new MemberNode(buf.ToString()));
        }

        protected uint ReadUnsignedInteger()
        {
            var buf = new StringBuilder();
            do
            {
                if (!char.IsDigit(Current))
                    break;

                buf.Append(Current);

            } while (MoveNext());

            uint index;
            if (!uint.TryParse(buf.ToString(), out index))
                throw new BindingException("Unable to parse integer text from {0} in {1}", buf.ToString(), text);
            return index;
        }

        protected string ReadQuotedString()
        {
            char ch = Current;
            if (ch != '\'' && ch != '\"')
                throw new BindingException("Error parsing string indexer , unexpected quote character {0} in text {1}", ch, text);

            if (!MoveNext())
                throw new BindingException("Error parsing string indexer , unterminated in text {0}", text);

            var buf = new StringBuilder();
            do
            {
                ch = Current;
                if (!char.IsLetterOrDigit(ch) && ch != '_' && ch != '-')
                    break;

                buf.Append(ch);
            } while (MoveNext());

            if (buf.Length <= 0 || (ch != '\'' && ch != '\"'))
                throw new BindingException("Error parsing string indexer , unexpected quote character {0} in text {1}", ch, text);
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

        protected void SkipWhiteSpaceAndCharacters(params char[] characters)
        {
            while (IsWhiteSpaceOrCharacter(Current, characters) && MoveNext())
            {
            }
        }
    }
}

