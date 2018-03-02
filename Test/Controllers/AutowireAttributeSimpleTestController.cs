using Spring.Core.CDH.Autowire;
using System;
using Test.Service.HelloWorld;

namespace Test.Controllers
{
    internal class AutowireAttributeSimpleTestController : IAutowireAttributeSimpleTestController
    {
        [Autowire]
        public IHelloWorldService HelloWorld_Default { get; set; }

        [Autowire(Type = (typeof(HelloWorldENService)))]
        public IHelloWorldService HelloWorld_EN { get; set; }

        [Autowire(Type = (typeof(HelloWorldKRService)))]
        public IHelloWorldService HelloWorld_KR { get; set; }

        [Autowire(Type = (typeof(HelloWorldJPService)))]
        public IHelloWorldService HelloWorld_JP { get; set; }

        public DateTime Now { get; } = DateTime.Now;
    }
}