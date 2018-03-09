using Spring.Context.Support;
using Spring.Core.IO;

namespace Spring.Core.CDH.Autowire
{
    public static class ContextRegister
    {
        private static object _lock = new object();

        public static void RegisterContext(string xmlContext, string rootContextName = SpringAutowire.DefaultRootContextName)
        {
            if (!ContextRegistry.IsContextRegistered(rootContextName))
            {
                lock (_lock)
                {
                    if (!ContextRegistry.IsContextRegistered(rootContextName))
                    {
                        XmlApplicationContext ctx = new XmlApplicationContext(new XmlApplicationContextArgs
                        {
                            Name = rootContextName,
                            ConfigurationResources = new IResource[] { new StringResource(xmlContext) }
                        });
                        ContextRegistry.RegisterContext(ctx);
                    }
                }
            }
        }
    }
}