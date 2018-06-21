﻿using Spring.Context.Support;
using Spring.Core.CDH.Util;
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
            AbstractApplicationContext ctx = GetApplicationContext(rootContextName);

            foreach (AutowireTargetPropertyInfo info in GetAutowireTargetPropertyInfo(obj.GetType()))
            {
                CreateObjectDefinition(ctx, info);
                info.PropertyInfo.SetValue(obj, ctx.GetObject(info.ObjectInfo.id));
            }
        }

        public static object Autowire(Type type, AutowireAttribute autowireAttribute, string rootContextName = DefaultRootContextName)
        {
            AbstractApplicationContext ctx = GetApplicationContext(rootContextName);
            var info = new AutowireTargetPropertyInfo(autowireAttribute, type);
            CreateObjectDefinition(ctx, info);
            return ctx.GetObject(info.ObjectInfo.id);
        }

        private static AbstractApplicationContext GetApplicationContext(string rootContextName)
        {
            return ContextRegistry.GetContext(rootContextName) as AbstractApplicationContext;
        }

        private static void CreateObjectDefinition(AbstractApplicationContext ctx, AutowireTargetPropertyInfo info)
        {
            if (!ctx.IsObjectNameInUse(info.ObjectInfo.id))
            {
                lock (_lockMap.GetOrAdd(info.ObjectInfo.id, new object()))
                {
                    if (!ctx.IsObjectNameInUse(info.ObjectInfo.id))
                    {
                        IObjectDefinition mergeTargetObjectDefinition = null;
                        IObjectDefinition objectDefinition = GetOrCreateObjectDefinition(ctx, info);
                        bool hasMergeContext = false;
                        if (info.ObjectInfo.HasMergeContextName() && ctx.IsObjectNameInUse(info.ObjectInfo.GetMergeContextName()))
                        {
                            hasMergeContext = true;
                            mergeTargetObjectDefinition = ctx.GetObjectDefinition(info.ObjectInfo.GetMergeContextName());
                        }

                        foreach (AutowireTargetPropertyInfo inInfo in GetAutowireTargetPropertyInfo(info.ObjectInfo.GetObjectType(), info))
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
                                objectDefinition.PropertyValues.Add(inInfo.PropertyInfo.Name, new RuntimeObjectReference(inInfo.ObjectInfo.id));
                            }
                        }

                        if (ObjectTypeUtil.IsInheritOfAdoDaoSupport(info.ObjectInfo.GetObjectType()))
                        {
                            if (!objectDefinition.PropertyValues.Contains("AdoTemplate"))
                            {
                                objectDefinition.PropertyValues.Add("AdoTemplate", new RuntimeObjectReference(info.ObjectInfo.GetAdoTemplateName()));
                            }
                        }
                    }
                }
            }
        }

        private static IObjectDefinition GetOrCreateObjectDefinition(AbstractApplicationContext ctx, AutowireTargetPropertyInfo info)
        {
            if (ctx.IsObjectNameInUse(info.ObjectInfo.id))
            {
                return ctx.GetObjectDefinition(info.ObjectInfo.id);
            }
            else
            {
                AbstractObjectDefinition objectDefinition = fac.CreateObjectDefinition(info.ObjectInfo.type, null, AppDomain.CurrentDomain);
                objectDefinition.IsSingleton = info.ObjectInfo.singleton;
                ctx.ObjectFactory.RegisterObjectDefinition(info.ObjectInfo.id, objectDefinition);
                return ctx.GetObjectDefinition(info.ObjectInfo.id);
            }
        }

        private static IEnumerable<AutowireTargetPropertyInfo> GetAutowireTargetPropertyInfo(Type type, AutowireTargetPropertyInfo parent = null)
        {
            return type.GetProperties().Where(prop => prop.IsAttributeDefined<AutowireAttribute>()).Select(prop => new AutowireTargetPropertyInfo(prop, parent));
        }
    }
}