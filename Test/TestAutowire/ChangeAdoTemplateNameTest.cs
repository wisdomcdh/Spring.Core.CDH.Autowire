using NUnit.Framework;
using Spring.Core.CDH.Autowire;
using Test.Controllers;

namespace Test
{
    internal class ChangeAdoTemplateNameTest : TestWithSpring
    {
        [Test]
        public void Test_AutowireAttributeSimpleTestController_1()
        {
            var instance = new AdoTemplateChangeTestController();
            SpringAutowire.Autowire(instance);

            Assert.AreEqual(base.conn, instance.MyTableService.MyTableDao.GetConnectionString());
            Assert.AreEqual(base.conn2, instance.MyTableService.MyTableDao2.GetConnectionString());
            Assert.AreEqual(base.conn2, instance.MyTableService2.MyTableDao.GetConnectionString());
            Assert.AreEqual(base.conn3, instance.MyTableService2.MyTableDao2.GetConnectionString());

            Assert.AreEqual(base.conn, instance.MyTableDao.GetConnectionString());
            Assert.AreEqual(base.conn2, instance.MyTableDao2.GetConnectionString());
            Assert.AreEqual(base.conn3, instance.MyTableDao3.GetConnectionString());
            Assert.AreEqual(base.conn4, instance.MyTableDao4.GetConnectionString());
        }
    }
}