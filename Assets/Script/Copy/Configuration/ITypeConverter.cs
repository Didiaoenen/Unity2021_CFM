using System;

namespace CFM.Framework.Configuration
{
    public interface ITypeConverter
    {
        bool Support(string typeName);

        Type GetType(string typeName);

        object Convert(Type type, object value);
    }
}

