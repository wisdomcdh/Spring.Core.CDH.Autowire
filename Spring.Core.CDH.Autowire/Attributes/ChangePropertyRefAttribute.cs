using System;

namespace Spring.Core.CDH.Autowire
{
    /// <summary>
    /// 속성의 Ref 값을 변경합니다.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ChangePropertyRefAttribute : PropertyAttribute
    {
        public string OldRef { get; set; }
        public bool DisallowSpread { get; set; }

        public ChangePropertyRefAttribute(string name, string oldRef, string @ref) : base(name, @ref)
        {
            OldRef = oldRef;
        }

        public override string ToString()
        {
            return $"<!--<property name=\"{Name}\" valaue=\"{OldRef}\"/>--><property name=\"{Name}\" valaue=\"{Ref}\"/>";
        }

        public override bool SameRules(PropertyAttribute attr)
        {
            if (attr is ChangePropertyRefAttribute)
            {
                return this.SameRules(attr as ChangePropertyRefAttribute);
            }
            else
            {
                return Name == attr.Name && OldRef == attr.Ref;
            }
        }

        public bool SameRules(ChangePropertyRefAttribute attr)
        {
            return Name == attr.Name && OldRef == attr.OldRef;
        }
    }
}