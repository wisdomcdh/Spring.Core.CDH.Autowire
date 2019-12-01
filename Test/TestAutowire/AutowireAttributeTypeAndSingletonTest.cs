using NUnit.Framework;
using Spring.Core.CDH.Autowire;
using Test.Controllers;

namespace Test
{
    /// <summary>
    /// Autowire특성의 타입과, 싱글톤여부에 따른 테스트
    /// </summary>
    public class AutowireAttributeTypeAndSingletonTest : TestWithSpring
    {
        [Test]
        public void Autowire_Singleton은기본값true를갖는다_따라서가져오는인스턴스는서로같다()
        {
            var instance = (AutowireAttributeSimpleTestController)SpringAutowire.Autowire(typeof(AutowireAttributeSimpleTestController), new AutowireAttribute { Singleton = true });
            var instance2 = (AutowireAttributeSimpleTestController)SpringAutowire.Autowire(typeof(AutowireAttributeSimpleTestController), new AutowireAttribute());
            Assert.AreEqual(instance.Now, instance2.Now);
            Assert.AreSame(instance, instance2);
            Assert.AreSame(instance.HelloWorld_Default, instance2.HelloWorld_Default);
            Assert.AreSame(instance.HelloWorld_EN, instance2.HelloWorld_EN);
            Assert.AreSame(instance.HelloWorld_KR, instance2.HelloWorld_KR);
            Assert.AreSame(instance.HelloWorld_JP, instance2.HelloWorld_JP);
        }

        [Test]
        public void Autowire_유형이갖더라도Singleton조정할때_가져오는인스턴스는서로다르다()
        {
            var instance = (AutowireAttributeSimpleTestController)SpringAutowire.Autowire(typeof(AutowireAttributeSimpleTestController), new AutowireAttribute { Singleton = false });
            var instance2 = (AutowireAttributeSimpleTestController)SpringAutowire.Autowire(typeof(AutowireAttributeSimpleTestController), new AutowireAttribute());
            Assert.AreNotEqual(instance.Now, instance2.Now);
            Assert.AreNotSame(instance, instance2);
            Assert.AreSame(instance.HelloWorld_Default, instance2.HelloWorld_Default);
            Assert.AreSame(instance.HelloWorld_EN, instance2.HelloWorld_EN);
            Assert.AreSame(instance.HelloWorld_KR, instance2.HelloWorld_KR);
            Assert.AreSame(instance.HelloWorld_JP, instance2.HelloWorld_JP);
        }

        [Test]
        public void Test_AutowireAttributeSimpleTestController_i2()
        {
            var instance = (IAutowireAttributeSimpleTestController)SpringAutowire.Autowire(typeof(IAutowireAttributeSimpleTestController), new AutowireAttribute() { Type = typeof(AutowireAttributeSimpleTestController), Singleton = true });
            var instance2 = (IAutowireAttributeSimpleTestController)SpringAutowire.Autowire(typeof(IAutowireAttributeSimpleTestController), new AutowireAttribute() { Type = typeof(AutowireAttributeSimpleTestController) });
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
            var instance = (IAutowireAttributeSimpleTestController)SpringAutowire.Autowire(typeof(IAutowireAttributeSimpleTestController), new AutowireAttribute { Type = typeof(AutowireAttributeSimpleTestController), Singleton = false });
            var instance2 = (IAutowireAttributeSimpleTestController)SpringAutowire.Autowire(typeof(IAutowireAttributeSimpleTestController), new AutowireAttribute() { Type = typeof(AutowireAttributeSimpleTestController) });
            var instance3 = (IAutowireAttributeSimpleTestController)SpringAutowire.Autowire(typeof(IAutowireAttributeSimpleTestController), new AutowireAttribute() { Type = typeof(AutowireAttributeSimpleTestController) });
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