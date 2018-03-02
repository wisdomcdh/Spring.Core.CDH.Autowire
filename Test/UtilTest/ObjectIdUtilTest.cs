using NUnit.Framework;
using Spring.Core.CDH;
using Spring.Core.CDH.Autowire;
using Spring.Core.CDH.Util;
using Test.Service.MyTable;

namespace Test
{
    internal class ObjectIdUtilTest
    {
        [Test]
        public void GetObjectId_1()
        {
            var service = new MyTableService();
            var propertyInfo = service.GetType().GetProperty("MyTableDao");
            var info = new ObjectInfo(null, propertyInfo);
            var name = ObjectIdUtil.GetObjectId(info);
            Assert.AreEqual("[True]Test.Dao.MyTable.MyTableDao, Spring.CDH.Test[AdoDaoSupport,AdoTemplate]", name);
        }

        [Test]
        public void GetObjectId_2()
        {
            var info = new ObjectInfo(new AutowireAttribute { Singleton = false }, typeof(MyTableService));
            var name = ObjectIdUtil.GetObjectId(info);
            Assert.AreEqual("[False]Test.Service.MyTable.MyTableService, Spring.CDH.Test[ChangeWire,]", name);
        }

        [Test]
        public void GetObjectId_3()
        {
            var info = new ObjectInfo(new AutowireAttribute { Singleton = false, Type = typeof(MyTableService) }, typeof(IMyTableService));
            var name = ObjectIdUtil.GetObjectId(info);
            Assert.AreEqual("[False]Test.Service.MyTable.MyTableService, Spring.CDH.Test[ChangeWire,]", name);
        }
    }
}