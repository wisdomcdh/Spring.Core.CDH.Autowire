using Spring.Core.CDH.Autowire;
using Spring.Core.CDH.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Spring.Core.CDH
{
    internal class ObjectInfo
    {
        public string id { get; private set; }
        public bool singleton { get; private set; }
        public string type { get; private set; }
        protected ObjectInfo Parent { get; private set; }
        protected AutowireAttribute ObjectAutowireAttribute { get; private set; }
        protected AdoTemplateNameAttribute ObjectAdoTemplateNameAttribute { get; private set; }
        protected Type ObjectType { get; private set; }
        protected IList<ChangeAdoTemplateAttribute> ChangeAdoTemplateAttributes { get; private set; }
        protected IList<ChangeWireAttribute> ChangeWireAttributes { get; private set; }

        public ObjectInfo(ObjectInfo parent, PropertyInfo prop)
        {
            Parent = parent;
            ObjectAutowireAttribute = prop.GetAutowireAttribute();
            ObjectAdoTemplateNameAttribute = prop.GetAdoTemplateNameAttribute();
            ObjectType = ObjectTypeUtil.GetObjectType(ObjectAutowireAttribute, prop.PropertyType);
            ChangeAdoTemplateAttributes = prop.GetChangeAdoTemplateAttributes();
            ChangeWireAttributes = prop.GetChangeWireAttributes();

            id = ObjectIdUtil.GetObjectId(this);
            type = ObjectTypeUtil.GetShortAssemblyName(ObjectType);
            singleton = ObjectAutowireAttribute.Singleton;
        }

        public ObjectInfo(AutowireAttribute autowireAttribute, Type objectType)
        {
            ObjectAutowireAttribute = autowireAttribute;
            ObjectAdoTemplateNameAttribute = objectType.GetAdoTemplateNameAttribute();
            ObjectType = ObjectTypeUtil.GetObjectType(ObjectAutowireAttribute, objectType);
            ChangeAdoTemplateAttributes = new List<ChangeAdoTemplateAttribute>();
            ChangeWireAttributes = new List<ChangeWireAttribute>();

            id = ObjectIdUtil.GetObjectId(this);
            type = ObjectTypeUtil.GetShortAssemblyName(ObjectType);
            singleton = ObjectAutowireAttribute.Singleton;
        }

        public AutowireAttribute GetAutowireAttribute()
        {
            return ObjectAutowireAttribute;
        }

        public Type GetObjectType()
        {
            return ObjectType;
        }

        public string GetAdoTemplateName()
        {
            var adoTemplateName = ObjectAdoTemplateNameAttribute?.AdoTemplateName ?? "AdoTemplate";
            var change = ChangeAdoTemplateAttributes.FirstOrDefault(t => adoTemplateName.Equals(t.Before, StringComparison.OrdinalIgnoreCase));
            if (change != null)
            {
                return change.After;
            }
            else
            {
                return adoTemplateName;
            }
        }

        public string GetChangeWireName()
        {
            IEnumerable<string> change = new List<string>();
            var info = this;
            while (info != null)
            {
                change = change.Union(info.ChangeWireAttributes.Select(t => t.ToString()));
                info = info.Parent;
            }
            return string.Join(",", change);
        }
    }
}