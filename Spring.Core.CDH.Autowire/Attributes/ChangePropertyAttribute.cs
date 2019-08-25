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
    }
}