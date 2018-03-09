using Spring.Core.CDH.Autowire;
using Spring.Transaction.Interceptor;
using Test.Dao.MyTable;

namespace Test.Service.MyTable
{
    public class TransactionMyTableService : ITransactionMyTableService
    {
        [Autowire]
        public IMyTableDao MyTableDao { get; set; }

        [Transaction]
        public void TestTransaction()
        {
            MyTableDao.GetConnectionString();
        }
    }
}