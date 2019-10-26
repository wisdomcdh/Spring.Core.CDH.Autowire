using Spring.Core.CDH.Autowire;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Spring.Core.CDH
{
    internal static class AutowireTargetPropertyGetter
    {
        public static IEnumerable<AutowireTargetPropertyInfo> FindProperties(Type type, AutowireTargetPropertyInfo parent)
        {
            return type.GetProperties()
                .Where(prop => prop.IsAttributeDefined<AutowireAttribute>())
                .Select(prop => new AutowireTargetPropertyInfo(prop, parent));
        }
    }
}