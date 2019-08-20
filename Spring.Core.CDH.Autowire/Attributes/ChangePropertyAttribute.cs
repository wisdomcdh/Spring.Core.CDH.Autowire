using System;

namespace Spring.Core.CDH.Autowire
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ChangePropertyAttribute : PropertyAttribute
    {
        public string OldRef { get; set; }

        public ChangePropertyAttribute(string name, string oldRef, string @ref) : base(name, @ref)
        {
            OldRef = oldRef;
        }

        public override string ToString()
        {
            return $"<!--<property name=\"{Name}\" valaue=\"{OldRef}\"/>--><property name=\"{Name}\" valaue=\"{Ref}\"/>";
        }

        public override bool Same(object attr)
        {
            if (attr is ChangePropertyAttribute)
            {
                if (base.Same(attr))
                {
                    var cAttr = attr as ChangePropertyAttribute;
                    return OldRef == cAttr.OldRef;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return base.Same(attr);
            }
        }
    }
}