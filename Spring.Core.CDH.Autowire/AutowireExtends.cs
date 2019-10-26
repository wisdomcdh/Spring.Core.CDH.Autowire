using Spring.Core.CDH.Autowire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Spring.Core.CDH
{
    internal static class AutowireExtends
    {
        public static bool IsAttributeDefined<T>(this MemberInfo element)
        {
            return Attribute.IsDefined(element, typeof(T), true);
        }

        public static bool IsAutowirePropertyDefined(this Type type, string name)
        {
            return type.GetProperty(name)?.IsAttributeDefined<AutowireAttribute>() ?? false;
        }

        public static AutowireAttribute GetAutowireAttribute(this MemberInfo element)
        {
            return element.GetCustomAttribute<AutowireAttribute>();
        }

        public static ChangePropertyRefAttribute[] GetChangePropertyAttributes(this MemberInfo element)
        {
            return element.GetCustomAttributes<ChangePropertyRefAttribute>().ToArray();
        }

        public static IEnumerable<PropertyAttribute> GetPropertyAttributes(this MemberInfo element)
        {
            return element.GetCustomAttributes<PropertyAttribute>();
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
            if (!string.IsNullOrEmpty(info.PropertyDefinedAutowireAttribute.ContextName))
            {
                return info.PropertyDefinedAutowireAttribute.ContextName;
            }
            else
            {
                if (!info.FromProperty)
                {
                    return info.Type;
                }
                else
                {
                    return string.Concat("[", info.Type, "]",
                        GetMD5String(string.Join(string.Empty, info.PropertyDefinedChangePropertyAttributes.Union(info.ConfirmedPropertyAttributes).Select(t => t.ToString()))));
                }
            }
        }

        private static string GetMD5String(string str)
        {
            byte[] resultArr;
            using (var md5 = new MD5CryptoServiceProvider())
            {
                resultArr = md5.ComputeHash(Encoding.Default.GetBytes(str));
            }
            return string.Join(string.Empty, resultArr.Select(t => t.ToString("X2")));
        }
    }
}