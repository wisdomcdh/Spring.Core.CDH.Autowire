using Spring.Core.CDH.Autowire;
using System.Collections.Generic;
using System.Reflection;

namespace Spring.Core.CDH.Util
{
    internal static class DefaultPropertyAttributesCreator
    {
        public static IList<PropertyAttribute> CreateDefaultPropertyAttributes(PropertyInfo prop)
        {
            IList<PropertyAttribute> defaultProperties = new List<PropertyAttribute>();
            if (prop.PropertyType.IsInheritOfAdoDaoSupport())
            {
                defaultProperties.Add(new PropertyAttribute("AdoTemplate", "AdoTemplate"));
            }
            return defaultProperties;
        }
    }
}