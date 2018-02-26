using Spring.Core.CDH.Autowire;
using System;

namespace Spring.Core.CDH.Util
{
    internal static class ObjectIdUtil
    {
        public static string GetObjectId(ObjectInfo info)
        {
            AutowireAttribute autowireAttribute = info.GetAutowireAttribute();
            if (!string.IsNullOrEmpty(autowireAttribute.ContextName))
            {
                return autowireAttribute.ContextName;
            }
            else
            {
                Type objectType = info.GetObjectType();
                string name = ObjectTypeUtil.GetShortAssemblyName(objectType);

                if (ObjectTypeUtil.IsInheritOfAdoDaoSupport(objectType))
                {
                    name += $"[AdoDaoSupport,{info.GetAdoTemplateName()}]";
                }
                else
                {
                    name += $"[ChangeWire,{info.GetChangeWireName()}]";
                }
                return name;
            }
        }
    }
}