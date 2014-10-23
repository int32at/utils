using System.Collections.Generic;
using System.Net;
using int32.Utils.Web.WebServer.Controller.Contracts;

namespace int32.Utils.Web.WebServer.Processors.Contracts
{
    public interface IRequestProcessor
    {
        List<IController> Cache { get; set; }
        bool Process(HttpListenerContext context);
    }
}
