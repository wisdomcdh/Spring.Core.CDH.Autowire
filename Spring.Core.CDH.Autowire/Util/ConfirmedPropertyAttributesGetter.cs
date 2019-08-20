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

            Extend(defaultAttrs, typeHasAttrs.Where(t => IsPropertyExists(info, t)).ToList(), propHasAttrs.Where(t => IsPropertyExists(info, t)).ToList());
            Merge(defaultAttrs, GetChangePropertyAttribute(info));

            return defaultAttrs.ToArray();
        }

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

        private static IList<PropertyAttribute> GetChangePropertyAttribute(ObjectInfo info)
        {
            IList<ChangePropertyAttribute> changeAttrs = new List<ChangePropertyAttribute>();
            while (info != null)
            {
                foreach (var attr in info.PropertyDefinedChangePropertyAttributes)
                {
                    if (!changeAttrs.Any(t => t.Same(attr)))
                    {
                        changeAttrs.Add(attr);
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