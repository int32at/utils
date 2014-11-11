using System.Collections.Generic;
using System.Net;
using int32.Utils.Web.WebServer.Processors.Contracts;

namespace int32.Utils.Web.WebServer
{
    public abstract class Bootstrapper
    {
        public string Url { get; set; }
        public string Root { get; set; }
        public AuthenticationSchemes Authentication { get; set; }
        public List<IRequestProcessor> Processors { get; set; }

        protected Bootstrapper()
        {
            Authentication = AuthenticationSchemes.Anonymous;
            Processors = new List<IRequestProcessor>();
        }
    }
}
