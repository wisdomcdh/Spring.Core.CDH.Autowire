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

        public static AdoTemplateNameAttribute GetAdoTemplateNameAttribute(this MemberInfo element)
        {
            return Attribute.GetCustomAttribute(element, typeof(AdoTemplateNameAttribute)) as AdoTemplateNameAttribute;
        }

        public static IList<ChangeAdoTemplateAttribute> GetChangeAdoTemplateAttributes(this MemberInfo element)
        {
            return Attribute.GetCustomAttributes(element, typeof(ChangeAdoTemplateAttribute)).Cast<ChangeAdoTemplateAttribute>().ToList();
        }

        public static IList<PropertyAttribute> GetChangeWireAttributes(this MemberInfo element)
        {
            return Attribute.GetCustomAttributes(element, typeof(PropertyAttribute)).Cast<PropertyAttribute>().ToList();
        }
    }
}