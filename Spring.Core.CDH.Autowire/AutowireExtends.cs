using Spring.Core.CDH.Autowire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Spring.Core.CDH
{
    internal static class AutowireExtends
    {
        public static bool IsAttributeDefined<T>(this MemberInfo element)
        {
            return Attribute.IsDefined(element, typeof(T), true);
        }

        public static AutowireAttribute GetAutowireAttribute(this MemberInfo element)
        {
            return Attribute.GetCustomAttribute(element, typeof(AutowireAttribute)) as AutowireAttribute;
        }

        public static IList<ChangeAdoTemplateAttribute> GetChangeAdoTemplateAttributes(this MemberInfo element)
        {
            return Attribute.GetCustomAttributes(element, typeof(ChangeAdoTemplateAttribute)).Cast<ChangeAdoTemplateAttribute>().ToList();
        }

        public static IList<ChangeWireAttribute> GetChangeWireAttributes(this MemberInfo element)
        {
            return Attribute.GetCustomAttributes(element, typeof(ChangeWireAttribute)).Cast<ChangeWireAttribute>().ToList();
        }
    }
}