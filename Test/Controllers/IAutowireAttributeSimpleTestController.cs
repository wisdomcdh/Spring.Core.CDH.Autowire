using System;
using Test.Service.HelloWorld;

namespace Test.Controllers
{
    internal interface IAutowireAttributeSimpleTestController
    {
        IHelloWorldService HelloWorld_Default { get; set; }
        IHelloWorldService HelloWorld_EN { get; set; }
        IHelloWorldService HelloWorld_JP { get; set; }
        IHelloWorldService HelloWorld_KR { get; set; }
        DateTime Now { get; }
    }
}