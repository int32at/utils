using System;
using System.Net;
using System.Security.Authentication;
using System.Security.Principal;
using int32.Utils.Web.WebServer.Controller.Contracts;

namespace int32.Utils.Web.WebServer.Controller
{
    public abstract class BaseController : IController
    {
        public Func<dynamic, dynamic> Get { get; set; }
        public Func<dynamic, dynamic> Post { get; set; }
        public Func<IPrincipal, bool> Auth { get; set; } 
        public string Path { get; set; }

        public void CheckAuth(HttpListenerContext context)
        {
            if (!ReferenceEquals(null, Auth) && !Auth(context.User))
                throw new AuthenticationException("not authorized.");
        }
    }
}
