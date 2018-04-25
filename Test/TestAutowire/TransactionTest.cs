using NUnit.Framework;
using Spring.Aop.Framework.DynamicProxy;
using Spring.Context.Support;
using Spring.Core.CDH.Autowire;
using System;
using Test.Controllers;

namespace Test
{
    internal class TransactionTest : ContextRegisterTest
    {
        public TransactionController TransactionController;

        [SetUp]
        public void SetUp()
        {
            TransactionController = (TransactionController)SpringAutowire.Autowire(typeof(TransactionController), new AutowireAttribute());
        }

        [Test]
        public void Test_AutowireAttributeSimpleTestController_1()
        {
            var instance2 = ContextRegistry.GetContext().GetObject("tranTest");
            Assert.AreEqual(true, instance2 is BaseCompositionAopProxy);
            Assert.AreEqual(true, TransactionController.TransactionMyTableService is BaseCompositionAopProxy);
        }

        [Test]
        public void Insert()
        {
            TransactionController.TransactionMyTableService.TestInsert(new Dao.MyTable.Model.MyTableItem
            {
                Name = "wisdomcd",
                Age = 14
            });

            var a = TransactionController.TransactionMyTableService.MyTableDao.GetAll();
        }
    }
}