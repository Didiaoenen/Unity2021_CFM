using System.IO;
using System.Globalization;
using System.Collections.Generic;

namespace CFM.Framework.Localizations
{
    public interface IDocumentParser
    {
        Dictionary<string, object> Parse(Stream input, CultureInfo cultureInfo);
    }
}

