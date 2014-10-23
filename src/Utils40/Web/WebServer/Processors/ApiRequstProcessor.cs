﻿using System;
using System.Net;
using int32.Utils.Core.Extensions;
using int32.Utils.Web.WebServer.Controller;
using int32.Utils.Web.WebServer.Processors.Contracts;
using Newtonsoft.Json;

namespace int32.Utils.Web.WebServer.Processors
{
    public class ApiRequstProcessor : IRequestProcessor
    {
        public bool Process(HttpListenerContext context)
        {
            try
            {
                var url = context.GetUrl();

                var controller = WebServerHelper.FindController<ApiController>(url);

                if (controller.IsNotNull() && !ReferenceEquals(null, controller.Get))
                {
                    var parameters = WebServerHelper.GetQueryString(context);
                    var result = controller.Get(parameters);
                    var json = JsonConvert.SerializeObject(new ApiResponse(result));

                    context.SetResponse(json);
                    context.Response.ContentType = "application/json";
                    return true;
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.SetResponse(new ApiResponse(ex).ToJSON());
            }

            return false;
        }
    }
}
