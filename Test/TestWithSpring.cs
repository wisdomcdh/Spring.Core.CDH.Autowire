using NUnit.Framework;
using Spring.Core.CDH.Autowire;
using System;
using System.IO;

namespace Test
{
    [TestFixture]
    public abstract class TestWithSpring
    {
        protected string conn { get; private set; } = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=TestDB.mdf;Integrated Security=True";
        protected string conn2 { get; private set; } = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=TestDB2.mdf;Integrated Security=True";
        protected string conn3 { get; private set; } = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=TestDB3.mdf;Integrated Security=True";
        protected string conn4 { get; private set; } = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=TestDB4.mdf;Integrated Security=True";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            string contextFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SpringAppContext.xml");
            ContextRegister.RegisterContextFromPath(contextFilePath);
        }
    }
}