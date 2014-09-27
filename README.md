utils
=====

* Configuration
* Logging

###Configuration
The `Config` class was designed to be simple and easy to use. Here are a few quick examples:

```cs
var config = new Config();

//add an entry
config.Set(new ConfigEntry("MyKey", 3));

//get the entry
var a = config.Get<int>("MyKey"); //use the default get method
var b = config["MyKey"].As<int>(); //use the As extension method

config.Remove("MyKey"); //removes the netry

//throws exception
var c = config["MyKey"];

//load the config from app.config or web.config
var cfg = Config.Create();
```

###Logging
Logging is relativly easy and straight forward. It is not a fullblown logging framework like NLog or log4net or similar - it is supposed to be fast and easy to use.

####Usage
```cs
using (var logger = new Logger(new ConsoleLogger()))
{
    logger.Warn("WarnMessage");
    logger.Warn("{0}{1}", "Warn", "Message"); 

    logger.Info("InfoMessage"); 
    logger.Info("{0}{1}", "Info", "Message");
    
    logger.Error(new ArgumentOutOfRangeException());
}
```

By default, the `ConsoleLogger` and `FileLogger` are available.

####Extending
You can easily roll your own logger by implementing the `ILogger` interface like this:

```cs
public class MockLogger : ILogger
{
    public void Dispose()
    {
    }

    public void Info(InfoMessage msg)
    {
    }

    public void Warn(WarnMessage msg)
    {
    }

    public void Error(ErrorMessage msg)
    {
    }
}
```
The `Logger` class also allows you to load multiple loggers at once, by overloading the constructor:

```cs
using (var logger = new Logger(new ConsoleLogger(), new FileLogger(@"C:\log.txt"), new MockLogger())))
{
    //call to logger.Info("message") now logs on all 3
}
```
####Configuraiton
The logger obviously also can be configured, here is a brief explanation:
```cs
logger.Config.EnableDebugging = true; //default; allows additional debugging information on Error()
logger.Config.EnableErrors = true; //default; allows logging of Error messages, if false will not log Error()'s
logger.Config.EnableWarnings = true; //default; same as EnableErrors but Warn() instead
logger.Config.EnableInfos = true; //default; same as EnableErrors but Warn() instead
logger.Config.Format = "{0}\t{1}\t{2}"; //default Format = DateTime.Now\tLOGLEVEL\tmessage
```

