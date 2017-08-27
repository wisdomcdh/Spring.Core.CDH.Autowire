using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Spring.Core.CDH.Autowire
{
    public class AutowireTargetInfo
    {
        private Type _AutowireContextType;
        private string _AutowireContextName;
        private bool? _IsAdoDaoSupport;

        public AutowireTargetInfo Parent { get; private set; }
        public PropertyInfo PropertyInfo { get; private set; }
        public AutowireAttribute AutowireAttribute { get; private set; }

        public Type AutowireContextType
        {
            get
            {
                if (_AutowireContextType == null)
                {
                    _AutowireContextType = GetAutowireContextType();
                }
                return _AutowireContextType;
            }
        }

        public string AutowireContextName
        {
            get
            {
                if (_AutowireContextName == null)
                {
                    _AutowireContextName = GetAutowireContextName();
                }
                return _AutowireContextName;
            }
        }

        public bool IsAdoDaoSupport
        {
            get
            {
                if (!_IsAdoDaoSupport.HasValue)
                {
                    _IsAdoDaoSupport = GetIsAdoDaoSupport(AutowireContextType);
                }
                return _IsAdoDaoSupport.Value;
            }
        }

        public AutowireTargetInfo(PropertyInfo prop, AutowireTargetInfo parent = null)
        {
            Parent = parent;
            PropertyInfo = prop;
            AutowireAttribute = prop.GetCustomAttribute<AutowireAttribute>(false);
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
                return $"{AutowireContextType.FullName}";
            }
            else
            {
                return $"{AutowireContextType.FullName},{adoTemplateName}";
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
                    return Type.GetType($"{PropertyInfo.PropertyType.Namespace}.{string.Concat(PropertyInfo.PropertyType.Name.Skip(1))}, {PropertyInfo.PropertyType.Assembly.FullName}");
                }
                else
                {
                    return PropertyInfo.PropertyType;
                }
            }
        }

        private bool GetIsAdoDaoSupport(Type type)
        {
            while (type != null)
            {
                if (type?.FullName == SpringAutowire.AdoDaoSupportFullName) return true;
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
                if (IsAdoDaoSupport)
                {
                    if (adoTemplateChangeAttributeList.Any(t => t.Before == adoTemplateName))
                    {
                        adoTemplateName = adoTemplateChangeAttributeList.Single(t => t.Before == adoTemplateName).After;
                    }
                }
                else
                {
                    if (adoTemplateChangeAttributeList.Count > 0)
                    {
                        adoTemplateName = string.Join(",", adoTemplateChangeAttributeList.Select(t => t.ToString()));
                    }
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