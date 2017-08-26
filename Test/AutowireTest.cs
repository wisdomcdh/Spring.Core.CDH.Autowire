using NUnit.Framework;
using Spring.Context;
using Spring.Context.Support;
using Spring.Core.CDH.Autowire;
using Spring.Data.Generic;
using System;
using System.IO;

namespace Test
{
    [TestFixture]
    public class AutowireTest
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SpringContext.xml");
            ContextRegistry.Clear();
            IApplicationContext context = new XmlApplicationContext(false, path);
            ContextRegistry.RegisterContext(context);
        }

        [Test]
        public void Test()
        {
            var myMvcController = new MyMvcController();

            SpringAutowire.Autowire(myMvcController);
        }
    }

    public class MyMvcController
    {
        [Autowire(Type = typeof(MyService1))]
        public IMyService MyService1 { get; set; }

        [Autowire(Type = typeof(MyService2))]
        public IMyService MyService2 { get; set; }

        [Autowire]
        public MyDao MyDao { get; set; }

        [Autowire]
        public MyDao2 MyDao2 { get; set; }
    }

    public interface IMyService { }

    public class MyService1 : IMyService
    {
        public string ValueByReferenceName { get; set; }
    }

    public class MyService2 : IMyService { }

    /// <summary>
    /// AdoDaoSupprot Default
    /// <para>
    /// [PropertyValue("AdoTemplate", ObjectReferenceName = "AdoTemplate")]
    /// </para>
    /// </summary>
    public class MyDao : AdoDaoSupport
    {
    }

    public class MyDao2 : AdoDaoSupport
    {
    }
}