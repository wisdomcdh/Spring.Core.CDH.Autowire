using Spring.Core.CDH.Autowire;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Spring.Core.CDH.Util
{
    internal static class ObjectIdUtil
    {
        public static string GetObjectId(AutowireAttribute autowireAttribute, Type objectType, IList<DIchangeAttribute> diChangeAttrList)
        {
            if (!string.IsNullOrEmpty(autowireAttribute.ContextName))
            {
                return autowireAttribute.ContextName;
            }
            else
            {
                var name = objectType.FullName;
                if (ObjectTypeUtil.IsInheritOfAdoDaoSupport(objectType))
                {

                }
                else
                {
                    if (diChangeAttrList.Count > 0)
                    {
                        name += $"[diChangeAttrs({string.Join(",", diChangeAttrList.Select(t => t.ToString()))})]";
                    }
                }

                //

                // AdoTemplate 명...
                /*
             상위로 올라가며 ChangeList를 찾아 내려온다 (아래에서 위로 역으로 올라가서 리버스)
             위로 올라가도 없으면

             */

                return name;
            }
        }
    }
}