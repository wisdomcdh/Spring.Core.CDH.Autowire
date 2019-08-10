using System;

namespace Spring.Core.CDH.Autowire
{
    /// <summary>
    /// Autowire 되는 현재 속성으로 부터 시작되는 스코프 내의 모든 AdoDaoSupport를 상속받는 클래스의 특정 AdoTemplateNameAttribute 설정을 강제로 변경합니다.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ChangeAdoTemplateAttribute : Attribute
    {
        /// <summary>
        /// 변경대상
        /// </summary>
        public string Before { get; set; }

        /// <summary>
        /// 변경정보
        /// </summary>
        public string After { get; set; }

        public ChangeAdoTemplateAttribute(string before, string after)
        {
            Before = before;
            After = after;
        }

        public override string ToString()
        {
            return $"{Before}-{After}";
        }
    }
}