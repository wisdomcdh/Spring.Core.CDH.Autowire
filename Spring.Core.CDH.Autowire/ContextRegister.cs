using Spring.Context.Support;
using Spring.Core.IO;
using Spring.Objects.Factory.Xml;

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
                        GenericApplicationContext ctx = new GenericApplicationContext();
                        ctx.Name = rootContextName;
                        XmlObjectDefinitionReader reader = new XmlObjectDefinitionReader(ctx);
                        reader.LoadObjectDefinitions(new StringResource(xmlContext));
                        ContextRegistry.RegisterContext(ctx);
                    }
                }
            }
        }
    }
}