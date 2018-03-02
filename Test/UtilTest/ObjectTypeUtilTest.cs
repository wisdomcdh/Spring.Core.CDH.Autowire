using NUnit.Framework;
using Spring.Core.CDH.Autowire;
using Spring.Core.CDH.Util;
using Spring.Data.Generic;
using System;
using Test.Service.HelloWorld;

namespace Test
{
    internal class ObjectTypeUtilTest
    {
        [Test]
        public void GetObjectType()
        {
            Type type;
            type = ObjectTypeUtil.GetObjectType(new AutowireAttribute(), typeof(IHelloWorldService));
            Assert.AreEqual(typeof(HelloWorldService), type);

            type = ObjectTypeUtil.GetObjectType(new AutowireAttribute { Type = typeof(HelloWorldJPService) }, typeof(IHelloWorldService));
            Assert.AreEqual(typeof(HelloWorldJPService), type);

            type = ObjectTypeUtil.GetObjectType(new AutowireAttribute { }, typeof(doNotUseInterface));
            Assert.AreEqual(null, type);
        }

        [Test]
        public void GetShortAssemblyName()
        {
            string name = ObjectTypeUtil.GetShortAssemblyName(typeof(ObjectTypeUtilTest));
            Assert.AreEqual("Test.UtilTest.ObjectTypeUtilTest, Spring.CDH.Test", name);
        }

        [Test]
        public void IsInheritOfAdoDaoSupport()
        {
            var result = ObjectTypeUtil.IsInheritOfAdoDaoSupport(typeof(IsInheritOfAdoDaoSupportClass));
            Assert.AreEqual(true, result);
            result = ObjectTypeUtil.IsInheritOfAdoDaoSupport(typeof(doNotUseInterface));
            Assert.AreEqual(false, result);
        }

        public interface doNotUseInterface { }

        public class IsInheritOfAdoDaoSupportClass : AdoDaoSupport
        {
        }
    }
}