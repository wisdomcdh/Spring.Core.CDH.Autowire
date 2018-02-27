using Test.Dao.MyTable;

namespace Test.Service.MyTable
{
    interface IRecursiveMyTableService2
    {
        IMyTableDao MyTableDao { get; set; }
        IMyTableDao MyTableDao2 { get; set; }
        IRecursiveMyTableService2 Recursive_MyTableService2 { get; set; }
    }
}