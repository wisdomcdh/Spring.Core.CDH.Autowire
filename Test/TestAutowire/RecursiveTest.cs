using NUnit.Framework;
using Spring.Core.CDH.Autowire;
using Test.Service.MyTable;

namespace Test
{
    internal class RecursiveTest : TestWithSpring
    {
        [Test]
        public void Test()
        {
            var instance = (IRecursiveMyTableService)SpringAutowire.Autowire(typeof(IRecursiveMyTableService), new AutowireAttribute());

            Assert.AreSame(instance, instance.Recursive_MyTableService);
            Assert.AreSame(instance, instance.Recursive_MyTableService.Recursive_MyTableService);
            Assert.AreEqual(base.conn, instance.MyTableDao.GetConnectionString());
            Assert.AreEqual(base.conn2, instance.MyTableDao2.GetConnectionString());
        }

        [Test]
        public void Test2()
        {
            var instance = (IRecursiveMyTableService2)SpringAutowire.Autowire(typeof(IRecursiveMyTableService2), new AutowireAttribute());

            Assert.AreNotSame(instance, instance.Recursive_MyTableService2);
            Assert.AreNotSame(instance, instance.Recursive_MyTableService2.Recursive_MyTableService2);
            Assert.AreSame(instance.Recursive_MyTableService2, instance.Recursive_MyTableService2.Recursive_MyTableService2);
            Assert.AreEqual(base.conn, instance.MyTableDao.GetConnectionString());
            Assert.AreEqual(base.conn3, instance.Recursive_MyTableService2.MyTableDao.GetConnectionString());
        }
    }
}