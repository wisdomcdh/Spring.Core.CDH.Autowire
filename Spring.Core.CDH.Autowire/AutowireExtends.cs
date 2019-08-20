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
            return element.GetCustomAttribute<AutowireAttribute>();
        }

        public static IList<ChangePropertyAttribute> GetChangePropertyAttributes(this MemberInfo element)
        {
            return element.GetCustomAttributes<ChangePropertyAttribute>().ToList();
        }

        public static IList<PropertyAttribute> GetPropertyAttributes(this MemberInfo element)
        {
            return element.GetCustomAttributes<PropertyAttribute>().ToList();
        }

        public static int FindIndex<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (items == null) throw new ArgumentNullException("items");
            if (predicate == null) throw new ArgumentNullException("predicate");

            int retVal = 0;
            foreach (var item in items)
            {
                if (predicate(item)) return retVal;
                retVal++;
            }
            return -1;
        }

        public static bool IsInheritOfAdoDaoSupport(this Type type)
        {
            while (type != null)
            {
                if (type.FullName == "Spring.Data.Generic.AdoDaoSupport"
                    || type.FullName == "Spring.Data.Core.AdoDaoSupport")
                {
                    return true;
                }
                else
                {
                    type = type.BaseType;
                }
            }
            return false;
        }

        public static Type GetCreateInstanceType(this Type propertyType, AutowireAttribute autowireAttr)
        {
            if (autowireAttr.Type != null)
            {
                return autowireAttr.Type;
            }
            else
            {
                if (propertyType.IsInterface)
                {
                    if (propertyType.Name.StartsWith("i", StringComparison.OrdinalIgnoreCase))
                    {
                        string presumedFullName = $"{propertyType.FullName.Substring(0, propertyType.FullName.LastIndexOf(propertyType.Name))}{propertyType.Name.Substring(1)}, {propertyType.Assembly.FullName}";
                        Type presumedType = Type.GetType(presumedFullName);
                        return presumedType;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return propertyType;
                }
            }
        }

        public static string GetShortAssemblyName(this Type type)
        {
            return string.Concat(type.FullName, ", ", type.Assembly.GetName().Name);
        }

        public static string GetObjectId(this ObjectInfo info)
        {
            string basedName;
            if (string.IsNullOrEmpty(info.PropertyDefinedAutowireAttribute.ContextName))
            {
                basedName = info.Type;
            }
            else
            {
                basedName = info.PropertyDefinedAutowireAttribute.ContextName;
            }
            string changeStr = string.Join(string.Empty, info.PropertyDefinedChangePropertyAttributes.Select(t => t.ToString()));
            return string.Concat("[", basedName, "]", info.PropertyDefinedAutowireAttribute, changeStr);
        }
    }
}