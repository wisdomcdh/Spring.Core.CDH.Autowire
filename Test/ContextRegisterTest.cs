using NUnit.Framework;
using Spring.Core.CDH.Autowire;
using System.IO;

namespace Test
{
    [TestFixture]
    public abstract class ContextRegisterTest
    {
        protected string conn { get; private set; }
        protected string conn2 { get; private set; }
        protected string conn3 { get; private set; }
        protected string conn4 { get; private set; }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var path = Path.GetDirectoryName(typeof(ContextRegisterTest).Assembly.Location);
            var connFormat = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={0};Integrated Security=True";
            var testDb = Path.Combine(path, "TestDB.mdf");
            var testDb2 = Path.Combine(path, "TestDB2.mdf");
            var testDb3 = Path.Combine(path, "TestDB3.mdf");
            var testDb4 = Path.Combine(path, "TestDB4.mdf");
            conn = string.Format(connFormat, testDb);
            conn2 = string.Format(connFormat, testDb2);
            conn3 = string.Format(connFormat, testDb3);
            conn4 = string.Format(connFormat, testDb4);

            var context = string.Format(xmlContext, conn, conn2, conn3, conn4);
            ContextRegister.RegisterContext(context);
        }

        private string xmlContext = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<objects xmlns=""http://www.springframework.net""
         xmlns:aop=""http://www.springframework.net/aop""
         xmlns:db=""http://www.springframework.net/database""
         xmlns:tx=""http://www.springframework.net/tx"">
    <db:provider id=""DbProvider"" provider=""System.Data.SqlClient"" connectionString=""{0}"" />
    <db:provider id=""DbProvider2"" provider=""System.Data.SqlClient"" connectionString=""{1}"" />
    <db:provider id=""DbProvider3"" provider=""System.Data.SqlClient"" connectionString=""{2}"" />
    <db:provider id=""DbProvider4"" provider=""System.Data.SqlClient"" connectionString=""{3}"" />
    <object id=""AdoTemplate"" type=""Spring.Data.Generic.AdoTemplate, Spring.Data"">
        <property name=""DbProvider"" ref=""DbProvider"" />
        <property name=""DataReaderWrapperType"" value=""Spring.Data.Support.NullMappingDataReader, Spring.Data"" />
        <property name=""CommandTimeout"" value=""60"" />
    </object>
    <object id=""AdoTemplate2"" type=""Spring.Data.Generic.AdoTemplate, Spring.Data"">
        <property name=""DbProvider"" ref=""DbProvider2"" />
        <property name=""DataReaderWrapperType"" value=""Spring.Data.Support.NullMappingDataReader, Spring.Data"" />
        <property name=""CommandTimeout"" value=""60"" />
    </object>
    <object id=""AdoTemplate3"" type=""Spring.Data.Generic.AdoTemplate, Spring.Data"">
        <property name=""DbProvider"" ref=""DbProvider3"" />
        <property name=""DataReaderWrapperType"" value=""Spring.Data.Support.NullMappingDataReader, Spring.Data"" />
        <property name=""CommandTimeout"" value=""60"" />
    </object>
    <object id=""AdoTemplate4"" type=""Spring.Data.Generic.AdoTemplate, Spring.Data"">
        <property name=""DbProvider"" ref=""DbProvider4"" />
        <property name=""DataReaderWrapperType"" value=""Spring.Data.Support.NullMappingDataReader, Spring.Data"" />
        <property name=""CommandTimeout"" value=""60"" />
    </object>
    <object id=""transactionManager"" type=""Spring.Data.Core.AdoPlatformTransactionManager, Spring.Data"">
        <property name=""DbProvider"" ref=""DbProvider"" />
    </object>
    <object id=""transactionManager2"" type=""Spring.Data.Core.AdoPlatformTransactionManager, Spring.Data"">
        <property name=""DbProvider"" ref=""DbProvider2"" />
    </object>
    <object id=""transactionManager3"" type=""Spring.Data.Core.AdoPlatformTransactionManager, Spring.Data"">
        <property name=""DbProvider"" ref=""DbProvider3"" />
    </object>
    <object id=""transactionManager4"" type=""Spring.Data.Core.AdoPlatformTransactionManager, Spring.Data"">
        <property name=""DbProvider"" ref=""DbProvider4"" />
    </object>
    <object id=""String1"" type=""System.String"" >
        <constructor-arg value=""Text1"" />
    </object>
    <object id=""String2"" type=""System.String"" >
        <constructor-arg value=""Text2"" />
    </object>
    <object id=""ChangeString"" type=""System.String"" >
        <constructor-arg value=""ChangeText1"" />
    </object>
    <object id=""tranMyTableDao"" type=""Test.Dao.MyTable.MyTableDao, Spring.CDH.Test"">
        <property name=""AdoTemplate"" ref=""AdoTemplate"" />
    </object>
    <object id=""tranTest"" type=""Test.Service.MyTable.TransactionMyTableService, Spring.CDH.Test"">
        <property name=""MyTableDao"" ref=""tranMyTableDao"" />
    </object>
    <tx:attribute-driven transaction-manager=""transactionManager""/>
    <tx:attribute-driven transaction-manager=""transactionManager2""/>
    <tx:attribute-driven transaction-manager=""transactionManager3""/>
    <tx:attribute-driven transaction-manager=""transactionManager4""/>
</objects>";
    }
}