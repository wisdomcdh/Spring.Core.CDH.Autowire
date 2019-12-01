using NUnit.Framework;
using Spring.Core.CDH.Autowire;

namespace Test
{
    public class AutowireContextNameTest : TestWithSpring
    {
        [Test]
        public void Autowire_Context구성파일에미리정의하고_ContextName가선언된Autowire하면_구성파일의내용으로만Autowired한다()
        {
            // Arrange
            /*
             * xml 구성파일에 AutowireMergeTestClass의 정의부분이 이미 존재한다.
             */

            // Act
            var instance = (AutowireContextNameTestClass)SpringAutowire.Autowire(typeof(AutowireContextNameTestClass));

            // Assert
            Assert.AreEqual("DEFINED_STR1", instance.AutowireContextNameTestInnerClass.Str1);
            Assert.AreEqual("DEFINED_STR2", instance.AutowireContextNameTestInnerClass.Str2);
        }
    }

    public class AutowireContextNameTestClass
    {
        [Autowire(ContextName = "DEFINED_AutowireContextNameTestInnerClass")]
        public AutowireContextNameTestInnerClass AutowireContextNameTestInnerClass { get; set; }
    }

    public class AutowireContextNameTestInnerClass
    {
        public string Str1 { get; set; }

        [Autowire(ContextName = "Not working")]
        public string Str2 { get; set; }
    }
}