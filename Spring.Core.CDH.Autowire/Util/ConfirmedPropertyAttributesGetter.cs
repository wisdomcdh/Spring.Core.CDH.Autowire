using Spring.Core.CDH.Autowire;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Spring.Core.CDH.Util
{
    internal static class ConfirmedPropertyAttributesGetter
    {
        /// <summary>
        /// 주입대상 객체에 정의될 확인된 속성을 가져온다.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static PropertyAttribute[] GetConfirmedPropertyAttributes(ObjectInfo info, PropertyInfo prop)
        {
            List<PropertyAttribute> defaultAttrs = GetDefaultPropertyAttribues(info);
            IEnumerable<PropertyAttribute> typeHasAttrs = GetTypeHasPropertyAttributes(info);
            IEnumerable<PropertyAttribute> propHasAttrs = GetPropertyHasPropertyAttributes(prop);

            ExtendToDefaultAttrs(info, defaultAttrs, typeHasAttrs, propHasAttrs);

            List<ChangePropertyRefAttribute> changeRefAttrs = GetUseableChangePropertyAttribute(info);

            ChangePropertyAttributeByChangeRefAttrs(defaultAttrs, changeRefAttrs);

            return defaultAttrs.ToArray();
        }

        /// <summary>
        /// PropertyAttribute 를 선언하지 않더라도 객체에 정의되어야 할 기본 속성을 가져 옵니다.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private static List<PropertyAttribute> GetDefaultPropertyAttribues(ObjectInfo info)
        {
            /*
             * AdoDaoSupport 지원객체를 상속받는 객체의 AdoTemplate 속성을 Property특성 선언없이 사용한다.
             * ADO.NET을 기본지원하는 이 곳은 나중에 설정 객체로 분리어 여러 환경에서 처리가능하도록 한다.
             */
            List<PropertyAttribute> defaultProperties = new List<PropertyAttribute>();
            if (info.ObjectType.IsInheritOfAdoDaoSupport())
            {
                defaultProperties.Add(new PropertyAttribute("AdoTemplate", "AdoTemplate"));
            }
            return defaultProperties;
        }

        /// <summary>
        ///  주입대상 객체에 선언된 Property 특성을 가져온다.
        ///  (interface가 아닌 주입될 class에 선언된 특성)
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private static IEnumerable<PropertyAttribute> GetTypeHasPropertyAttributes(ObjectInfo info)
        {
            return info.ObjectType.GetPropertyAttributes();
        }

        /// <summary>
        /// 주입대상 객체가 어느 객체의 속성으로 있는경우, 그 속성에 정의된 Property 특성을 가져온다.
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        private static IList<PropertyAttribute> GetPropertyHasPropertyAttributes(PropertyInfo prop)
        {
            return prop?.GetPropertyAttributes().ToList() ?? new List<PropertyAttribute>();
        }

        /// <summary>
        /// 주입대상 객체의 속성들이 선언된 특성과 달리 자신을 소유한 부모들로 부터 강제적으로 변경되어야 하는 정보인 Property특성을 가져온다.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private static List<ChangePropertyRefAttribute> GetUseableChangePropertyAttribute(ObjectInfo info)
        {
            List<ChangePropertyRefAttribute> changeAttrs = new List<ChangePropertyRefAttribute>();
            int depth = 0, findIndex;
            ObjectInfo checkInfo = info;
            while (checkInfo != null)
            {
                foreach (var attr in checkInfo.PropertyDefinedChangePropertyAttributes)
                {
                    if (!attr.DisallowSpread || (attr.DisallowSpread && depth == 0))
                    {
                        // 적용 우선순위를 판단하여 유일한 특성 하나만을 가져온다.
                        findIndex = changeAttrs.FindIndex(t => t.SameRules(attr));
                        if (findIndex > -1)
                        {
                            changeAttrs[findIndex] = attr;
                        }
                        else
                        {
                            changeAttrs.Add(attr);
                        }
                    }
                }
                checkInfo = checkInfo.Parent;
                depth--;
            }
            return changeAttrs;
        }

        /// <summary>
        /// 전달된 기본 Property 특성 리스트에 타입과, 프로퍼티에 정의된 Property특성을 확장한다.
        /// </summary>
        /// <param name="defaultAttrs"></param>
        /// <param name="sources"></param>
        /// <returns></returns>
        private static void ExtendToDefaultAttrs(ObjectInfo info, List<PropertyAttribute> defaultAttrs, IEnumerable<PropertyAttribute> typeHasAttrs, IEnumerable<PropertyAttribute> propHasAttrs)
        {
            // 객체에 정의된 특성을 우선 병합 후, 부모객체의 속성에 정의된 특성을 병합한다.
            // 따라서 Union의 순서를 주의해야한다.
            int findIndex;
            foreach (var attr in typeHasAttrs.Union(propHasAttrs).Where(t => IsPropertyExists(info, t)))
            {
                findIndex = defaultAttrs.FindIndex(t => t.SameRules(attr));
                if (findIndex > -1)
                {
                    defaultAttrs[findIndex] = attr;
                }
                else
                {
                    defaultAttrs.Add(attr);
                }
            }
        }

        /// <summary>
        /// Property특성을 설정된 규칙에 의해 강제 변경한다.
        /// </summary>
        /// <param name="desc"></param>
        /// <param name="sources"></param>
        /// <returns></returns>
        private static void ChangePropertyAttributeByChangeRefAttrs(List<PropertyAttribute> defaultAttrs, List<ChangePropertyRefAttribute> changeRefAttrs)
        {
            foreach (var attr in changeRefAttrs)
            {
                foreach (var findIndex in defaultAttrs.Select((t, index) => attr.SameRules(t) ? index : -1)
                                                     .Where(index => index > -1)
                                                     .ToArray())
                {
                    defaultAttrs[findIndex] = attr;
                }
            }
        }

        private static bool IsPropertyExists(ObjectInfo info, PropertyAttribute propAttr)
        {
            return info.ObjectType.GetProperty(propAttr.Name) != null;
        }
    }
}