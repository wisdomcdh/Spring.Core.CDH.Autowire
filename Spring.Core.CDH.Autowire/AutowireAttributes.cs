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

    [AttributeUsage(AttributeTargets.Property)]
    public class AdoTemplateNameAttribute : Attribute
    {
        public string AdoTemplateName { get; set; }

        public AdoTemplateNameAttribute(string adoTemplateName = "AdoTemplate")
        {
            AdoTemplateName = adoTemplateName;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class AdoTemplateChangeAttribute : Attribute
    {
        public string Before { get; set; }
        public string After { get; set; }

        public AdoTemplateChangeAttribute(string before, string after)
        {
            Before = before;
            After = after;
        }
    }
}