using System;
using System.IO;
using System.Net;
using int32.Utils.Core.Extensions;
using int32.Utils.Core.Generic.ViewEngine;
using int32.Utils.Web.WebServer.Controller;
using int32.Utils.Web.WebServer.Processors.Contracts;
using WebResponse = int32.Utils.Web.WebServer.Controller.WebResponse;

namespace int32.Utils.Web.WebServer.Processors
{
    public class WebRequestProcessor : IRequestProcessor
    {
        public bool Process(HttpListenerContext context)
        {
            try
            {
                var file = context.GetUrl();

                if (file.IsNullOrEmpty())
                    file = "index.html";

                if (!Path.HasExtension(file))
                    return false;

                string result = string.Empty;

                var data = File.ReadAllText(file);

                var controller = WebServerHelper.FindController<WebController>(file);

                //when a controller and model is found, use the view engine to render the html
                if (controller != null && !ReferenceEquals(null, controller.Get))
                {
                    var parameters = WebServerHelper.GetQueryString(context);
                    result = ViewEngine.Instance.Render(data, controller.Get(parameters));
                }
                    //otherwise just return the data that was found
                else result = data;

                context.SetResponse(new WebResponse(result).ToString());

                return true;
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.SetResponse(new WebResponse(ex).ToString());
            }

            return false;
        }
    }
}
