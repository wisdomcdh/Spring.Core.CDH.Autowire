using Spring.Core.CDH.Autowire;
using Spring.Core.CDH.Util;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Spring.Core.CDH
{
    internal class ObjectInfo
    {
        public string id { get; private set; }
        public bool singleton { get; private set; }
        public string type { get { return ObjectType.AssemblyQualifiedName; } }
        protected ObjectInfo Parent { get; private set; }
        protected AutowireAttribute ObjectAutowireAttribute { get; private set; }
        protected Type ObjectType { get; private set; }
        protected IList<ChangeAdoTemplateAttribute> ChangeAdoTemplateAttributes { get; private set; }
        protected IList<ChangeWireAttribute> ChangeWireAttributes { get; private set; }

        public ObjectInfo(ObjectInfo parent, PropertyInfo prop)
        {
            Parent = parent;
            ObjectAutowireAttribute = prop.GetAutowireAttribute();
            ObjectType = ObjectTypeUtil.GetObjectType(ObjectAutowireAttribute, prop.PropertyType);
            ChangeAdoTemplateAttributes = prop.GetChangeAdoTemplateAttributes();
            ChangeWireAttributes = prop.GetChangeWireAttributes();
        }

        public ObjectInfo(AutowireAttribute autowireAttribute, Type objectType)
        {
            ObjectAutowireAttribute = autowireAttribute;
            ObjectType = ObjectTypeUtil.GetObjectType(ObjectAutowireAttribute, objectType);
            ChangeAdoTemplateAttributes = new List<ChangeAdoTemplateAttribute>();
            ChangeWireAttributes = new List<ChangeWireAttribute>();
        }

        //public ObjectInfo(ObjectInfo parent, object @class)
        //{
        //    Parent = parent;
        //    type = ObjectTypeUtil.GetObjectType(null, @class.GetType());

        //}

        //public ObjectInfo(AutowireAttribute autowireAttribute,
        //    Type ownedType,
        //    IList<DIchangeAttribute> diChangeArrtList,
        //    ObjectInfo ownedObjectInfo)
        //{
        //    Parent = ownedObjectInfo;
        //    OwnedType = ownedType;
        //    AutowireAttribute = autowireAttribute;
        //    this.diChangeArrtList = diChangeArrtList;

        //    SetObjectTypeAndName();
        //    SetObjectId();
        //}

        //private void SetObjectTypeAndName()
        //{
        //    ObjectType = ObjectTypeUtil.GetObjectType(AutowireAttribute, OwnedType);
        //    ObjectTypeName = ObjectType.AssemblyQualifiedName;
        //}

        //private void SetObjectId()
        //{
        //    ObjectId = ObjectIdUtil.GetObjectId(AutowireAttribute, ObjectType, GetAllDichangeAttrList(this));
        //}

        //private IList<DIchangeAttribute> GetAllDichangeAttrList(ObjectInfo objectInfo)
        //{
        //    IList<DIchangeAttribute> allList = new List<DIchangeAttribute>();
        //    DIchangeAttribute attr;
        //    while (objectInfo != null)
        //    {
        //        for (int i = 0; i < objectInfo.diChangeArrtList.Count; i++)
        //        {
        //            attr = diChangeArrtList[i];
        //            if (!allList.Any(t => t.Before == attr.Before)) allList.Add(attr);
        //        }
        //        objectInfo = objectInfo.Parent;
        //    }
        //    return allList;
        //}
    }
}