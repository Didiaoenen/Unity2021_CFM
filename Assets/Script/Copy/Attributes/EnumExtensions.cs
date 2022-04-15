using System;
using System.Reflection;

namespace CFM.Framework.Attributes
{
    public static class EnumExtensions
    {
        public static string GetRemark(this Enum e)
        {
            Type type = e.GetType();
            FieldInfo fieldInfo = type.GetField(e.ToString());
            if (fieldInfo == null)
                return string.Empty;

            object[] attrs = fieldInfo.GetCustomAttributes(typeof(RemarkAttribute), false);
            foreach (RemarkAttribute attr in attrs)
                return attr.Remark;

            return string.Empty;
        }
    }
}

