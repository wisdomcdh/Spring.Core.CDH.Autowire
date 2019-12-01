using NUnit.Framework;
using Spring.Core.CDH.Autowire;

namespace Test
{
    /// <summary>
    /// Context 구성파일로 부터 이미 등록된 내용을 병합할 때, 테스트
    /// </summary>
    public class AutowireMergBaseeTest : TestWithSpring
    {
        [Test]
        public void Autowire_Context구성파일에미리정의하고_MergeBase가선언된Autowire하면_구성파일을베이스로하여병합하며Autowired한다()
        {
            // Arrange
            /*
             * xml 구성파일에 AutowireMergeTestClass의 정의부분이 이미 존재한다.
             */

            // Act
            var instance = (AutowireMergeTestClass)SpringAutowire.Autowire(typeof(AutowireMergeTestClass));

            // Assert
            Assert.AreEqual("MERGE_STR1", instance.AutowireMergeTestInnerClass.Str1);
            Assert.AreEqual("Text2", instance.AutowireMergeTestInnerClass.Str2);
        }
    }

    public class AutowireMergeTestClass
    {
        [Autowire(MergeBase = "MERGE_AutowireMergeTestInnerClass")]
        public AutowireMergeTestInnerClass AutowireMergeTestInnerClass { get; set; }
    }

    public class AutowireMergeTestInnerClass
    {
        public string Str1 { get; set; }

        [Autowire(ContextName = "String2")]
        public string Str2 { get; set; }
    }
}