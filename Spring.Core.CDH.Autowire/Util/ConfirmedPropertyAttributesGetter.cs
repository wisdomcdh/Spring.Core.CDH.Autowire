using Spring.Core.CDH.Autowire;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Spring.Core.CDH.Util
{
    internal static class ConfirmedPropertyAttributesGetter
    {
        public static PropertyAttribute[] GetConfirmedPropertyAttributes(ObjectInfo info, PropertyInfo prop)
        {
            var defaultAttrs = GetDefaultPropertyAttribues(info);
            var typeHasAttrs = info.ObjectType.GetPropertyAttributes();
            var propHasAttrs = prop.GetPropertyAttributes();
            var changeAttrs = GetUseableChangePropertyAttribute(info);

            Extend(defaultAttrs, typeHasAttrs.Where(t => IsPropertyExists(info, t)).ToList(), propHasAttrs.Where(t => IsPropertyExists(info, t)).ToList());
            Merge(defaultAttrs, changeAttrs);

            return defaultAttrs.ToArray();
        }

        /// <summary>
        /// PropertyAttribute 를 선언하지 않더라도 주입되어야 할 기본 속성을 가져 옵니다.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private static IList<PropertyAttribute> GetDefaultPropertyAttribues(ObjectInfo info)
        {
            IList<PropertyAttribute> defaultProperties = new List<PropertyAttribute>();
            if (info.ObjectType.IsInheritOfAdoDaoSupport())
            {
                defaultProperties.Add(new PropertyAttribute("AdoTemplate", "AdoTemplate"));
            }
            return defaultProperties;
        }

        private static IList<PropertyAttribute> Extend(IList<PropertyAttribute> desc, params IList<PropertyAttribute>[] sources)
        {
            foreach (var source in sources)
            {
                foreach (var attr in source)
                {
                    if (desc.Any(t => t.Same(attr)))
                    {
                        var index = desc.FindIndex(t => t.Same(attr));
                        desc[index] = attr;
                    }
                    else
                    {
                        desc.Add(attr);
                    }
                }
            }
            return desc;
        }

        private static IList<PropertyAttribute> Merge(IList<PropertyAttribute> desc, params IList<PropertyAttribute>[] sources)
        {
            foreach (var source in sources)
            {
                foreach (var attr in source)
                {
                    if (desc.Any(t => t.Same(attr)))
                    {
                        var index = desc.FindIndex(t => t.Same(attr));
                        desc[index] = attr;
                    }
                }
            }
            return desc;
        }

        private static IList<PropertyAttribute> GetUseableChangePropertyAttribute(ObjectInfo info)
        {
            PropertyInfo find;
            IList<ChangePropertyAttribute> changeAttrs = new List<ChangePropertyAttribute>();
            while (info != null)
            {
                foreach (var attr in info.PropertyDefinedChangePropertyAttributes)
                {
                    find = info.ObjectType.GetProperty(attr.Name);
                    if (attr.Type == null || attr.Type.Equals(find.PropertyType))
                    {
                        if (!changeAttrs.Any(t => t.Same(attr)))
                        {
                            changeAttrs.Add(attr);
                        }
                    }
                }
                info = info.Parent;
            }
            return changeAttrs.OfType<PropertyAttribute>().ToList();
        }

        private static bool IsPropertyExists(ObjectInfo info, PropertyAttribute propAttr)
        {
            var findProp = info.ObjectType.GetProperty(propAttr.Name);
            if (findProp != null)
            {
                if (propAttr.Type != null)
                {
                    return findProp.PropertyType.Equals(propAttr.Type);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}