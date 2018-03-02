using Spring.Core.CDH.Autowire;
using Test.Dao.MyTable;
using Test.Service.MyTable;

namespace Test.Controllers
{
    public class AdoTemplateChangeTestController
    {
        [Autowire]
        public IMyTableService MyTableService { get; set; }

        [Autowire]
        [ChangeAdoTemplate("AdoTemplate", "AdoTemplate2")]
        [ChangeAdoTemplate("AdoTemplate2", "AdoTemplate3")]
        public IMyTableService MyTableService2 { get; set; }

        /// <summary>
        /// AdoDaoSupport 를 상속받는 DAO 클래스
        /// AdoTemplateName 특성이 선언되어 있지 않으면, AdoTemplate 를 기본으로 한다.
        /// </summary>
        [Autowire]
        public IMyTableDao MyTableDao { get; set; }

        [Autowire]
        [AdoTemplateName("AdoTemplate2")]
        public IMyTableDao MyTableDao2 { get; set; }

        [Autowire]
        [ChangeAdoTemplate("AdoTemplate", "AdoTemplate3")]
        public IMyTableDao MyTableDao3 { get; set; }

        [Autowire]
        [AdoTemplateName("AdoTemplate2")]
        [ChangeAdoTemplate("AdoTemplate2", "AdoTemplate4")]
        public IMyTableDao MyTableDao4 { get; set; }
    }
}