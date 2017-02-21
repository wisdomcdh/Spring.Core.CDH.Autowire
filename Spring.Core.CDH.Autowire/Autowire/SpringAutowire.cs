using Spring.Context.Support;
using Spring.Objects;
using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Spring.Core.CDH.Autowire
{
    public static class SpringAutowire
    {
        private static object _lock = new object();
        private const string AdoDaoSupportFullName = "Spring.Data.Generic.AdoDaoSupport";

        public static void Autowire(object obj)
        {
            AbstractApplicationContext ctx = ContextRegistry.GetContext() as AbstractApplicationContext;
            Autowire(ctx, obj);
        }

        private static void Autowire(AbstractApplicationContext ctx, object obj)
        {
            Type objType = obj.GetType();
            foreach (var prop in GetAutowireTargetProperties(objType))
            {
                if (!ctx.IsObjectNameInUse(prop.AutowireAttr.ContextName))
                {
                    lock (_lock)
                    {
                        if (!ctx.IsObjectNameInUse(prop.AutowireAttr.ContextName))
                        {
                            CreateObjectDefinition(ctx, prop);
                        }
                    }
                }
                prop.Prop.SetValue(obj, ctx.GetObject(prop.AutowireAttr.ContextName));
            }
        }

        private static string CreateObjectDefinition(AbstractApplicationContext ctx, AutowireTargetProperty prop)
        {
            if (!ctx.IsObjectNameInUse(prop.AutowireAttr.ContextName))
            {
                var objectDefinition = GetObjectDefinition(ctx, prop);
                PropertyValue pv;
                foreach (var inProp in GetAutowireTargetProperties(prop.AutowireAttr.Type))
                {
                    pv = objectDefinition.PropertyValues.PropertyValues.FirstOrDefault(t => t.Name.Equals(inProp.Prop.Name));
                    if (pv == null)
                    {
                        objectDefinition.PropertyValues.Add(inProp.Prop.Name, new RuntimeObjectReference(CreateObjectDefinition(ctx, inProp)));
                    }
                }
                foreach (var diProp in prop.Prop.GetCustomAttributes<InjectionPropertyAttribute>())
                {
                    pv = objectDefinition.PropertyValues.PropertyValues.FirstOrDefault(t => t.Name.Equals(diProp.PropertyName));
                    if (pv != null)
                    {
                        objectDefinition.PropertyValues.Remove(pv);
                    }
                    objectDefinition.PropertyValues.Add(diProp.PropertyName, new RuntimeObjectReference(diProp.ReferenceContextName));
                }

                // AdoTemplate Default
                if (IsAssignableFromAdoDaoSupport(prop.AutowireAttr.Type))
                {
                    if (!objectDefinition.PropertyValues.Contains("AdoTemplate"))
                    {
                        var adoTemplateAttr = prop.Prop.GetCustomAttribute<InjectionAdoTemplateAttribute>();
                        objectDefinition.PropertyValues.Add("AdoTemplate", new RuntimeObjectReference(adoTemplateAttr?.ReferenceContextName ?? "AdoTemplate"));
                    }
                }
            }
            return prop.AutowireAttr.ContextName;
        }

        private static IObjectDefinition GetObjectDefinition(AbstractApplicationContext ctx, AutowireTargetProperty prop)
        {
            if (ctx.IsObjectNameInUse(prop.AutowireAttr.ContextName))
            {
                return ctx.GetObjectDefinition(prop.AutowireAttr.ContextName);
            }

            IObjectDefinitionFactory fac = new DefaultObjectDefinitionFactory();

            var objectDefinition = fac.CreateObjectDefinition(prop.AutowireAttr.Type.AssemblyQualifiedName, null, AppDomain.CurrentDomain);
            objectDefinition.IsSingleton = prop.AutowireAttr.Singleton;

            ctx.ObjectFactory.RegisterObjectDefinition(prop.AutowireAttr.ContextName, objectDefinition);
            return ctx.GetObjectDefinition(prop.AutowireAttr.ContextName);
        }

        private static IEnumerable<AutowireTargetProperty> GetAutowireTargetProperties(Type objectType)
        {
            return objectType.GetProperties()
                .Where(t => Attribute.IsDefined(t, typeof(AutowireAttribute)))
                .Select(t =>
                {
                    var prop = new AutowireTargetProperty();
                    prop.Prop = t;
                    prop.AutowireAttr = t.GetCustomAttribute<AutowireAttribute>();
                    prop.AutowireAttr.Type = GetDependencyInjectionType(prop.Prop, prop.AutowireAttr);
                    prop.AutowireAttr.ContextName = GetAutowireContextName(prop.Prop, prop.AutowireAttr);
                    return prop;
                });
        }

        private static string GetAutowireContextName(PropertyInfo prop, AutowireAttribute autowireAttr)
        {
            if (!string.IsNullOrEmpty(autowireAttr.ContextName))
            {
                return autowireAttr.ContextName;
            }

            if (IsAssignableFromAdoDaoSupport(autowireAttr.Type))
            {
                var adoTemplate = GetAdoTemplateContextPropertyAttribute(prop);
                return $"{autowireAttr.Type.FullName}_{adoTemplate?.ReferenceContextName ?? "AdoTemplate"}";
            }

            return $"{autowireAttr.Type.FullName},{prop.Name}";
        }

        private static Type GetDependencyInjectionType(PropertyInfo prop, AutowireAttribute autowireAttr)
        {
            if (autowireAttr.Type != null) return autowireAttr.Type;

            if (prop.PropertyType.IsInterface)
            {
                var propArray = prop.PropertyType.FullName.Split('.');
                propArray[propArray.Length - 1] = propArray[propArray.Length - 1].Replace(prop.PropertyType.Name, prop.PropertyType.Name.Substring(1));
                return Type.GetType(string.Format("{0}, {1}", string.Join(".", propArray), prop.PropertyType.Assembly.FullName));
            }

            return prop.PropertyType;
        }

        private static bool IsAssignableFromAdoDaoSupport(Type type)
        {
            while (type != null)
            {
                if (type.FullName.Equals(AdoDaoSupportFullName)) return true;
                type = type.BaseType;
            }
            return false;
        }

        private static InjectionPropertyAttribute GetAdoTemplateContextPropertyAttribute(PropertyInfo prop)
        {
            return prop.GetCustomAttributes<InjectionPropertyAttribute>().FirstOrDefault(t => t is InjectionAdoTemplateAttribute || t.PropertyName.Equals("AdoTemplate"));
        }

        internal class AutowireTargetProperty
        {
            public PropertyInfo Prop { get; set; }
            public AutowireAttribute AutowireAttr { get; set; }
        }
    }
}