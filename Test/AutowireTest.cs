using NUnit.Framework;
using Spring.Context;
using Spring.Context.Support;
using Spring.Core.CDH.Autowire;
using Spring.Data.Generic;
using Spring.Transaction.Interceptor;
using System;
using System.IO;

namespace Test
{
    public class AutowireTest
    {
        [SetUp]
        public void OneTimeSetup()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SpringContext.xml");
            ContextRegistry.Clear();
            IApplicationContext context = new XmlApplicationContext(false, path);
            ContextRegistry.RegisterContext(context);
        }

        [Autowire]
        public ITestService injectTestService { get; set; }

        [Autowire]
        public ITestDao injectTestDao { get; set; }

        [Autowire]
        [InjectionAdoTemplate("AdoTemplate2")]
        public ITestDao injectTestDao2 { get; set; }

        [Autowire(Type = typeof(TestDao2))]
        [InjectionProperty("AdoTemplate", "AdoTemplate")]
        public ITestDao injectTestDao3 { get; set; }

        [Autowire]
        public ITestService2 injectTestService2 { get; set; }

        [Test]
        public void Test()
        {
            SpringAutowire.Autowire(this);
        }

        [Test]
        public void Test2()
        {
            ITestServiceTran svc = new TestServiceTran();
            SpringAutowire.Autowire(svc);
        }

        public interface ITestDao { void TestFunction(); }

        public class TestDao : AdoDaoSupport, ITestDao
        {
            public void TestFunction()
            {
            }
        }

        public class TestDao2 : ITestDao
        {
            public object AdoTemplate { get; set; }

            public void TestFunction()
            {
            }
        }

        public interface ITestService { void TestFunction(); }

        public interface ITestService2 { }

        public interface ITestService3 { }

        public class TestService : ITestService
        {
            [Autowire]
            public ITestDao TestDao { get; set; }

            public void TestFunction()
            {
                TestDao.TestFunction();
            }
        }

        public class TestService2 : ITestService2
        {
            [Autowire]
            public ITestService3 TestService3 { get; set; }
        }

        public class TestService3 : ITestService3
        {
            [Autowire]
            public ITestService2 TestService2 { get; set; }
        }

        public interface ITestServiceTran { void Tran(); }

        public class TestServiceTran : ITestServiceTran
        {
            [Autowire]
            public ITestDao TestDao { get; set; }

            [Transaction]
            public void Tran()
            {
            }
        }
    }
}