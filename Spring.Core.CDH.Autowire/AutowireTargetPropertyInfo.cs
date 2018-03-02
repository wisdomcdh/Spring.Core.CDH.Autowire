using System;
using System.Reflection;

namespace Spring.Core.CDH.Autowire
{
    internal class AutowireTargetPropertyInfo
    {
        public AutowireTargetPropertyInfo Parent { get; private set; }
        public PropertyInfo PropertyInfo { get; private set; }
        public ObjectInfo ObjectInfo { get; private set; }

        public AutowireTargetPropertyInfo(PropertyInfo prop, AutowireTargetPropertyInfo parent)
        {
            Parent = parent;
            PropertyInfo = prop;
            ObjectInfo = new ObjectInfo(Parent?.ObjectInfo, PropertyInfo);
        }

        public AutowireTargetPropertyInfo(AutowireAttribute autowireAttribute, Type type)
        {
            ObjectInfo = new ObjectInfo(autowireAttribute, type);
        }
    }
}