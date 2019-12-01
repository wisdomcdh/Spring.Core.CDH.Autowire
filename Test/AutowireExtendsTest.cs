using NUnit.Framework;
using Spring.Core.CDH;
using Spring.Core.CDH.Autowire;

namespace Test
{
    public class AutowireExtendsTest
    {
        [Test]
        public void GeCreateInstanceType_By_Interface_Naming_Rule()
        {
            var prop = typeof(TestClass).GetProperty(nameof(TestClass.NamedInterface));
            AutowireAttribute autowireAttr = new AutowireAttribute();
            var type = prop.PropertyType.GetCreateInstanceType(autowireAttr);
            Assert.AreEqual(typeof(NamedInterface), type);
        }

        [Test]
        public void GeCreateInstanceType_By_AutowireAttribute()
        {
            var prop = typeof(TestClass).GetProperty(nameof(TestClass.NamedInterface));
            AutowireAttribute autowireAttr = new AutowireAttribute() { Type = typeof(MyNamedClass) };
            var type = prop.PropertyType.GetCreateInstanceType(autowireAttr);
            Assert.AreEqual(typeof(MyNamedClass), type);
        }

        [Test]
        public void IsInheritOfAdoDaoSupport_Is_SpringDataCode_AdoDaoSupport()
        {
            Assert.IsTrue(typeof(SpringDataCoreDao).IsInheritOfAdoDaoSupport());
        }

        [Test]
        public void IsInheritOfAdoDaoSupport_Is_SpringDataGeneric_AdoDaoSupport()
        {
            Assert.IsTrue(typeof(SpringDataGenericDao).IsInheritOfAdoDaoSupport());
        }

        public class TestClass
        {
            public INamedInterface NamedInterface { get; set; }
        }

        public interface INamedInterface { }

        public class NamedInterface : INamedInterface { }

        public class MyNamedClass : INamedInterface { }

        public class SpringDataCoreDao : Spring.Data.Core.AdoDaoSupport
        {
        }

        public class SpringDataGenericDao : Spring.Data.Generic.AdoDaoSupport
        {
        }
    }
}