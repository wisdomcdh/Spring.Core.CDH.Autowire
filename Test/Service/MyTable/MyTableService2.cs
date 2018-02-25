using Spring.Core.CDH.Autowire;
using Test.Dao.MyTable;

namespace Test.Service.MyTable
{
    public class MyTableService2 : IMyTableService2
    {
        [Autowire]
        public IMyTableDao MyTableDao { get; set; }

        [Autowire]
        [AdoTemplateName("AdoTemplate2")]
        public IMyTableDao MyTableDao2 { get; set; }

        [Autowire(Type = typeof(MyTableService))]
        public IMyTableService MyTableService { get; set; }

        [Autowire(Type = typeof(MyTableService))]
        [ChangeWire("AdoTemplate", "AdoTemplate3")]
        public IMyTableService MyTableService_AdoTemplateChange { get; set; }

        [Autowire(Type = typeof(MyTableService2))]
        [ChangeWire("AdoTemplate", "AdoTemplate2")]
        [ChangeWire("AdoTemplate3", "AdoTemplate2")]
        public IMyTableService2 MyTableService2_Recursive_AdoTemplateChange { get; set; }
    }
}