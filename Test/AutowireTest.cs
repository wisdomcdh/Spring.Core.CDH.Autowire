using NUnit.Framework;
using Spring.Core.CDH.Autowire;
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
    }
}