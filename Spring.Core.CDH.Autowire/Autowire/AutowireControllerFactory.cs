using Spring.Context.Support;
using System;
using System.Globalization;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;

namespace Spring.Core.CDH.Autowire
{
    public class AutowireControllerFactory : DefaultControllerFactory
    {
        private static object _lock = new object();

        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            IController controller = null;
            string area = (requestContext.RouteData.DataTokens["area"] ?? string.Empty).ToString();
            string contextName = $"{area}{(string.IsNullOrEmpty(area) ? string.Empty : ".")}{controllerName}Controller";

            var ctx = ContextRegistry.GetContext();

            if (!ctx.ContainsObjectDefinition(contextName))
            {
                lock (_lock)
                {
                    if (!ctx.ContainsObjectDefinition(contextName))
                    {
                        var controllerType = base.GetControllerType(requestContext, controllerName);
                        var autowireControllerAttribute = controllerType.GetCustomAttribute<AutowireControllerAttribute>();
                        if (autowireControllerAttribute != null)
                        {
                            var autowireAttribute = new AutowireAttribute();
                            autowireAttribute.ContextName = contextName;
                            autowireAttribute.Type = controllerType;
                            autowireAttribute.Singleton = autowireControllerAttribute.Singleton;

                            var autowireTargetProperty = new SpringAutowire.AutowireTargetProperty();
                            autowireTargetProperty.Prop = new ControllerPropertyInfo(autowireAttribute);
                            autowireTargetProperty.AutowireAttr = autowireAttribute;

                            SpringAutowire.CreateObjectDefinition(ctx as AbstractApplicationContext, autowireTargetProperty);
                        }
                    }
                }
            }

            if (ctx.ContainsObject(contextName))
            {
                controller = (IController)ctx.GetObject(contextName);
            }
            else
            {
                controller = base.CreateController(requestContext, controllerName);
            }

            return controller;
        }

        internal class ControllerPropertyInfo : PropertyInfo
        {
            private AutowireAttribute AutowireAttribute;

            public ControllerPropertyInfo(AutowireAttribute autowireAttribute)
            {
                AutowireAttribute = autowireAttribute;
            }

            public override PropertyAttributes Attributes
            {
                get
                {
                    return PropertyAttributes.None;
                }
            }

            public override bool CanRead
            {
                get
                {
                    return true;
                }
            }

            public override bool CanWrite
            {
                get
                {
                    return false;
                }
            }

            public override Type DeclaringType
            {
                get
                {
                    return null;
                }
            }

            public override string Name
            {
                get
                {
                    return AutowireAttribute.ContextName;
                }
            }

            public override Type PropertyType
            {
                get
                {
                    return AutowireAttribute.Type;
                }
            }

            public override Type ReflectedType
            {
                get
                {
                    return null;
                }
            }

            public override MethodInfo[] GetAccessors(bool nonPublic)
            {
                return new MethodInfo[] { };
            }

            public override object[] GetCustomAttributes(bool inherit)
            {
                return new Attribute[] { };
            }

            public override object[] GetCustomAttributes(Type attributeType, bool inherit)
            {
                return new Attribute[] { };
            }

            public override MethodInfo GetGetMethod(bool nonPublic)
            {
                return null;
            }

            public override ParameterInfo[] GetIndexParameters()
            {
                return new ParameterInfo[] { };
            }

            public override MethodInfo GetSetMethod(bool nonPublic)
            {
                return null;
            }

            public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
            {
                return null;
            }

            public override bool IsDefined(Type attributeType, bool inherit)
            {
                return false;
            }

            public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
            {
            }
        }
    }
}