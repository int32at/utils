utils
=====

My .NET Utils Framework

Includes:
* Logging
* Configuration
* Event Aggregator
* Domains (Environments) Specifics
* Service Helpers
* Useful Extension Methods

See the `Test` project on how to use.

###Logging
Logging is relativly easy and straight forward. It is not a fullblown logging framework like NLog or log4net or similar - it is supposed to be fast and easy to use.

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

By default, the `ConsoleLogger` and `FileLogger` are available, but you can easily roll your own by implementing the `ILogger` interface like this:

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

