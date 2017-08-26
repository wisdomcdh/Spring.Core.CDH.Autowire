using System.Web.Mvc;
using System.Web.Routing;

namespace Spring.Core.CDH.Autowire
{
    public class AutowireControllerFactory : DefaultControllerFactory
    {
        private static object _lock = new object();
        private string rootContextName;

        public AutowireControllerFactory(string contextName = SpringAutowire.DefaultRootContextName)
        {
            rootContextName = contextName;
        }

        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            var controller = base.CreateController(requestContext, controllerName);
            SpringAutowire.Autowire(controller, rootContextName);
            return controller;
        }
    }
}