using NUnit.Framework;
using Spring.Core.CDH.Autowire;
using Test.Dao.MyTable;
using Test.Service.HelloWorld;
using Test.Service.MyTable;
using Test.Service.TestModules;

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

            Assert.AreEqual(base.conn, instance.MyTableDao.GetConnectionString());
        }

        [Test]
        public void Autowire_AdoDaoSupport_SetOther_AdoTemplateName()
        {
            var instance = new SetOther_AdoTemplateName();
            SpringAutowire.Autowire(instance);

            Assert.AreEqual(base.conn2, instance.MyTableDao.GetConnectionString());
        }

        [Test]
        public void Autowire_AdoDaoSupport_SetChange_AdoTemplateName()
        {
            var instance = new SetChange_AdoTemplateName();
            SpringAutowire.Autowire(instance);

            Assert.AreEqual(base.conn3, instance.MyTableDao.GetConnectionString());
        }

        [Test]
        public void Autowire_AdoDaoSupport_SetChange_AdoTemplateName_Recursive_Tree_Struct()
        {
            /*
             TestController_Recursive
                - MyTableService2
                    - MyTableDao [AdoTemplate:conn]
                    - MyTableDao2 [AdoTemplate2:conn2]
                    - MyTableService
                        - MyTableDao [AdoTemplate:conn]
                        - MyTableDao2 [AdoTemplate2:conn2]
                    - MyTableService_AdoTemplateChange (AdoTemplate -> AdoTemplate3)
                        - MyTableDao [AdoTempldate3:conn3]
                        - MyTableDao2 [AdoTemplate2:conn2]
                    - MyTableService2_Recursive_AdoTemplateChange (AdoTemplate, AdoTemplate3-> AdoTemplate2)
                        - MyTableDao [AdoTemplate2:conn2]
                        - MyTableDao2 [AdoTemplate2:conn2]
                        - MyTableService
                            - MyTableDao [AdoTemplate3:conn3]
                            - MyTableDao2 [AdoTemplate2:conn2]
                        - MyTableService_AdoTemplateChange (AdoTemplate -> AdoTemplate3)
                            - MyTableDao [AdoTempldate3:conn3]
                            - MyTableDao2 [AdoTemplate2:conn2]
                        - MyTableService2_Recursive_AdoTemplateChange (AdoTemplate, AdoTemplate3-> AdoTemplate2)
                            - Recursive...
             */

            var instance = new TestController_Recursive();
            SpringAutowire.Autowire(instance);

            Assert.AreEqual(base.conn, instance.MyTableService2.MyTableDao.GetConnectionString());
            Assert.AreEqual(base.conn2, instance.MyTableService2.MyTableDao2.GetConnectionString());
            // MyTableService
            Assert.AreEqual(base.conn, instance.MyTableService2.MyTableService.MyTableDao.GetConnectionString());
            Assert.AreEqual(base.conn2, instance.MyTableService2.MyTableService.MyTableDao2.GetConnectionString());
            // MyTableService_AdoTemplateChange
            Assert.AreEqual(base.conn3, instance.MyTableService2.MyTableService_AdoTemplateChange.MyTableDao.GetConnectionString());
            Assert.AreEqual(base.conn2, instance.MyTableService2.MyTableService_AdoTemplateChange.MyTableDao2.GetConnectionString());
            // MyTableService2_Recursive_AdoTemplateChange
            Assert.AreEqual(base.conn2, instance.MyTableService2.MyTableService2_Recursive_AdoTemplateChange.MyTableDao.GetConnectionString());
            Assert.AreEqual(base.conn2, instance.MyTableService2.MyTableService2_Recursive_AdoTemplateChange.MyTableDao2.GetConnectionString());
            Assert.AreEqual(base.conn2, instance.MyTableService2.MyTableService2_Recursive_AdoTemplateChange.MyTableService.MyTableDao.GetConnectionString());
            Assert.AreEqual(base.conn2, instance.MyTableService2.MyTableService2_Recursive_AdoTemplateChange.MyTableService.MyTableDao2.GetConnectionString());
            Assert.AreEqual(base.conn3, instance.MyTableService2.MyTableService2_Recursive_AdoTemplateChange.MyTableService_AdoTemplateChange.MyTableDao.GetConnectionString());
            Assert.AreEqual(base.conn2, instance.MyTableService2.MyTableService2_Recursive_AdoTemplateChange.MyTableService_AdoTemplateChange.MyTableDao2.GetConnectionString());
        }

        internal class TestController
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

        internal class SetDefault_AdoTemplateName
        {
            [Autowire]
            public IMyTableDao MyTableDao { get; set; }
        }

        internal class SetOther_AdoTemplateName
        {
            [Autowire]
            [AdoTemplateName("AdoTemplate2")]
            public IMyTableDao MyTableDao { get; set; }
        }

        internal class SetChange_AdoTemplateName
        {
            [Autowire]
            [AdoTemplateName("AdoTemplate2")]
            [DIchange("AdoTemplate2", "AdoTemplate3")]
            public IMyTableDao MyTableDao { get; set; }
        }

        internal class TestController_Recursive
        {
            [Autowire]
            public IMyTableService2 MyTableService2 { get; set; }
        }
    }
}