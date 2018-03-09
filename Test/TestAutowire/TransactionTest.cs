using NUnit.Framework;
using Spring.Aop.Framework.DynamicProxy;
using Spring.Context.Support;
using Spring.Core.CDH.Autowire;
using Test.Controllers;

namespace Test
{
    internal class TransactionTest : ContextRegisterTest
    {
        [Test]
        public void Test_AutowireAttributeSimpleTestController_1()
        {
            var instance = new TransactionController();
            var instance2 = ContextRegistry.GetContext().GetObject("tranTest");
            SpringAutowire.Autowire(instance);
            Assert.AreEqual(true, instance2 is BaseCompositionAopProxy);
            Assert.AreEqual(true, instance.TransactionMyTableService is BaseCompositionAopProxy);
        }
    }
}