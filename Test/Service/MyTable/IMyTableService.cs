using Test.Dao.MyTable;

namespace Test.Service.MyTable
{
    public interface IMyTableService
    {
        IMyTableDao MyTableDao { get; set; }
        IMyTableDao MyTableDao2 { get; set; }
    }
}