using NUnit.Framework;
using Spring.Core.CDH.Autowire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Test.Autowire
{
    [TestFixture]
    public class AutowireTargetInfoUtilTest
    {
        /*
         
        속성은 Autowire 하고
        DaoSupprot 의 
         
         */

        [Test]
        public void Test()
        {
            //    var a = new ASDF();
            //    var type = a.GetType();
            //    var attrA = type.GetProperty("A").GetCustomAttribute<AutowireAttribute>();
            //    attrA.ContextName = "12345";
            //    var attrB = type.GetProperty("B").GetCustomAttribute<AutowireAttribute>();
            //    attrB.ContextName = "12345";

            //    attrA = type.GetProperty("A").GetCustomAttribute<AutowireAttribute>();
            //    attrB = type.GetProperty("B").GetCustomAttribute<AutowireAttribute>();

            var c = typeof(ASDF).GetProperty("B").PropertyType.GetProperty("C");


        }

    }

    public class ASDF
    {
        [Autowire]
        public AAAA A { get; set; }

        [Autowire]
        public FFFF B { get; set; }
    }

    public class AAAA
    {
    }

    public class FFFF
    {
        [Autowire]
        public AAAA C { get; set; }
    }
}
