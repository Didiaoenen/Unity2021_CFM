using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Paths
{
    public class TypeFinderUtils
    {
        private static List<Assembly> assemblies = new List<Assembly>();

        public static Type FindType(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
                return null;

            List<Assembly> assemblies = GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                Type type = assembly.GetType(typeName, false, false);
                if (type != null)
                    return type;
            }

            var name = string.Format(".{0}", typeName);
            foreach (Assembly assembly in assemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.FullName.EndsWith(name))
                        return type;
                }
            }

            return null;
        }

        private static List<Assembly> GetAssemblies()
        {
            if (assemblies.Count > 0)
                return assemblies;

            Assembly[] listAssembly = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in listAssembly)
            {
                var name = assembly.FullName;
                if (Regex.IsMatch(name, "^((mscorlib)|(nunit)|(System)|(UnityEngine)|(Loxodon.Log))"))
                    continue;

                assemblies.Add(assembly);
            }
            return assemblies;
        }
    }
}

