using Spring.Core.CDH.Autowire;
using Spring.Core.CDH.Util;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Spring.Core.CDH
{
    internal class ObjectInfo
    {
        public string Id { get; private set; }
        public bool Singleton { get; private set; }
        public string Type { get; private set; }
        public ObjectInfo Parent { get; private set; }
        public Type ObjectType { get; private set; }
        public AutowireAttribute PropertyDefinedAutowireAttribute { get; private set; }
        public IList<ChangePropertyAttribute> PropertyDefinedChangePropertyAttributes { get; private set; }
        public PropertyAttribute[] ConfirmedPropertyAttributes { get; private set; }

        public ObjectInfo(ObjectInfo parent, PropertyInfo prop)
        {
            Parent = parent;
            PropertyDefinedAutowireAttribute = prop.GetAutowireAttribute();
            PropertyDefinedChangePropertyAttributes = prop.GetChangePropertyAttributes();
            ObjectType = prop.PropertyType.GetCreateInstanceType(PropertyDefinedAutowireAttribute);
            ConfirmedPropertyAttributes = ConfirmedPropertyAttributesGetter.GetConfirmedPropertyAttributes(this, prop);
            Type = ObjectType.GetShortAssemblyName();
            Singleton = PropertyDefinedAutowireAttribute.Singleton;
            Id = this.GetObjectId();
        }

        public ObjectInfo(AutowireAttribute autowireAttribute, Type objectType)
        {
            //PropertyDefinedAutowireAttribute = autowireAttribute;
            //ObjectAdoTemplateNameAttribute = objectType.GetAdoTemplateNameAttribute();
            //ObjectType = ObjectTypeUtil.GetObjectType(PropertyDefinedAutowireAttribute, objectType);
            //ChangeAdoTemplateAttributes = new List<ChangeAdoTemplateAttribute>();
            //ConfirmedPropertyAttributes = new List<PropertyAttribute>();

            //Type = ObjectTypeUtil.GetShortAssemblyName(ObjectType);
            //Singleton = PropertyDefinedAutowireAttribute.Singleton;
            //Id = ObjectIdUtil.GetObjectId(this);
        }

        //public Type GetObjectType()
        //{
        //    return ObjectType;
        //}

        //public string GetAdoTemplateName()
        //{
        //    var adoTemplateName = ObjectAdoTemplateNameAttribute?.AdoTemplateName ?? "AdoTemplate";
        //    var info = this;
        //    ChangeAdoTemplateAttribute attr;
        //    while (info != null)
        //    {
        //        attr = info.ChangeAdoTemplateAttributes.FirstOrDefault(t => adoTemplateName.Equals(t.OldName, StringComparison.OrdinalIgnoreCase));
        //        if (attr != null)
        //        {
        //            return attr.NewName;
        //        }
        //        info = info.Parent;
        //    }
        //    return adoTemplateName;
        //}

        //public string GetChangeWireName()
        //{
        //    IEnumerable<string> change = new List<string>();
        //    var info = this;
        //    while (info != null)
        //    {
        //        change = change.Union(info.PropertyAttributes.Select(t => t.ToString()))
        //            .Union(info.ChangeAdoTemplateAttributes.Select(t => t.ToString()));
        //        info = info.Parent;
        //    }
        //    return string.Join(",", change);
        //}

        //public string GetChangeWireContextName(string beforeContextName)
        //{
        //    var info = this;
        //    PropertyAttribute attr;
        //    while (info != null)
        //    {
        //        attr = info.PropertyAttributes.FirstOrDefault(t => t.Name == beforeContextName);
        //        if (attr != null)
        //        {
        //            return attr.Ref;
        //        }
        //        info = info.Parent;
        //    }
        //    return beforeContextName;
        //}

        //public bool HasMergeContextName()
        //{
        //    return !string.IsNullOrEmpty(PropertyDefinedAutowireAttribute.MergeBase);
        //}

        //public string GetMergeContextName()
        //{
        //    return ObjectAutowireAttribute.MergeBase;
        //}
    }
}