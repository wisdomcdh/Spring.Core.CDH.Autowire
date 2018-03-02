using Spring.Core.CDH.Autowire;
using System;
using System.Linq;

namespace Spring.Core.CDH.Util
{
    internal static class ObjectTypeUtil
    {
        /// <summary>
        /// 주입대상 Type
        /// AutowireAttribute 특성이 선언된 Type을 분석하여 실제 주입대상 Type을 찾는다.
        /// </summary>
        /// <param name="autowireAttribute"></param>
        /// <param name="ownedType">AutowireAttribute 특성 소유자의 타입</param>
        /// <returns>주입대상 Type</returns>
        public static Type GetObjectType(AutowireAttribute autowireAttribute, Type ownedType)
        {
            // 특성에 타입지정이 되어 있으면 그것을 리턴
            if (autowireAttribute.Type != null)
            {
                return autowireAttribute.Type;
            }
            else
            {
                // 특성 소유자의 타입이 인터페이스인 경우 소유자의 namespace 에서 실제 주입대상 Type을 찾는다.
                if (ownedType.IsInterface)
                {
                    // 1. interface의 이름이 i 로 시작하는 경우 i를 제외한 이름의 대상을 찾는다.
                    if (ownedType.Name.StartsWith("i", StringComparison.OrdinalIgnoreCase))
                    {
                        Type like = Type.GetType($"{ownedType.Namespace}.{string.Concat(ownedType.Name.Skip(1))}, {ownedType.Assembly.FullName}");
                        if (like != null && ownedType.IsAssignableFrom(like))
                        {
                            return like;
                        }
                    }
                    return null;
                }
                else
                {
                    // 인터페이스가 아닌경우 소유자의 타입 그대로 리턴
                    return ownedType;
                }
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

        public static string GetShortAssemblyName(Type type)
        {
            return $"{type.FullName}, {type.Assembly.GetName().Name}";
        }
    }
}