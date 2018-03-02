using Spring.Core.CDH.Autowire;
using Test.Dao.MyTable;

namespace Test.Service.MyTable
{
    internal class RecursiveMyTableService2 : IRecursiveMyTableService2
    {
        [Autowire]
        public IMyTableDao MyTableDao { get; set; }

        [Autowire]
        [AdoTemplateName("AdoTemplate2")]
        public IMyTableDao MyTableDao2 { get; set; }

        [Autowire]
        [ChangeAdoTemplate("AdoTemplate", "AdoTemplate3")]
        public IRecursiveMyTableService2 Recursive_MyTableService2 { get; set; }
    }
}