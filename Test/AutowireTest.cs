using NUnit.Framework;
using Spring.Core.CDH.Autowire;
using Test.Dao.MyTable;
using Test.Service.HelloWorld;

namespace Test
{
    public class AutowireTest : ContextRegisterTest
    {
        [Test]
        public void Autowire()
        {
            var instance = new TestController();
            SpringAutowire.Autowire(instance);

            Assert.AreEqual("SayHello", instance.HelloWorld_Default.SayHello());
            Assert.AreEqual("hello", instance.HelloWorld_EN.SayHello());
            Assert.AreEqual("안녕하세요", instance.HelloWorld_KR.SayHello());
            Assert.AreEqual("こんにちは", instance.HelloWorld_JP.SayHello());
        }

        [Test]
        public void Autowire_AdoDaoSupport_SetDefault_AdoTemplateName()
        {
            var instance = new SetDefault_AdoTemplateName();
            SpringAutowire.Autowire(instance);

            Assert.AreEqual("SERVER=localhost;DATABASE=myTestDB;USER ID=myAccount;PASSWORD=myPassword", instance.MyTableDao.GetConnectionString());
        }

        [Test]
        public void Autowire_AdoDaoSupport_SetOther_AdoTemplateName()
        {
            var instance = new SetOther_AdoTemplateName();
            SpringAutowire.Autowire(instance);

            Assert.AreEqual("SERVER=192.168.0.101;DATABASE=myTestDB;USER ID=myAccount;PASSWORD=myPassword", instance.MyTableDao.GetConnectionString());
        }

        public class TestController
        {
            [Autowire]
            public IHelloWorldService HelloWorld_Default { get; set; }

            [Autowire(Type = (typeof(HelloWorldENService)))]
            public IHelloWorldService HelloWorld_EN { get; set; }

            [Autowire(Type = (typeof(HelloWorldKRService)))]
            public IHelloWorldService HelloWorld_KR { get; set; }

            [Autowire(Type = (typeof(HelloWorldJPService)))]
            public IHelloWorldService HelloWorld_JP { get; set; }
        }

        public class SetDefault_AdoTemplateName
        {
            [Autowire]
            public IMyTableDao MyTableDao { get; set; }
        }

        public class SetOther_AdoTemplateName
        {
            [Autowire]
            [AdoTemplateName("AdoTemplate2")]
            public IMyTableDao MyTableDao { get; set; }
        }
    }
}