using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

namespace mvvm
{
    public static class PropertyPathAccessors
    {
        class PathComparer : IEqualityComparer<PropertyInfo[]>
        {
            public bool Equals(PropertyInfo[] x, PropertyInfo[] y)
            {
                if (x.Length != y.Length) return false;
                for (var i = 0; i < x.Length; i++)
                    if (x[i] != y[i]) return false;
                return true;
            }

            public int GetHashCode(PropertyInfo[] obj)
            {
                unchecked
                {
                    var hash = 17;

                    for (var i = 0; i < obj.Length; i++)
                    {
                        var item = obj[i];
                        hash = hash * 23 + ((item != null) ? item.GetHashCode() : 0);
                    }

                    return hash;
                }
            }

        }

        public static readonly Func<object, object> NoGetter = o => null;

        public static readonly Action<object, object> NoSetter = (o, o1) => { };

        public static readonly IEqualityComparer<PropertyInfo[]> Comparer = new PathComparer();

        public static readonly BindingFlags Flags = BindingFlags.Public | BindingFlags.Instance;

        private static readonly Dictionary<PropertyInfo[], Func<object, object>> Getters = new Dictionary<PropertyInfo[], Func<object, object>>(Comparer);
        private static readonly Dictionary<PropertyInfo[], Action<object, object>> Setters = new Dictionary<PropertyInfo[], Action<object, object>>(Comparer);

        static bool _initialzed;

        public static void Register(PropertyInfo[] prop, Func<object, object> getter, Action<object, object> setter)
        {
            Getters[prop] = getter;
            Setters[prop] = setter;
        }

        internal static bool ValidateGetter(PropertyInfo[] path, ref Func<object, object> getter)
        {
            if (!_initialzed) return false;

            if (getter != null)
                return getter != NoGetter;

            if (Getters.TryGetValue(path, out getter))
                return true;
            getter = NoGetter;
            return false;
        }

        internal static bool ValidateSetter(PropertyInfo[] path, ref Action<object, object> setter)
        {
            if (!_initialzed) return false;

            if (setter != null)
                return setter != NoSetter;

            if (Setters.TryGetValue(path, out setter))
                return true;
            setter = NoSetter;
            return false;
        }

        public static void initialize()
        {
            _initialzed = true;
        }
    }
}
