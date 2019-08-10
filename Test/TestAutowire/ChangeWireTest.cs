using NUnit.Framework;
using Spring.Core.CDH.Autowire;

namespace Test
{
    internal class ChangeWireTest : ContextRegisterTest
    {
        [Test]
        public void Test()
        {
            var instance = (ChangeWireTestClass)SpringAutowire.Autowire(typeof(ChangeWireTestClass), new AutowireAttribute());
            Assert.AreEqual("Text1", instance.Str1);
            Assert.AreEqual("Text2", instance.Str2);
            Assert.AreEqual("ChangeText1", instance.TestD.Str1);
            Assert.AreEqual("ChangeText1", instance.TestD.TestD.Str1);
        }
    }

    public class ChangeWireTestClass
    {
        [Autowire(ContextName = "String1")]
        public string Str1 { get; set; }

        [Autowire(ContextName = "String2")]
        public string Str2 { get; set; }

        [Autowire]
        [Spring.Core.CDH.Autowire.Property("String1", "ChangeString")]
        public ChangeWireTestClass TestD { get; set; }
    }
}