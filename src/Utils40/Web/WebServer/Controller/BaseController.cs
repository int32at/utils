using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Authentication;
using System.Security.Principal;
using int32.Utils.Web.WebServer.Controller.Contracts;

namespace int32.Utils.Web.WebServer.Controller
{
    public abstract class BaseController : IController
    {
        public Dictionary<string, Func<dynamic, dynamic>> Get { get; set; }

        public Dictionary<string, Func<dynamic, dynamic>> Post { get; set; }

        public Func<IPrincipal, bool> Auth { get; set; }

        public string Path { get; set; }

        protected BaseController(string rootPath)
        {
            Path = rootPath;
            Get = new Dictionary<string, Func<dynamic, dynamic>>();
            Post = new Dictionary<string, Func<dynamic, dynamic>>();
        }

        protected BaseController() : this("") { }

        public void CheckAuth(HttpListenerContext context)
        {
            if (!ReferenceEquals(null, Auth) && !Auth(context.User))
                throw new AuthenticationException("not authorized.");
        }
    }
}
