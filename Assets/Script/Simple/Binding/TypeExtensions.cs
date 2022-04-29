using System;
using System.Reflection;

namespace Assembly_CSharp.Assets.Script.Simple.Binding
{
    public static class TypeExtensions
    {
        public static MemberInfo FindFirstMemberInfo(this Type type, string name)
        {
            var member = type.GetMember(name);
            if (member == null || member.Length <= 0)
                return null;
            return member[0];
        }

        public static MemberInfo FindFirstMemberInfo(this Type type, string name, BindingFlags flags)
        {
            var member = type.GetMember(name, flags);
            if (member == null || member.Length <= 0)
                return null;
            return member[0];
        }
    }
}

