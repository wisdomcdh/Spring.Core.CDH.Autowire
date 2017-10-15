using NUnit.Framework;
using Spring.Core.CDH.Autowire;
using Test.Dao.MyTable;
using Test.Service.HelloWorld;
using Test.Service.MyTable;
using Test.Service.TestModules;

namespace Test
{
    public class AutowireTest2 : ContextRegisterTest
    {
        public void Autowire_IOC()
        {
            //SpringAutowire.Autowire()
            //var a = new TestController
        }

        internal class TestController2
        {
            [Autowire]
            public TestModule Test1 { get; set; }

            [Autowire]
            public TestModule Test2 { get; set; }
        }

        internal class TestController3
        {
            [Autowire]
            public TestModule Test1 { get; set; }

            [Autowire]
            public TestModule Test3 { get; set; }
        }
    }
}