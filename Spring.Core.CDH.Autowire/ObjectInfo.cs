using Spring.Core.CDH.Autowire;
using Spring.Core.CDH.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Spring.Core.CDH
{
    internal class ObjectInfo
    {
        public Type ObjectType { get; private set; }
        public string ObjectTypeName { get; private set; }
        public string ObjectId { get; private set; }
        public bool IsSingleton { get; private set; }

        /// <summary>
        /// 주입대상 객체의 소유자의 타입
        /// <para>Property의 소유객체 타입</para>
        /// <para>Property가 아닌, 즉각 생성이면(ControllerFactory 등에서의) typeof(Object) 가 된다. </para>
        /// </summary>
        protected Type OwnedType { get; private set; }

        protected AutowireAttribute AutowireAttribute { get; private set; }
        protected AdoTemplateAttribute
        protected IList<DIchangeAttribute> diChangeArrtList { get; private set; }
        protected ObjectInfo Parent { get; private set; }

        public ObjectInfo(AutowireAttribute autowireAttribute,
            Type ownedType,
            IList<DIchangeAttribute> diChangeArrtList,
            ObjectInfo ownedObjectInfo)
        {
            Parent = ownedObjectInfo;
            OwnedType = ownedType;
            AutowireAttribute = autowireAttribute;
            this.diChangeArrtList = diChangeArrtList;

            SetObjectTypeAndName();
            SetObjectId();
        }

        private void SetObjectTypeAndName()
        {
            ObjectType = ObjectTypeUtil.GetObjectType(AutowireAttribute, OwnedType);
            ObjectTypeName = ObjectType.AssemblyQualifiedName;
        }

        private void SetObjectId()
        {
            ObjectId = ObjectIdUtil.GetObjectId(AutowireAttribute, ObjectType, GetAllDichangeAttrList(this));
        }

        private IList<DIchangeAttribute> GetAllDichangeAttrList(ObjectInfo objectInfo)
        {
            IList<DIchangeAttribute> allList = new List<DIchangeAttribute>();
            DIchangeAttribute attr;
            while (objectInfo != null)
            {
                for (int i = 0; i < objectInfo.diChangeArrtList.Count; i++)
                {
                    attr = diChangeArrtList[i];
                    if (!allList.Any(t => t.Before == attr.Before)) allList.Add(attr);
                }
                objectInfo = objectInfo.Parent;
            }
            return allList;
        }
    }
}