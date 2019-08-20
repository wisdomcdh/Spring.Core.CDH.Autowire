using System;

namespace Spring.Core.CDH.Autowire
{
    /// <summary>
    /// 속성을 정의 합니다.
    /// 설정 우선순위는 속성 > 클래스 입니다.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = true)]
    public class PropertyAttribute : Attribute
    {
        public string Name { get; set; }
        public string Ref { get; set; }
        public Type Type { get; set; }

        public PropertyAttribute(string name, string @ref)
        {
            Name = name;
            Ref = @ref;
        }

        public override string ToString()
        {
            return $"<property name=\"{Name}\" valaue=\"{Ref}\"/>";
        }

        public virtual bool Same(object attr)
        {
            if (attr is PropertyAttribute)
            {
                var cAttr = attr as PropertyAttribute;
                return Name == cAttr.Name
                    && Type == cAttr.Type;
            }
            else
            {
                return false;
            }
        }
    }
}