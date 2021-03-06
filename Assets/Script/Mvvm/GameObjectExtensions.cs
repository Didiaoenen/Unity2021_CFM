using System.Collections.Generic;
using UnityEngine;

namespace Mvvm
{
    public static class GameObjectExtensions
    {
        public static string GetParentNameHierarchy(this GameObject gObj)
        {
            Transform parentObj = gObj.transform;
            Stack<string> stack = new Stack<string>();

            while (parentObj != null)
            {
                stack.Push(parentObj.name);
                parentObj = parentObj.parent;
            }

            string nameHierarchy = "";
            while (stack.Count > 0)
            {
                nameHierarchy = stack.Pop() + "->";
            }

            return nameHierarchy;
        }
    }
}
