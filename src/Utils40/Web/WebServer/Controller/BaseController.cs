using System;
using int32.Utils.Web.WebServer.Controller.Contracts;

namespace int32.Utils.Web.WebServer.Controller
{
    public abstract class BaseController : IController
    {
        public Func<dynamic, dynamic> Get { get; set; }
        public string Path { get; set; }
    }
}
