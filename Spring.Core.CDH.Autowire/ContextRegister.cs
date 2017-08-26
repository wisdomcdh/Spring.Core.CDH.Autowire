using Spring.Context.Support;
using Spring.Core.IO;
using Spring.Objects.Factory.Xml;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Spring.Core.CDH.Autowire
{
    public static class ContextRegister
    {
        private static object _lock = new object();

        public static void RegisterContext(string context, string contextName = SpringAutowire.DefaultRootContextName)
        {
            if (!ContextRegistry.IsContextRegistered(contextName))
            {
                lock (_lock)
                {
                    if (!ContextRegistry.IsContextRegistered(contextName))
                    {
                        GenericApplicationContext ctx = new GenericApplicationContext();
                        ctx.Name = contextName;
                        XmlObjectDefinitionReader reader = new XmlObjectDefinitionReader(ctx);
                        reader.LoadObjectDefinitions(new StringResource(context));
                        ContextRegistry.RegisterContext(ctx);
                    }
                }
            }
        }
    }
}