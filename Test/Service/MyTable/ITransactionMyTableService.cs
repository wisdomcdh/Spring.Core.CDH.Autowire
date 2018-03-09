using Test.Dao.MyTable;

namespace Test.Service.MyTable
{
    public interface ITransactionMyTableService
    {
        IMyTableDao MyTableDao { get; set; }

        void TestTransaction();
    }
}