using System;

namespace CFM.Framework.Localizations.UI
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class AllowedMembersAttribute : Attribute
    {
        private Type type;

        private string[] names;

        public AllowedMembersAttribute(Type type, params string[] names)
        {
            this.type = type;
            this.names = names;
            if (this.names == null)
                this.names = new string[0];
        }

        public Type Type { get { return type; } }

        public string[] Names { get { return names; } }
    }
}

