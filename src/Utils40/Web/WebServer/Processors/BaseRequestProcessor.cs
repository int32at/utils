using System.Collections.Generic;
using System.Linq;
using System.Net;
using int32.Utils.Web.WebServer.Controller.Contracts;
using int32.Utils.Web.WebServer.Processors.Contracts;

namespace int32.Utils.Web.WebServer.Processors
{
    public abstract class BaseRequestProcessor : IRequestProcessor
    {
        public List<IController> Cache { get; set; }
        public abstract bool Process(HttpListenerContext context);

        protected BaseRequestProcessor()
        {
            Cache = new List<IController>();
        }

        protected T GetController<T>(string url) where T : IController
        {
            var cache = Cache.FirstOrDefault(i => i != null && i.Path == url);

            if (cache != null)
                return (T)cache;

            var ctrl = WebServerHelper.FindController<T>(url);

            if(ctrl != null)
                Cache.Add(ctrl);

            return ctrl;
        }
    }
}
