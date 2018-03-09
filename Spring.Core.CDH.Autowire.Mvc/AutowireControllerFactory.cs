using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;

namespace Spring.Core.CDH.Autowire
{
    public class AutowireControllerFactory : DefaultControllerFactory
    {
        private string rootContextName;

        public AutowireControllerFactory()
        {
            rootContextName = SpringAutowire.DefaultRootContextName;
        }

        public AutowireControllerFactory(string contextName)
        {
            rootContextName = contextName;
        }

        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            var type = base.GetControllerType(requestContext, controllerName);
            if (type.IsDefined(typeof(AutowireAttribute), true))
            {
                var autowireAttribute = type.GetCustomAttribute<AutowireAttribute>(true);
                return (IController)SpringAutowire.Autowire(type, autowireAttribute, rootContextName);
            }
            else
            {
                return base.CreateController(requestContext, controllerName);
            }
        }
    }
}