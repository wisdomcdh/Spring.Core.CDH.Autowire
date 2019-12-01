using Spring.Core.CDH.Autowire;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Spring.Core.CDH
{
    /// <summary>
    /// Autowire 대상 속성을 판별하여, <see cref="AutowireTargetPropertyInfo"/> 열거하여 가져 옵니다.
    /// </summary>
    internal static class AutowireTargetPropertyGetter
    {
        /// <summary>
        /// Autowie 대상 정보를 가져 옵니다.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static IEnumerable<AutowireTargetPropertyInfo> GetAutowireTargetPropertiesInfo(Type type, AutowireTargetPropertyInfo parent)
        {
            return type.GetProperties()
                .Where(prop => prop.IsAttributeDefined<AutowireAttribute>())
                .Select(prop => new AutowireTargetPropertyInfo(prop, parent));
        }
    }
}