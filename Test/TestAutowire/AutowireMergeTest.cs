using NUnit.Framework;
using Spring.Core.CDH.Autowire;

namespace Test
{
    internal class AutowireMergeTest : TestWithSpring
    {
        [Test]
        public void Test()
        {
            var instance = (AutowireMergeTestClass)SpringAutowire.Autowire(typeof(AutowireMergeTestClass), new AutowireAttribute());
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
        [Autowire(ContextName = "String1")]
        public string Str1 { get; set; }

        [Autowire(ContextName = "String2")]
        public string Str2 { get; set; }
    }
}