using Spring.Core.CDH.Autowire;
using Test.Dao.MyTable;

namespace Test.Service.MyTable
{
    internal class RecursiveMyTableService : IRecursiveMyTableService
    {
        [Autowire]
        public IMyTableDao MyTableDao { get; set; }

        [Autowire]
        [AdoTemplateName("AdoTemplate2")]
        public IMyTableDao MyTableDao2 { get; set; }

        [Autowire]
        public IRecursiveMyTableService Recursive_MyTableService { get; set; }
    }
}