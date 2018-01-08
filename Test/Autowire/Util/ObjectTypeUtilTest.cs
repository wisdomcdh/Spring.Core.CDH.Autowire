using NUnit.Framework;
using Spring.Core.CDH.Autowire;
using Spring.Core.CDH.Util;
using System;
using System.Collections;

namespace Test.Autowire.Util
{
    [TestFixture]
    public class ObjectTypeUtilTest
    {
        [Test, TestCaseSource(typeof(ObjectTypeUtilTestCase), "Cases")]
        public Type GetObjectType(AutowireAttribute attr, Type ownedType)
        {
            return ObjectTypeUtil.GetObjectType(attr, ownedType);
        }

        public class ObjectTypeUtilTestCase
        {
            private static Type AnyType = typeof(object);

            public static IEnumerable Cases
            {
                get
                {
                    // Type 이 지정되어 있으면 그 Type을 반환한다.
                    yield return new TestCaseData(
                        new AutowireAttribute
                        {
                            Type = typeof(string)
                        },
                        AnyType
                        )
                    {
                        ExpectedResult = typeof(string),
                        TestName = "Type 이 지정되어 있으면 그 Type을 반환한다.",
                    };

                    // Type 이 지정되어 있지 않으면, 소유자(PropertyType 등) 의 전달된 Type 으로 반환한다.
                    // 단, 소유자가 interface 가 아니면 그대로 소유자 타입을 리턴한다.
                    yield return new TestCaseData(
                        new AutowireAttribute
                        {
                            Type = null
                        },
                        typeof(TestOwnedClass)
                        )
                    {
                        ExpectedResult = typeof(TestOwnedClass),
                        TestName = "Type 이 지정되어 있지 않고 소유자가 인터페이스가 아닌 경우.",
                    };

                    // Type 이 지정되어 있지 않으면, 소유자(PropertyType 등) 의 전달된 Type 으로 반환한다.
                    // 단, 소유자가 interface 이면 interface와 같은 namespace에 위치한 접두어 'I'를 class 를 찾아 반환한다.
                    yield return new TestCaseData(
                        new AutowireAttribute
                        {
                            Type = null
                        },
                        typeof(ITestOwnedClass)
                        )
                    {
                        ExpectedResult = typeof(TestOwnedClass),
                        TestName = "Type 이 지정되어 있지 않고 소유자 인터페이스를 사용하는 타입을 찾은 경우.",
                    };

                    // Type 이 지정되어 있지 않으면, 소유자(PropertyType 등) 의 전달된 Type 으로 반환한다.
                    // 단, 소유자가 interface 이면 interface와 같은 namespace에 위치한 접두어 'I'를 class 를 찾아 반환한다.
                    // 찾지 못하면 null을 반환한다.
                    yield return new TestCaseData(
                        new AutowireAttribute
                        {
                            Type = null
                        },
                        typeof(INothing)
                        )
                    {
                        ExpectedResult = null,
                        TestName = "Type 이 지정되어 있지 않고 소유자 인터페이스를 사용하는 타입을 찾지 못할 경우.",
                    };
                }
            }
        }
    }

    public class TestOwnedClass : ITestOwnedClass
    {
        public void Do()
        {
        }
    }

    public interface ITestOwnedClass
    {
        void Do();
    }

    public interface INothing { }
}