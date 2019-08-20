using Spring.Context.Support;
using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;
using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace Spring.Core.CDH.Autowire
{
    public static class SpringAutowire
    {
        internal const string DefaultRootContextName = "spring.root";
        private static readonly ConcurrentDictionary<string, object> _lockMap = new ConcurrentDictionary<string, object>();
        private static readonly IObjectDefinitionFactory fac = new DefaultObjectDefinitionFactory();

        /// <summary>
        /// 객체의 속성을 분석하여 Autowire 합니다.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="rootContextName"></param>
        public static void Autowire(object obj, string rootContextName = DefaultRootContextName)
        {
            AbstractApplicationContext ctx = GetApplicationContext(rootContextName);

            foreach (AutowireTargetPropertyInfo info in AutowireTargetPropertyGetter.FindProperties(obj.GetType()))
            {
                CreateObjectDefinition(ctx, info);
                info.PropertyInfo.SetValue(obj, ctx.GetObject(info.ObjectInfo.Id));
            }
        }

        public static object Autowire(Type type, AutowireAttribute autowireAttribute = null, string rootContextName = DefaultRootContextName)
        {
            AbstractApplicationContext ctx = GetApplicationContext(rootContextName);
            if (autowireAttribute == null)
            {
                autowireAttribute = type.GetCustomAttribute<AutowireAttribute>();
            }
            var info = new AutowireTargetPropertyInfo(autowireAttribute, type);
            CreateObjectDefinition(ctx, info);
            return ctx.GetObject(info.ObjectInfo.Id);
        }

        public static T Autowire<T>(AutowireAttribute autowireAttribute = null, string rootContextName = DefaultRootContextName)
        {
            return (T)Autowire(typeof(T), autowireAttribute, rootContextName);
        }

        private static AbstractApplicationContext GetApplicationContext(string rootContextName)
        {
            return ContextRegistry.GetContext(rootContextName) as AbstractApplicationContext;
        }

        private static void CreateObjectDefinition(AbstractApplicationContext ctx, AutowireTargetPropertyInfo info)
        {
            if (!ctx.IsObjectNameInUse(info.ObjectInfo.Id))
            {
                lock (_lockMap.GetOrAdd(info.ObjectInfo.Id, new object()))
                {
                    if (!ctx.IsObjectNameInUse(info.ObjectInfo.Id))
                    {
                        IObjectDefinition mergeTargetObjectDefinition = null;
                        IObjectDefinition objectDefinition = GetOrCreateObjectDefinition(ctx, info);
                        bool hasMergeContext = false;
                        if (!string.IsNullOrEmpty(info.ObjectInfo.PropertyDefinedAutowireAttribute.MergeBase) && ctx.IsObjectNameInUse(info.ObjectInfo.PropertyDefinedAutowireAttribute.MergeBase))
                        {
                            hasMergeContext = true;
                            mergeTargetObjectDefinition = ctx.GetObjectDefinition(info.ObjectInfo.PropertyDefinedAutowireAttribute.MergeBase);
                        }

                        foreach (AutowireTargetPropertyInfo inInfo in AutowireTargetPropertyGetter.FindProperties(info.ObjectInfo.ObjectType, info))
                        {
                            if (hasMergeContext)
                            {
                                if (mergeTargetObjectDefinition.PropertyValues.Contains(inInfo.PropertyInfo.Name))
                                {
                                    if (objectDefinition.PropertyValues.Contains(inInfo.PropertyInfo.Name))
                                    {
                                        objectDefinition.PropertyValues.Remove(objectDefinition.PropertyValues.GetPropertyValue(inInfo.PropertyInfo.Name));
                                    }
                                    objectDefinition.PropertyValues.Add(mergeTargetObjectDefinition.PropertyValues.GetPropertyValue(inInfo.PropertyInfo.Name));
                                }
                            }

                            if (!objectDefinition.PropertyValues.Contains(inInfo.PropertyInfo.Name))
                            {
                                CreateObjectDefinition(ctx, inInfo);
                                objectDefinition.PropertyValues.Add(inInfo.PropertyInfo.Name, new RuntimeObjectReference(inInfo.ObjectInfo.Id));
                            }
                        }

                        foreach (var confirmedPropAttr in info.ObjectInfo.ConfirmedPropertyAttributes)
                        {
                            if (!objectDefinition.PropertyValues.Contains(confirmedPropAttr.Name))
                            {
                                objectDefinition.PropertyValues.Add(confirmedPropAttr.Name, new RuntimeObjectReference(confirmedPropAttr.Ref));
                            }
                        }

                        //if (ObjectTypeUtil.IsInheritOfAdoDaoSupport(info.ObjectInfo.GetObjectType()))
                        //{
                        //    if (!objectDefinition.PropertyValues.Contains("AdoTemplate"))
                        //    {
                        //        objectDefinition.PropertyValues.Add("AdoTemplate", new RuntimeObjectReference(info.ObjectInfo.GetAdoTemplateName()));
                        //    }
                        //}
                    }
                }
            }
        }

        private static IObjectDefinition GetOrCreateObjectDefinition(AbstractApplicationContext ctx, AutowireTargetPropertyInfo info)
        {
            if (ctx.IsObjectNameInUse(info.ObjectInfo.Id))
            {
                return ctx.GetObjectDefinition(info.ObjectInfo.Id);
            }
            else
            {
                AbstractObjectDefinition objectDefinition = fac.CreateObjectDefinition(info.ObjectInfo.Type, null, AppDomain.CurrentDomain);
                objectDefinition.IsSingleton = info.ObjectInfo.Singleton;
                ctx.ObjectFactory.RegisterObjectDefinition(info.ObjectInfo.Id, objectDefinition);
                return ctx.GetObjectDefinition(info.ObjectInfo.Id);
            }
        }
    }
}