using System;

namespace Spring.Core.CDH.Autowire
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class AutowireAttribute : Attribute
    {
        public string ContextName { get; set; }
        public string MergeContextName { get; set; }
        public Type Type { get; set; }
        public bool Singleton { get; set; } = true;
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class AutowireMAttribute : AutowireAttribute
    {
        public string MergeBase { get; set; }
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
    public class ChangeWireAttribute : Attribute
    {
        public string Before { get; set; }
        public string After { get; set; }

        public ChangeWireAttribute(string before, string after)
        {
            Before = before;
            After = after;
        }

        public override string ToString()
        {
            return $"{Before}-{After}";
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ChangeAdoTemplateAttribute : Attribute
    {
        public string Before { get; set; }
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