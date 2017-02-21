using System;

namespace Spring.Core.CDH.Autowire
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AutowireAttribute : Attribute
    {
        public string ContextName { get; set; }

        public Type Type { get; set; }

        public bool Singleton { get; set; } = true;
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class InjectionPropertyAttribute : Attribute
    {
        public string PropertyName { get; set; }
        public string ReferenceContextName { get; set; }

        public InjectionPropertyAttribute(string propertyName, string referenceContextName)
        {
            PropertyName = propertyName;
            ReferenceContextName = referenceContextName;
        }
    }

    public class InjectionAdoTemplateAttribute : InjectionPropertyAttribute
    {
        public InjectionAdoTemplateAttribute(string referenceContextName = "AdoTemplate") : base("AdoTemplate", referenceContextName)
        {
        }
    }
}