using NUnit.Framework;
using Spring.Core.CDH.Autowire;
using Test.Controllers;

namespace Test
{
    internal class AutowireAttributeSimpleTest : ContextRegisterTest
    {
        [Test]
        public void Test_AutowireAttributeSimpleTestController_1()
        {
            var instance = new AutowireAttributeSimpleTestController();
            SpringAutowire.Autowire(instance);

            Assert.AreEqual("SayHello", instance.HelloWorld_Default.SayHello());
            Assert.AreEqual("hello", instance.HelloWorld_EN.SayHello());
            Assert.AreEqual("안녕하세요", instance.HelloWorld_KR.SayHello());
            Assert.AreEqual("こんにちは", instance.HelloWorld_JP.SayHello());
        }

        [Test]
        public void Test_AutowireAttributeSimpleTestController_2()
        {
            var instance = (AutowireAttributeSimpleTestController)SpringAutowire.Autowire(typeof(AutowireAttributeSimpleTestController), new AutowireAttribute());
            var instance2 = (AutowireAttributeSimpleTestController)SpringAutowire.Autowire(typeof(AutowireAttributeSimpleTestController), new AutowireAttribute());
            Assert.AreEqual(instance.Now, instance2.Now);
            Assert.AreSame(instance, instance2);
            Assert.AreSame(instance.HelloWorld_Default, instance2.HelloWorld_Default);
            Assert.AreSame(instance.HelloWorld_EN, instance2.HelloWorld_EN);
            Assert.AreSame(instance.HelloWorld_KR, instance2.HelloWorld_KR);
            Assert.AreSame(instance.HelloWorld_JP, instance2.HelloWorld_JP);
        }

        [Test]
        public void Test_AutowireAttributeSimpleTestController_i2()
        {
            var instance = (IAutowireAttributeSimpleTestController)SpringAutowire.Autowire(typeof(IAutowireAttributeSimpleTestController), new AutowireAttribute());
            var instance2 = (IAutowireAttributeSimpleTestController)SpringAutowire.Autowire(typeof(IAutowireAttributeSimpleTestController), new AutowireAttribute());
            Assert.AreEqual(instance.Now, instance2.Now);
            Assert.AreSame(instance, instance2);
            Assert.AreSame(instance.HelloWorld_Default, instance2.HelloWorld_Default);
            Assert.AreSame(instance.HelloWorld_EN, instance2.HelloWorld_EN);
            Assert.AreSame(instance.HelloWorld_KR, instance2.HelloWorld_KR);
            Assert.AreSame(instance.HelloWorld_JP, instance2.HelloWorld_JP);
        }

        [Test]
        public void Test_AutowireAttributeSimpleTestController_3()
        {
            var instance = (AutowireAttributeSimpleTestController)SpringAutowire.Autowire(typeof(AutowireAttributeSimpleTestController), new AutowireAttribute { Singleton = false });
            var instance2 = (AutowireAttributeSimpleTestController)SpringAutowire.Autowire(typeof(AutowireAttributeSimpleTestController), new AutowireAttribute());
            var instance3 = (AutowireAttributeSimpleTestController)SpringAutowire.Autowire(typeof(AutowireAttributeSimpleTestController), new AutowireAttribute());
            Assert.AreNotEqual(instance.Now, instance2.Now);
            Assert.AreNotSame(instance, instance2);
            Assert.AreSame(instance.HelloWorld_Default, instance2.HelloWorld_Default);
            Assert.AreSame(instance.HelloWorld_EN, instance2.HelloWorld_EN);
            Assert.AreSame(instance.HelloWorld_KR, instance2.HelloWorld_KR);
            Assert.AreSame(instance.HelloWorld_JP, instance2.HelloWorld_JP);

            Assert.AreEqual(instance2.Now, instance3.Now);
            Assert.AreSame(instance2, instance3);
            Assert.AreSame(instance2.HelloWorld_Default, instance3.HelloWorld_Default);
            Assert.AreSame(instance2.HelloWorld_EN, instance3.HelloWorld_EN);
            Assert.AreSame(instance2.HelloWorld_KR, instance3.HelloWorld_KR);
            Assert.AreSame(instance2.HelloWorld_JP, instance3.HelloWorld_JP);
        }

        [Test]
        public void Test_AutowireAttributeSimpleTestController_i3()
        {
            var instance = (IAutowireAttributeSimpleTestController)SpringAutowire.Autowire(typeof(IAutowireAttributeSimpleTestController), new AutowireAttribute { Singleton = false });
            var instance2 = (IAutowireAttributeSimpleTestController)SpringAutowire.Autowire(typeof(IAutowireAttributeSimpleTestController), new AutowireAttribute());
            var instance3 = (IAutowireAttributeSimpleTestController)SpringAutowire.Autowire(typeof(IAutowireAttributeSimpleTestController), new AutowireAttribute());
            Assert.AreNotEqual(instance.Now, instance2.Now);
            Assert.AreNotSame(instance, instance2);
            Assert.AreSame(instance.HelloWorld_Default, instance2.HelloWorld_Default);
            Assert.AreSame(instance.HelloWorld_EN, instance2.HelloWorld_EN);
            Assert.AreSame(instance.HelloWorld_KR, instance2.HelloWorld_KR);
            Assert.AreSame(instance.HelloWorld_JP, instance2.HelloWorld_JP);

            Assert.AreEqual(instance2.Now, instance3.Now);
            Assert.AreSame(instance2, instance3);
            Assert.AreSame(instance2.HelloWorld_Default, instance3.HelloWorld_Default);
            Assert.AreSame(instance2.HelloWorld_EN, instance3.HelloWorld_EN);
            Assert.AreSame(instance2.HelloWorld_KR, instance3.HelloWorld_KR);
            Assert.AreSame(instance2.HelloWorld_JP, instance3.HelloWorld_JP);
        }
    }
}