using NUnit.Framework;
using Spring.Core.CDH.Autowire;

namespace Test
{
    [TestFixture]
    public class ContextRegisterTest
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            ContextRegister.RegisterContext(xmlContext);
        }

        [Test]
        public void RegisterContext()
        {
        }

        private string xmlContext = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<objects xmlns=""http://www.springframework.net""
         xmlns:db=""http://www.springframework.net/database""
         xmlns:tx=""http://www.springframework.net/tx"">
    <db:provider id=""DbProvider"" provider=""System.Data.SqlClient"" connectionString=""SERVER=localhost;DATABASE=myTestDB;USER ID=myAccount;PASSWORD=myPassword"" />
    <object id=""AdoTemplate"" type=""Spring.Data.Generic.AdoTemplate, Spring.Data"">
        <property name=""DbProvider"" ref=""DbProvider"" />
        <property name=""DataReaderWrapperType"" value=""Spring.Data.Support.NullMappingDataReader, Spring.Data"" />
        <property name=""CommandTimeout"" value=""60"" />
    </object>
    <object id=""transactionManager"" type=""Spring.Data.Core.AdoPlatformTransactionManager, Spring.Data"">
        <property name=""DbProvider"" ref=""DbProvider"" />
    </object>
    <tx:attribute-driven />
</objects>";
    }
}