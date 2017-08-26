using System;
using System.Reflection;

namespace Spring.Core.CDH
{
    internal static class AutowireExtends
    {
        public static bool IsAttributeDefined<T>(this MemberInfo element)
        {
            return Attribute.IsDefined(element, typeof(T), true);
        }
    }
}