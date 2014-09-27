utils
=====

* Configuration
* Domain
* Aggregator
* Logging
* Extension Methods
* Design Patterns

##Configuration
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

##Domain
The `Domain` class has nothing to do with internet - it's more like an environmental object that allows you to distinquish between systems and load config files accordingly (for example). Following domains are available:

* Development (default)
* Integration
* QualityAssurance
* Staging
* Production

**Note**: By default, `Domain.Current.Config` will be loaded with the `appSettings` section of the current config file (app.config or web.config).

````cs
/* this assumes there is an key 'MyKey' present in the app.config/web.config file
 * all keys from the appSettings section will be loaded into the Config object automatically
 * on requesting the Current object */
var setting = Domain.Current.Config["MyKey"].As<int>();

//sets the current domain to production
Domain.SetTo<Production>();

/* react to switching of domains
 * this uses the custom .Switch() extension method (also in this framework)
 * but obviously it would also be possible to use a normal method instead */
Domain.OnChanged = () => Domain.Current.Switch()
    .Case<Development>(() => Domain.Current.Config["MyUrl"] = "http://dev")
    .Case<Production>(() => Domain.Current.Config["MyUrl"] = "http://prod");
```

##Aggregator
The `Aggregator` singleton follows the Event Aggregator pattern and allows you to send "messages" between different parts of your application. There is no specifc way how/where to use it - but it's written very generic and can be used basically anywhere. You can publish and subscribe to events, it is even possible to subscribe to one type of event multiple times which means you can update parts of your application (like ViewModels) by just publishing 1 single event.

```cs
//push a new message to all subscribers
Aggregator.Instance.Publish(new SampleEvent { Data = 3});

/* some other part of your application;
 * it is even possible to subscribe to the same event multiple times */
Aggregator.Instance.Subbscribe<SampleEvent>(message => {
    //receive the message and print it
    Console.WriteLine(message.Data);
});

```


##Logging
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

##Extension Methods
In this framework, Extension Methods are used very heavily. There are a lot of inbuilt functionallities (especially generic ones) that are powering different parts and features as well. Here is a list of all available methods based by type:

####DateTime:
Name | Description
--- | --- 
DateTime Tomorrow(); | gets yesterdays date
DateTime Yesterday(); |gets tomorrows date
```cs
var tomorrow = DateTime.Now.Tomorrow();
var yesterday = DateTime.Now.Yesterday();
```

####string:
Name | Description
--- | --- 
T FromJSON<T>(); | converts an JSON string into T
bool Matches("Some Regex Exp"); | whether the text matches the given Regex 
void Matches("Some Regex Exp", Action); | whether the text matches the given Regex, then execute action
```cs
var sampleText = "sample 33 xxx 322";

//text matches regular expression
if(sampleText.Matches(@"\d+"))
{
    //do something
}

//execute action when it matches...
sampleText.Matches(@"\d+", () => Console.WriteLine("matches"));
```

####object:
Name | Description
--- | --- 
* string ToJSON(); | converts an object to an JSON string
* T As<T>(); | converts an object into T
* bool Is<T>(); | whether the object's type is T
```cs
var model = new SampleModel() { Age = 23, Title = "Andreas", Type = ModelType.Example };
//convert the object to an json string
var s = model.ToJSON();

//convert string to int
var a = "3".As<int>();

//easy type check
var b = model.Is<SampleModel>();
```


####Generics:
Name | Description
--- | --- 
T ThrowIfNull<T>(name); | Throws an ArgumentNullException for the given name, when the object is null
bool IsNull<T>(); | whether the object is null
T And<T>(); | doesn't actually do anything, expect making the syntax more readable
T And<T>(Action<T>); | simple executing an action; makes it more readable
T And<T>(Func<T, T>); | simple executing an action; makes it more readable
T IfNull<T>(Action); | if the object is null, then execute function
T IfNull<T>(Action<T>); | if the object is null, then execute function + paramter
T IfNull<T>(Func<T>); | if the object is null, then execute function + return
T IfNull<T>(Func<T, T>); | if the object is null, then execute function + parameter + return
T IfNotNull<T>(Action); | if the object is not null, then execute function
T IfNotNull<T>(Action<T>); | if the object is not null, then execute function + parameter
T IfNotNull<T>(Func<T>); | if the object is not null, then execute function + return 
T IfNotNull<T>(Func<T, T>); | if the object is not null, then execute function + parameter + return
bool In<T>(list); | whether the object exists in the given list
bool Between<T>(lower, upper) | whether the object exists in the certain range
void ForEach<T>(Action) | executes function for every item in a list
void Remove<T>(Func<T, bool>); | removes items from an list based on a query
void Update<T>(Action<T>); | updates all items in a list with the Action
Switch<T> Switch<T>(); | begins the typeswitch
```cs
var o1 = null;

var isNull = o1.IsNull();
o1.ThrowIfNull("o1"); //throws exception when o1 is null

//samples for the IfNull/IfNotNull functionallity
o1.IfNull(() => Console.WriteLine("o1 is null"));
o1.IfNotNull(i => Console.WriteLine("o1 is not null but is: " + i));

var numbers = new List<int> { 1, 2, 3, 4, 5 };
3.In(numbers); //true
new [] { 3, 5 }.In(numbers); //true
new [] { 7, 1 }.In(numbers); //false, both numbers must be in range

3.Between(1, 5); //true
3.Between(1, 2); //false

//check whether every number is between 1 and 10
numbers.ForEach(i => i.Between(1, 10));

//removes all numbers from the array that are greather than 3
numbers.Remove(i => i > 3);

var models = new List<SampleModel>
{
    new SampleModel() {Age = 23},
    new SampleModel() {Age = 12},
    new SampleModel() {Age = 33}
};

//update all items at once
models.Update(model => model.Type = ModelType.Sample);

//switch statement based on type
Domain.Current.Switch()
    .Case<Development>(() => Domain.Current.Config.Set("Url", "http://dev"))
    .Case<Production>(() => Domain.Current.Config.Set("Url", "http://prod"))
    .Default(() => Domain.Current.Config.Load());
```
