using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace CFM.Framework.Utilities
{
    public class StringSpliter : IEnumerator<char>
    {
        [ThreadStatic]
        private static StringSpliter spliter;

        private static StringSpliter Spliter
        {
            get
            {
                if (spliter == null)
                    spliter = new StringSpliter();
                return spliter;
            }
        }

        public static string[] Split(string input, params char[] characters)
        {
            if (string.IsNullOrEmpty(input))
                return new string[0];

            var spliter = Spliter;
            try
            {
                spliter.Reset(input, characters);
                return spliter.Split();
            }
            finally
            {
                spliter.Clear();
            }
        }

        private string text;

        private char[] separators;

        private int total = 0;

        private int pos = -1;

        private readonly List<string> items = new List<string>();

        private StringSpliter()
        {
        }

        public void Reset(string text, char[] separators)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("Invalid argument", "text");

            if (separators == null || separators.Length == 0)
                this.separators = new char[] { ',' };
            else
                this.separators = separators;

            this.text = text;
            total = this.text.Length;
            pos = -1;
            items.Clear();
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
            if (pos < total - 1)
            {
                pos++;
                return true;
            }
            return false;
        }

        public void Reset()
        {
            pos = -1;
            items.Clear();
        }

        public void Clear()
        {
            text = null;
            separators = null;
            pos = -1;
            total = 0;
            items.Clear();
        }

        public string[] Split()
        {
            while (MoveNext())
            {
                char ch = Current;
                if (separators.Contains(ch))
                {
                    items.Add("");
                    continue;
                }

                string content = ReadString(separators);
                items.Add(content);
            }

            if (separators.Contains(Current))
                items.Add("");

            return items.ToArray();
        }

        private bool IsEOF()
        {
            return pos >= total;
        }

        private void ReadStructString(StringBuilder buf, char start, char end)
        {
            char ch = Current;
            if (ch != start)
                throw new Exception(string.Format("Error parsing string , unexpected quote character {0} in text {1}", ch, this.text));

            buf.Append(ch);

            while (MoveNext())
            {
                ch = Current;
                if (ch == '(')
                {
                    ReadStructString(buf, '(', ')');
                    continue;
                }
                else if (ch == '[')
                {
                    ReadStructString(buf, '[', ']');
                    continue;
                }
                else if (ch == '{')
                {
                    ReadStructString(buf, '{', '}');
                    continue;
                }
                else if (ch == '<')
                {
                    ReadStructString(buf, '<', '>');
                    continue;
                }

                buf.Append(ch);
                if (ch == end)
                    return;
            }

            throw new Exception(string.Format("Not found the end character '{0}' in the text {1}.", end, text));
        }

        private void ReadQuotedString(StringBuilder buf, char start, char end)
        {
            char prev = '\0';
            char ch = Current;
            if (ch != start)
                throw new Exception(string.Format("Error parsing string , unexpected quote character {0} in text {1}", ch, text));

            while (MoveNext())
            {
                prev = ch;
                ch = Current;
                if (prev != '\\' && ch == end)
                    return;

                buf.Append(ch);
            }

            throw new Exception(string.Format("Not found the end character '{0}' in the text {1}.", end, text));
        }

        private string ReadString(char[] separators)
        {
            StringBuilder buf = new StringBuilder();
            char ch = Current;
            do
            {
                ch = Current;
                if (ch == '(')
                {
                    ReadStructString(buf, '(', ')');
                }
                else if (ch == '[')
                {
                    ReadStructString(buf, '[', ']');
                }
                else if (ch == '{')
                {
                    ReadStructString(buf, '{', '}');
                }
                else if (ch == '<')
                {
                    ReadStructString(buf, '<', '>');
                }
                else if (ch == '\'')
                {
                    ReadQuotedString(buf, '\'', '\'');
                }
                else if (ch == '\"')
                {
                    ReadQuotedString(buf, '\"', '\"');
                }
                else
                {
                    if (separators.Contains(ch))
                        break;

                    buf.Append(ch);
                }
            } while (MoveNext());

            buf.Replace("&quot;", "\"");
            buf.Replace("\\\"", "\"");
            return buf.ToString();
        }
    }
}