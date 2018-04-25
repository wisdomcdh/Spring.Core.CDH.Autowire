using Spring.Core.CDH.Autowire;
using Spring.Transaction.Interceptor;
using System;
using Test.Dao.MyTable;
using Test.Dao.MyTable.Model;

namespace Test.Service.MyTable
{
    public class TransactionMyTableService : ITransactionMyTableService
    {
        [Autowire]
        public IMyTableDao MyTableDao { get; set; }
        [Autowire]
        [AdoTemplateName("AdoTemplate2")]
        public IMyTableDao MyTableDao2 { get; set; }

        [Transaction]
        public void TestTransaction()
        {
            MyTableDao.GetConnectionString();
        }

        [Transaction]
        public void TestInsert(MyTableItem item)
        {
            MyTableDao.Insert(item);
            MyTableDao2.Insert(item);
            if (true)
            {
                //throw new Exception("TEST");
            }
        }
    }
}