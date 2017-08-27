using Test.Dao.MyTable;

namespace Test.Service.MyTable
{
    public interface IMyTableService2
    {
        IMyTableDao MyTableDao { get; set; }
        IMyTableDao MyTableDao2 { get; set; }
        IMyTableService MyTableService { get; set; }
        IMyTableService2 MyTableService2_Recursive_AdoTemplateChange { get; set; }
        IMyTableService MyTableService_AdoTemplateChange { get; set; }
    }
}