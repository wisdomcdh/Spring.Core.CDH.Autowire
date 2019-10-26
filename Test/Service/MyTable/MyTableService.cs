using Spring.Core.CDH.Autowire;
using Test.Dao.MyTable;

namespace Test.Service.MyTable
{
    public class MyTableService : IMyTableService
    {
        [Autowire]
        public IMyTableDao MyTableDao { get; set; }

        [Autowire]
        [AdoTemplateName("AdoTemplate2")]
        public IMyTableDao MyTableDao2 { get; set; }
    }
}