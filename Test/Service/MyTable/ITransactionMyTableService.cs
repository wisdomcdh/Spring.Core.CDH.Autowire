using Test.Dao.MyTable;
using Test.Dao.MyTable.Model;

namespace Test.Service.MyTable
{
    public interface ITransactionMyTableService
    {
        IMyTableDao MyTableDao { get; set; }

        void TestTransaction();
        void TestInsert(MyTableItem item);
    }
}