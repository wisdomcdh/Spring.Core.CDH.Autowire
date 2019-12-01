using System;

namespace Spring.Core.CDH.Autowire
{
    /// <summary>
    /// Autowire 특성이 선언된 속성들은 자동으로 SpringContext에 등록됩니다.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class AutowireAttribute : Attribute
    {
        /// <summary>
        /// 등록 될 컨텍스트 명
        /// <para>값이 없으면 정해진 룰에 의해 SpringContext에 등록 됩니다.</para>
        /// </summary>
        public string ContextName { get; set; }

        /// <summary>
        /// SpringContext에 미리 설정된 값으로부터 Autorwire 작업을 진행 합니다.
        /// <para></para>
        /// </summary>
        public string MergeBase { get; set; }

        /// <summary>
        /// 생성될 인스턴스의 유형
        /// <para></para>
        /// 유형을 지정하지 않으면, 속성의 유형은 정해진 규칙에 따라 유추합니다.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// 싱글턴 여부
        /// <para></para>
        /// 기본값 True
        /// </summary>
        public bool Singleton { get; set; } = true;

        /// <summary>
        /// 지연된 생성여부
        /// <para></para>
        /// 기본값 False
        /// </summary>
        public bool LazyInit { get; set; } = false;

        public override string ToString()
        {
            return $"<Autowire singleton=\"{Singleton}\" lazy-init=\"{LazyInit}\" context-name=\'{ContextName}\' merge-base=\'{MergeBase}\' type=\'{Type?.Name}\'/>";
        }
    }
}