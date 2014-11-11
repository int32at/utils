using System;
using System.IO;
using System.Net;
using System.Windows.Forms;
using int32.Utils.Core.Extensions;
using int32.Utils.Core.Generic.ViewEngine;
using int32.Utils.Web.WebServer.Controller;

namespace int32.Utils.Web.WebServer.Processors
{
    public class WebRequestProcessor : BaseRequestProcessor
    {
        public override bool Process(HttpListenerContext context)
        {
            try
            {
                //get the file name instead of the url


                var file = context.GetUrl().AbsolutePath.Substring(1);

                if (file.IsNullOrEmpty())
                    file = "index.html";

                if (!Path.HasExtension(file))
                    return false;

                string result = "";

                var controller = GetController<WebController>(file);

                if (controller != null)
                    controller.CheckAuth(context);

                var data = File.ReadAllText(file);

                //when a controller and model is found, use the view engine to render the html
                if (controller != null && controller.Get.Count > 0)
                {
                    foreach (var get in controller.Get)
                    {
                        var parameters = WebServerHelper.GetQueryString(context);
                        result = ViewEngine.Instance.Render(data, get.Value(parameters));
                        break;
                    }

                }
                    //otherwise just return the data that was found
                else result = data;

                context.SetResponse(new Controller.WebResponse(result).ToString());
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.SetResponse(new Controller.WebResponse(ex).ToString());
            }

            return true;
        }
    }
}
