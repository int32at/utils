using System;
using System.Net;
using System.Security.Authentication;
using int32.Utils.Core.Extensions;
using int32.Utils.Web.WebServer.Controller;
using Newtonsoft.Json;

namespace int32.Utils.Web.WebServer.Processors
{
    public class ApiRequestProcessor : BaseRequestProcessor
    {
        public ApiRequestProcessor(string dir) : base(dir) { }

        public override bool Process(HttpListenerContext context)
        {
            try
            {
                var url = context.GetUrl();

                var controller = GetController<ApiController>(url);

                switch (context.Request.HttpMethod)
                {
                    case "GET":
                        return ProcessGet(context, controller);
                    case "POST":
                        return ProcessPost(context, controller);
                }
            }
            catch (AuthenticationException aex)
            {
                context.Response.StatusCode = 401;
                context.SetResponse(new ApiResponse(aex, 401).ToJSON());
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.SetResponse(new ApiResponse(ex).ToJSON());
            }

            return false;
        }

        private bool ProcessGet(HttpListenerContext context, BaseController controller)
        {
            return ProccessRequest(context, controller, i => i.Get, i =>
            {
                var get = WebServerHelper.GetQueryString(context);
                return i.Get(get);
            });
        }

        private bool ProcessPost(HttpListenerContext context, BaseController controller)
        {
            return ProccessRequest(context, controller, i => i.Post, i =>
            {
                var post = context.GetPostData();
                return i.Post(post);
            });
        }

        private static bool ProccessRequest(HttpListenerContext context, BaseController controller, Func<BaseController, dynamic> check, Func<BaseController, dynamic> action)
        {
            if (!IsValid(controller, check)) return false;

            controller.CheckAuth(context);

            var result = action(controller);
            var json = JsonConvert.SerializeObject(new ApiResponse(result));

            context.SetResponse(json);
            context.Response.ContentType = "application/json";
            return true;
        }

        private static bool IsValid(BaseController controller, Func<BaseController, dynamic> action)
        {
            if (controller.IsNull()) return false;

            var method = action(controller);
            return !ReferenceEquals(null, method);
        }
    }
}
