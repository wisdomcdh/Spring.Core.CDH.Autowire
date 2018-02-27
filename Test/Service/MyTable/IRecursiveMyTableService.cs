using Test.Dao.MyTable;

namespace Test.Service.MyTable
{
    internal interface IRecursiveMyTableService
    {
        IMyTableDao MyTableDao { get; set; }
        IMyTableDao MyTableDao2 { get; set; }
        IRecursiveMyTableService Recursive_MyTableService { get; set; }
    }
}