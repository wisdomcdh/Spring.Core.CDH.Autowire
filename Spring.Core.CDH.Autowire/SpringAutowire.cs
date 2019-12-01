using Spring.Context.Support;
using Spring.Objects;
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
        /// 전달된 <paramref name="instance"/> 인스턴스에서 Autowire 특성이 선언된 속성을 Spring Context에 등록하고, 주입합니다.
        /// </summary>
        /// <param name="instance">직접 생성한 인스턴스</param>
        /// <param name="rootContextName"></param>
        public static void Autowire(object instance, string rootContextName = DefaultRootContextName)
        {
            AbstractApplicationContext ctx = GetApplicationContext(rootContextName);

            foreach (AutowireTargetPropertyInfo info in AutowireTargetPropertyGetter.GetAutowireTargetPropertiesInfo(instance.GetType(), null))
            {
                CreateObjectDefinition(ctx, info);
                info.PropertyInfo.SetValue(instance, ctx.GetObject(info.ObjectInfo.Id));
            }
        }

        /// <summary>
        /// <paramref name="type"/> 유형과 <paramref name="autowireAttribute"/>의 Autowire 특성을 분석하여 Spring Context에 등록하고, 객체를 생성하여 반환합니다.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="autowireAttribute"></param>
        /// <param name="rootContextName"></param>
        /// <returns></returns>
        public static object Autowire(Type type, AutowireAttribute autowireAttribute, string rootContextName = DefaultRootContextName)
        {
            AbstractApplicationContext ctx = GetApplicationContext(rootContextName);
            if (autowireAttribute == null)
            {
                throw new NullReferenceException("AutowireAttribute missing");
            }

            if (autowireAttribute.Type == null)
            {
                autowireAttribute.Type = type;
            }
            if (string.IsNullOrEmpty(autowireAttribute.ContextName))
            {
                autowireAttribute.ContextName = $"[{type.GetShortAssemblyName()}]{$"Singleton={autowireAttribute.Singleton.ToString().ToLower()}".ToMD5String()}";
            }

            var info = new AutowireTargetPropertyInfo(autowireAttribute, type);
            CreateObjectDefinition(ctx, info);
            return ctx.GetObject(info.ObjectInfo.Id);
        }

        /// <summary>
        /// 전달된 <paramref name="type"/>유형에 선언된 Autowire 특성을 분석하여 Spring Context에 등록하고, 객체를 생성하여 반환합니다.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="rootContextName"></param>
        /// <returns></returns>
        public static object Autowire(Type type, string rootContextName = DefaultRootContextName)
        {
            AutowireAttribute autowireAttribute = type.GetCustomAttribute<AutowireAttribute>();
            if (autowireAttribute == null)
            {
                autowireAttribute = new AutowireAttribute
                {
                    ContextName = $"[{type.GetShortAssemblyName()}]{"Singleton=true".ToMD5String()}",
                    Type = type,
                    Singleton = false
                };
            }
            return Autowire(type, autowireAttribute, rootContextName);
        }

        /// <summary>
        /// 유형 <typeparamref name="T"/>을 전달된 <paramref name="autowireAttribute"/> Autowire 특성을 분석하여 Spring Context에 등록하고, 객체를 생성하여 반환합니다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="autowireAttribute"></param>
        /// <param name="rootContextName"></param>
        /// <returns></returns>
        public static T Autowire<T>(AutowireAttribute autowireAttribute, string rootContextName = DefaultRootContextName)
        {
            return (T)Autowire(typeof(T), autowireAttribute, rootContextName);
        }

        /// <summary>
        /// 유형 <typeparamref name="T"/>을 선언된 Autowire 특성을 분석하여 Spring Context에 등록하고, 객체를 생성하여 반환합니다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rootContextName"></param>
        /// <returns></returns>
        public static T Autowire<T>(string rootContextName = DefaultRootContextName)
        {
            return (T)Autowire(typeof(T), rootContextName);
        }

        /// <summary>
        /// Spring AbstractApplicationContext 을 가져옵니다.
        /// </summary>
        /// <param name="rootContextName"></param>
        /// <returns></returns>
        private static AbstractApplicationContext GetApplicationContext(string rootContextName)
        {
            return ContextRegistry.GetContext(rootContextName) as AbstractApplicationContext;
        }

        /// <summary>
        /// <paramref name="info"/> 정보를 분석하여 Spring Context에 등록합니다.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="info"></param>
        private static void CreateObjectDefinition(AbstractApplicationContext ctx, AutowireTargetPropertyInfo info)
        {
            if (!ctx.IsObjectNameInUse(info.ObjectInfo.Id))
            {
                lock (_lockMap.GetOrAdd(info.ObjectInfo.Id, new object()))
                {
                    if (!ctx.IsObjectNameInUse(info.ObjectInfo.Id))
                    {
                        IObjectDefinition objectDefinition = GetOrCreateObjectDefinition(ctx, info);
                        // 병힙정의 찾기
                        IObjectDefinition mergeTargetObjectDefinition = null;
                        if (!string.IsNullOrEmpty(info.ObjectInfo.PropertyDefinedAutowireAttribute.MergeBase) && ctx.IsObjectNameInUse(info.ObjectInfo.PropertyDefinedAutowireAttribute.MergeBase))
                        {
                            mergeTargetObjectDefinition = ctx.GetObjectDefinition(info.ObjectInfo.PropertyDefinedAutowireAttribute.MergeBase);
                        }

                        // 병합정의가 있는경우 우선 처리
                        if (mergeTargetObjectDefinition != null)
                        {
                            foreach (PropertyValue propertyValue in mergeTargetObjectDefinition.PropertyValues)
                            {
                                if (info.ObjectInfo.ObjectType.GetProperty(propertyValue.Name) != null)
                                {
                                    // 현재 정의에 없다면 등록한다.
                                    if (!objectDefinition.PropertyValues.Contains(propertyValue.Name))
                                    {
                                        objectDefinition.PropertyValues.Add(mergeTargetObjectDefinition.PropertyValues.GetPropertyValue(propertyValue.Name));
                                    }
                                }
                            }
                        }

                        // 현재 정의의 객체나 프로퍼티에 선언된 Property 특성으로 부터 확인된 속성정의를 우선 등록한다.
                        foreach (var confirmedPropAttr in info.ObjectInfo.ConfirmedPropertyAttributes)
                        {
                            // 현재 정의에 이미 존재한다면, 제거하고 새로 등록한다.
                            if (objectDefinition.PropertyValues.Contains(confirmedPropAttr.Name))
                            {
                                objectDefinition.PropertyValues.Remove(objectDefinition.PropertyValues.GetPropertyValue(confirmedPropAttr.Name));
                            }
                            objectDefinition.PropertyValues.Add(confirmedPropAttr.Name, new RuntimeObjectReference(confirmedPropAttr.Ref));
                        }

                        // 현재 정의의 객체에 Autowire 특성을 가지고 있는 프로퍼티
                        foreach (AutowireTargetPropertyInfo inInfo in AutowireTargetPropertyGetter.GetAutowireTargetPropertiesInfo(info.ObjectInfo.ObjectType, info))
                        {
                            // 이미 정의되어 있으나 이 것이 MergeBase에 의한 것이라면 지운다
                            if (objectDefinition.PropertyValues.Contains(inInfo.PropertyInfo.Name))
                            {
                                if (mergeTargetObjectDefinition != null && mergeTargetObjectDefinition.PropertyValues.Contains(inInfo.PropertyInfo.Name))
                                {
                                    objectDefinition.PropertyValues.Remove(objectDefinition.PropertyValues.GetPropertyValue(inInfo.PropertyInfo.Name));
                                }
                            }

                            // 핸재 정의에 존재하지 않는다면 정의를 새로 생성한다.
                            if (!objectDefinition.PropertyValues.Contains(inInfo.PropertyInfo.Name))
                            {
                                CreateObjectDefinition(ctx, inInfo);
                                objectDefinition.PropertyValues.Add(inInfo.PropertyInfo.Name, new RuntimeObjectReference(inInfo.ObjectInfo.Id));
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Spring ObjectDefinition을 가져오거나 생성합니다.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="info"></param>
        /// <returns></returns>
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