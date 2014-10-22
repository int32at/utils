using System;

namespace int32.Utils.Web.WebServer.Controller
{
    public abstract class WebController
    {
        public Func<dynamic, dynamic> Get { get; set; }

        public string Path { get; set; }

        protected WebController(string viewName)
        {
            Path = viewName;
        }
    }
}
