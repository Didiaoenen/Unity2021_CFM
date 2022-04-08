using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;

namespace CFM.Framework.Localizations
{
    public abstract class AbstractDocumentParser: IDocumentParser
    {
        private List<ITypeConverter> converters = new List<ITypeConverter>();

        public AbstractDocumentParser(): this(null)
        {

        }

        public AbstractDocumentParser(List<ITypeConverter> converters)
        {

        }

        public abstract Dictionary<string, object> Parse(Stream input, CultureInfo cultureInfo);

        protected virtual object Parse(string typeName, string value)
        {
            throw new NotSupportedException(string.Format("{0}", typeName));
        }

        protected virtual object Parse(string typeName, IList<string> values)
        {
            throw new NotSupportedException(string.Format("{0}", typeName));
        }
    }
}

