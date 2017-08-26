using Spring.Context.Support;
using Spring.Objects;
using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Spring.Core.CDH.Autowire
{
    public static class SpringAutowire
    {
        internal const string DefaultRootContextName = "spring.root";
        internal const string AdoDaoSupportFullName = "Spring.Data.Generic.AdoDaoSupport";
        private static ConcurrentDictionary<string, object> _lockMap = new ConcurrentDictionary<string, object>();
        private static readonly IObjectDefinitionFactory fac = new DefaultObjectDefinitionFactory();

        public static void Autowire(object obj, string rootContextName = DefaultRootContextName)
        {
            AbstractApplicationContext ctx = ContextRegistry.GetContext(rootContextName) as AbstractApplicationContext;

            foreach (AutowireTargetInfo info in GetAutowireTargetInfos(obj.GetType()))
            {
                CreateObjectDefinition(ctx, info);
                info.PropertyInfo.SetValue(obj, ctx.GetObject(info.AutowireContextName));
            }
        }

        private static void CreateObjectDefinition(AbstractApplicationContext ctx, AutowireTargetInfo info)
        {
            if (!ctx.IsObjectNameInUse(info.AutowireContextName))
            {
                object lockObj, lockObjCheck;
                lock (lockObj = _lockMap.GetOrAdd(info.AutowireContextName, new object()))
                {
                    if (!ctx.IsObjectNameInUse(info.AutowireContextName))
                    {
                        IObjectDefinition objectDefinition = GetOrCreateObjectDefinition(ctx, info);
                        PropertyValue pv;
                        foreach (AutowireTargetInfo inInfo in GetAutowireTargetInfos(info.AutowireContextType, info))
                        {
                            pv = objectDefinition.PropertyValues.PropertyValues.FirstOrDefault(t => t.Name == inInfo.PropertyInfo.Name);
                            if (pv == null)
                            {
                                CreateObjectDefinition(ctx, inInfo);
                                objectDefinition.PropertyValues.Add(inInfo.PropertyInfo.Name, new RuntimeObjectReference(inInfo.AutowireContextName));
                            }
                        }

                        if (info.IsAdoDaoSupport)
                        {
                            if (!objectDefinition.PropertyValues.Contains("AdoTemplate"))
                            {
                                objectDefinition.PropertyValues.Add("AdoTemplate", new RuntimeObjectReference(info.GetAdoTemplateName()));
                            }
                        }
                    }
                    _lockMap.TryRemove(info.AutowireContextName, out lockObjCheck);
                }
            }
        }

        private static IObjectDefinition GetOrCreateObjectDefinition(AbstractApplicationContext ctx, AutowireTargetInfo info)
        {
            if (ctx.IsObjectNameInUse(info.AutowireContextName))
            {
                return ctx.GetObjectDefinition(info.AutowireContextName);
            }
            else
            {
                AbstractObjectDefinition objectDefinition = fac.CreateObjectDefinition(info.AutowireContextType.AssemblyQualifiedName, null, AppDomain.CurrentDomain);
                objectDefinition.IsSingleton = info.AutowireAttribute.Singleton;

                ctx.ObjectFactory.RegisterObjectDefinition(info.AutowireContextName, objectDefinition);
                return ctx.GetObjectDefinition(info.AutowireContextName);
            }
        }

        private static IEnumerable<AutowireTargetInfo> GetAutowireTargetInfos(Type type, AutowireTargetInfo parent = null)
        {
            return type.GetProperties().Where(prop => prop.IsAttributeDefined<AutowireAttribute>()).Select(prop => new AutowireTargetInfo(prop, parent));
        }
    }
}