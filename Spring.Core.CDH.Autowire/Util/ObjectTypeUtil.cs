using Spring.Core.CDH.Autowire;
using System;
using System.Linq;

namespace Spring.Core.CDH.Util
{
    internal static class ObjectTypeUtil
    {
        /// <summary>
        /// AutowireAttribute 와 이 특성이 정의된 속성의 Type을 분석하여 실제 주입대상 Type을 찾는다.
        /// </summary>
        /// <param name="autowireAttribute"></param>
        /// <param name="ownedType">주입대상 객체의 소유자의 타입</param>
        /// <returns>주입대상 실제 Type</returns>
        public static Type GetObjectType(AutowireAttribute autowireAttribute, Type ownedType)
        {
            if (autowireAttribute.Type == null)
            {
                if (ownedType.IsInterface)
                {
                    // interface 이며 AutowireAttribute 에 Type이 지정되지 않은경우 해당 interface의 위치에서 'I' 를 뺀 클래스를 찾아 리턴한다.
                    var likelyType = Type.GetType($"{ownedType.Namespace}.{string.Concat(ownedType.Name.Skip(1))}, {ownedType.Assembly.FullName}");
                    if (likelyType != null)
                    {
                        if (ownedType.IsAssignableFrom(likelyType))
                        {
                            return likelyType;
                        }
                    }
                    return null;
                }
                else
                {
                    return ownedType;
                }
            }
            else
            {
                return autowireAttribute.Type;
            }
        }

        /// <summary>
        /// AdoDaoSupport를 상속받는지 확인 합니다.
        /// </summary>
        /// <param name="type">Type</param>
        public static bool IsInheritOfAdoDaoSupport(Type type)
        {
            while (type != null)
            {
                if (type?.FullName == SpringAutowire.AdoDaoSupportFullName) return true;
                type = type.BaseType;
            }
            return false;
        }
    }
}