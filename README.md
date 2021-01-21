# Spring.Core.CDH.Autowire

Use the autowire attribute on the property.

Caution!) This library is designed to avoid declaring service objects in a way that uses XML.

You are not implementing a separate custom ObjectDefinitionScanner.

Use it simply when you want to reduce XML declarations.

```cs
[Autowire]
public class AnyController : Controller
{
    [Autowire]
    public IMyDao MyDao { get; set; }
    
    [Autowire(Type = typeof(UserNameUpdaeService))]
    public IAnyUpdateService UserNameUpdaeService { get; set; }
}

public class MyDao : AdoDaoSupport, IMyDao
{
}

public class UserNameUpdaeService : IAnyUpdateService
{
    [Autowire]
    public IMyDao MyDao { get; set; }
}
```

## Used

Basically you should call the Autowire function.
'SpringAutowire.Autowire' registers properties to be autowired in the SpringContext configuration.
It either initializes a property that declares an Autowire attribute on the instance or returns an instance of the type.

```cs
var anyController = new AnyController();
SpringAutowire.Autowire(anyController);

var anyController = SpringAutowire.Autowire(typeof(AnyController));
var anyController = SpringAutowire.Autowire<AnyController>();
```

## examples

### ASP.NET

Register logic to call SpringAutowire.Autowire in controller factory.

```cs
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
```

### HOW ADO.NET AdoTemplate Property Change
```xml
<object id="AdoTemplate" type="Spring.Data.Generic.AdoTemplate, Spring.Data">
  <property name="DbProvider" ref="DbProvider" />
  <property name="DataReaderWrapperType" value="Spring.Data.Support.NullMappingDataReader, Spring.Data" />
  <property name="CommandTimeout" value="60" />
</object>
<object id="AdoTemplate2" type="Spring.Data.Generic.AdoTemplate, Spring.Data">
  <property name="DbProvider" ref="DbProvider" />
  <property name="DataReaderWrapperType" value="Spring.Data.Support.NullMappingDataReader, Spring.Data" />
  <property name="CommandTimeout" value="60" />
</object>
```
```cs
public class AnyController : Controller
{
   [Autowire] // <-- MyDao.AdoTemplate == <object id="AdoTemplate" ... />, AdoTemplate is Default.
   public IMyDao MyDao { get; set; }
   [Autowire, AdoTemplate("AdoTemplate2")] // <-- MyDao.AdoTemplate == <object id="AdoTemplate2" ... />
   public IMyDao MyDao { get; set; }
}
```

