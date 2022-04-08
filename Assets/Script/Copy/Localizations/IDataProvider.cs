using System.Globalization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CFM.Framework.Localizations
{
    public interface IDataProvider
    {
        Task<Dictionary<string, object>> Load(CultureInfo cultureInfo);
    }
}

