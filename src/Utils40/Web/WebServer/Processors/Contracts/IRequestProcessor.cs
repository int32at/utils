using System.Net;

namespace int32.Utils.Web.WebServer.Processors.Contracts
{
    public interface IRequestProcessor
    {
        bool Process(HttpListenerContext context);
    }
}
