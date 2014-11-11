using System;
using int32.Utils.Core.Domain;
using int32.Utils.Web.WebServer;

namespace int32.Utils.SelfHostedWebApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //WebServer.OnStarted = () => Console.WriteLine("server started at {0}.", WebServer.Config.Url);
                //WebServer.OnStopped = () => Console.WriteLine("server stopped.");
                //WebServer.OnRequestReceived = context => Console.WriteLine("request to {0} {1} {2}", context.Request.HttpMethod, context.Request.ProtocolVersion, context.Request.Url);
                //WebServer.OnRequestProcessed = (processor, context) => Console.WriteLine("request processed by {0}.", processor.GetType().Name);

                new WebServer().Start<Boot>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.Read();
        }
    }

    public class Boot : Bootstrapper
    {
        public Boot()
        {
            Url = Domain.Current.Config["Url"].ToString();
        }
    }
}
