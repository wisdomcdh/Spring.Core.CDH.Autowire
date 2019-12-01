using System;
using System.Reflection;

namespace Spring.Core.CDH.Autowire
{
    /// <summary>
    /// Autowire 대상정보 객체
    /// </summary>
    internal class AutowireTargetPropertyInfo
    {
        /// <summary>
        /// 부모정보
        /// 자신을 속성으로 포함하고 있는 부모의 정보
        /// </summary>
        public AutowireTargetPropertyInfo Parent { get; private set; }

        /// <summary>
        /// PropertyInfo
        /// </summary>
        public PropertyInfo PropertyInfo { get; private set; }

        /// <summary>
        /// ObjectInfo
        /// </summary>
        public ObjectInfo ObjectInfo { get; private set; }

        /// <summary>
        /// AutowireTargetPropertyInfo
        /// 속성으로 부터 정보를 가져올 때 사용합니다.
        /// </summary>
        /// <param name="prop">속성정보</param>
        /// <param name="parent">자신을 속성으로 포함하고 있는 부모의 정보</param>
        public AutowireTargetPropertyInfo(PropertyInfo prop, AutowireTargetPropertyInfo parent)
        {
            Parent = parent;
            PropertyInfo = prop;
            ObjectInfo = new ObjectInfo(Parent?.ObjectInfo, PropertyInfo);
        }

        /// <summary>
        /// AutowireTargetPropertyInfo
        /// 유형으로 부터 정보를 가져올 때 사용합니다.
        /// </summary>
        /// <param name="autowireAttribute"></param>
        /// <param name="type"></param>
        public AutowireTargetPropertyInfo(AutowireAttribute autowireAttribute, Type type)
        {
            ObjectInfo = new ObjectInfo(autowireAttribute, type);
        }
    }
}