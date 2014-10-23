using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using int32.Utils.Web.WebServer.Controller.Contracts;
using int32.Utils.Web.WebServer.Processors.Contracts;

namespace int32.Utils.Web.WebServer.Processors
{
    public abstract class BaseRequestProcessor : IRequestProcessor
    {
        public string Root { get; set; }

        protected BaseRequestProcessor(string dir)
        {
            Cache = new List<IController>();
            Root = dir;
        }

        protected T GetController<T>(string url) where T : IController
        {
            var cache = Cache.FirstOrDefault(i => i != null && i.Path == url);

            if (cache != null)
                return (T) cache;

            var ctrl = WebServerHelper.FindController<T>(url);
            Cache.Add(ctrl);

            return ctrl;
        }

        public List<IController> Cache { get; set; }

        public abstract bool Process(HttpListenerContext context);
    }
}
