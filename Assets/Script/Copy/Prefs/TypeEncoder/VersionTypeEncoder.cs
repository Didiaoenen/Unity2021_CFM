using System;

namespace CFM.Framework.Prefs.TypeEncoder
{
    public class VersionTypeEncoder : ITypeEncoder
    {
        private int priority = 999;

        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        public bool IsSupport(Type type)
        {
            if (type.Equals(typeof(Version)))
                return true;
            return false;
        }

        public object Decode(Type type, string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            return new Version(value);
        }

        public string Encode(object value)
        {
            return ((Version)value).ToString();
        }
    }
}
