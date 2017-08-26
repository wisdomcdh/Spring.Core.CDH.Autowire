using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Spring.Core.CDH.Autowire
{
    public class AutowireTargetInfo
    {
        public AutowireAttribute AutowireAttribute { get; private set; }
        public AutowireTargetInfo Parent { get; private set; }
        public Type AutowireContextType { get; private set; }
        public string AutowireContextName { get; private set; }
        public PropertyInfo PropertyInfo { get; private set; }
        public bool IsAdoDaoSupport { get; private set; }

        public AutowireTargetInfo(PropertyInfo prop, AutowireTargetInfo parent = null)
        {
            Parent = parent;
            PropertyInfo = prop;
            AutowireAttribute = prop.GetCustomAttribute<AutowireAttribute>(false);
            AutowireContextName = GetAutowireContextName();
            AutowireContextType = GetAutowireContextType();
            IsAdoDaoSupport = GetIsAdoDaoSupport(AutowireContextType);
        }

        private string GetAutowireContextName()
        {
            if (!string.IsNullOrEmpty(AutowireAttribute.ContextName))
            {
                return AutowireAttribute.ContextName;
            }

            var adoTemplateName = GetAdoTemplateName();
            if (string.IsNullOrEmpty(adoTemplateName))
            {
                return $"{AutowireContextType.FullName},{adoTemplateName}";
            }
            else
            {
                return AutowireAttribute.ContextName;
            }
        }

        private Type GetAutowireContextType()
        {
            if (AutowireAttribute.Type != null)
            {
                return AutowireAttribute.Type;
            }
            else
            {
                if (PropertyInfo.PropertyType.IsInterface)
                {
                    // find inherited class on namespace
                    return PropertyInfo.PropertyType.Assembly.GetTypes()
                        .Where(t => t.IsClass && t.Name == PropertyInfo.PropertyType.Namespace && PropertyInfo.PropertyType.IsAssignableFrom(t))
                        .Single();
                }
                else
                {
                    return PropertyInfo.PropertyType;
                }
            }
        }

        private bool GetIsAdoDaoSupport(Type type)
        {
            while (AutowireContextType != null)
            {
                if (type.FullName.Equals(SpringAutowire.AdoDaoSupportFullName)) return true;
                type = type.BaseType;
            }
            return false;
        }

        public string GetAdoTemplateName()
        {
            string adoTemplateName;

            if (IsAdoDaoSupport)
            {
                if (PropertyInfo.IsAttributeDefined<AdoTemplateNameAttribute>())
                {
                    adoTemplateName = PropertyInfo.GetCustomAttribute<AdoTemplateNameAttribute>().AdoTemplateName;
                }
                else
                {
                    adoTemplateName = "AdoTemplate";
                }
            }
            else
            {
                adoTemplateName = string.Empty;
            }

            if (IsAdoTemplateChangeAttributeSpreadFromParentsOrMe())
            {
                var adoTemplateChangeAttributeList = GetAdoTemplateChangeAttributeSpreadFromParentsOrMe();
                if (adoTemplateChangeAttributeList.Any(t => t.Before == adoTemplateName))
                {
                    adoTemplateName = adoTemplateChangeAttributeList.Single(t => t.Before == adoTemplateName).After;
                }
            }
            return adoTemplateName;
        }

        private bool IsAdoTemplateChangeAttributeSpreadFromParentsOrMe()
        {
            AutowireTargetInfo info = this;
            while (info != null)
            {
                if (info.PropertyInfo.IsAttributeDefined<AdoTemplateChangeAttribute>())
                {
                    return true;
                }
                info = info.Parent;
            }
            return false;
        }

        private IList<AdoTemplateChangeAttribute> GetAdoTemplateChangeAttributeSpreadFromParentsOrMe()
        {
            List<AdoTemplateChangeAttribute> result = new List<AdoTemplateChangeAttribute>();
            AutowireTargetInfo info = this;
            while (info != null)
            {
                foreach (var attr in info.PropertyInfo.GetCustomAttributes<AdoTemplateChangeAttribute>())
                {
                    if (!result.Any(t => t.Before == attr.Before))
                    {
                        result.Add(attr);
                    }
                }
                info = info.Parent;
            }
            return result;
        }
    }
}