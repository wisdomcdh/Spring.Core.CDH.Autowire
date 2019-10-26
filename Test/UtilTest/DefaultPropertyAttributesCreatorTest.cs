using NUnit.Framework;
using Spring.Core.CDH.Util;
using Spring.Data.Generic;

namespace Test.UtilTest
{
    public class DefaultPropertyAttributesCreatorTest
    {
        [Test]
        public void CreateDefaultPropertyAttributes_When_PropertyType_Is_AdoDaoSupport()
        {
            var prop = typeof(TestClass).GetProperty(nameof(TestClass.IsDaoProperty));
            var expectAttr = new Spring.Core.CDH.Autowire.PropertyAttribute("AdoTemplate", "AdoTemplate");

            var result = DefaultPropertyAttributesCreator.CreateDefaultPropertyAttributes(prop);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(true, expectAttr.SameRules(result[0]));

            var prop2 = typeof(TestClass).GetProperty(nameof(TestClass.IsDaoProperty2));
            result = DefaultPropertyAttributesCreator.CreateDefaultPropertyAttributes(prop2);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(true, expectAttr.SameRules(result[0]));
        }

        [Test]
        public void CreateDefaultPropertyAttributes_When_PropertyType_IsNot_AdoDaoSupport()
        {
            var prop = typeof(TestClass).GetProperty(nameof(TestClass.IsNotDaoProperty));
            var result = DefaultPropertyAttributesCreator.CreateDefaultPropertyAttributes(prop);
            Assert.AreEqual(0, result.Count);
        }

        public class TestClass
        {
            public TestDao1 IsDaoProperty { get; set; }
            public TestDao2 IsDaoProperty2 { get; set; }
            public string IsNotDaoProperty { get; set; }

            public class TestDao1 : AdoDaoSupport { }

            public class TestDao2 : Spring.Data.Core.AdoDaoSupport { }
        }
    }
}