using System;

namespace Spring.Core.CDH.Autowire
{
    /// <summary>
    /// AdoDaoSupport 를 상속받는 타입의 AdoTemplate 속성을 채울 Context 이름 입니다.
    /// 전달되는 <see cref="AdoTemplateNameAttribute.AdoTemplateName"/>은 먼저 SpringContext에 등록되어 있어야 합니다.
    /// 속성이나 클래스에 선언할 수 있으며, 우선순위는 속성 > 클래스 의 순서로 처리됩니다.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class AdoTemplateNameAttribute : Attribute
    {
        /// <summary>
        /// SpringContext에 선언된 Spring.Data.Core.AdoTemplate형태의 Id 입니다.
        /// </summary>
        public string AdoTemplateName { get; set; }

        public AdoTemplateNameAttribute(string adoTemplateName = "AdoTemplate")
        {
            AdoTemplateName = adoTemplateName;
        }
    }
}