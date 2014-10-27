using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Authentication;
using System.Text.RegularExpressions;
using int32.Utils.Core.Extensions;
using int32.Utils.Web.WebServer.Controller;
using Newtonsoft.Json;

namespace int32.Utils.Web.WebServer.Processors
{
    public class ApiRequestProcessor : BaseRequestProcessor
    {
        public override bool Process(HttpListenerContext context)
        {
            try
            {
                var rootUrl = GetRootUrl(context);

                //search all api controller that are registered to the ROOT url
                //since sub urls can be made using the Get[] object within the constructor of the controller
                var ctrl = GetController<ApiController>(rootUrl);

                if (ctrl != null)
                {
                    ctrl.CheckAuth(context);

                    switch (context.Request.HttpMethod)
                    {
                        case "GET": return ProcessGet(context, rootUrl, ctrl);
                        case "POST": return ProcessPost(context, rootUrl, ctrl);
                    }
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

        private bool ProcessGet(HttpListenerContext context, string root, BaseController controller)
        {
            //pass in the query string data
            var get = WebServerHelper.GetQueryString(context);

            return ProcessRequest(context, root, controller.Get, get);
        }

        private bool ProcessPost(HttpListenerContext context, string root, BaseController controller)
        {
            //pass in the POST data from the request
            var post = context.GetPostData();

            return ProcessRequest(context, root, controller.Post, post);
        }

        private bool ProcessRequest(HttpListenerContext context, string root,
            Dictionary<string, Func<dynamic, dynamic>> method, dynamic param)
        {
            var full = GetFullUrl(context);

            foreach (var m in method)
            {
                //get the complete service url
                var service = RemoveEndingSlash(root + m.Key);

                //parameters from the query string, will be loaded from the matches method
                Dictionary<string, object> paramList;

                //check if the service url matches the full url of the request
                if (!Matches(service, full, out paramList)) continue;

                //merge the post or get objects with the parameter list
                var data = WebServerHelper.Merge(param, paramList);

                //execute the specific method
                var result = m.Value(data);

                //convert the result to json and set the response
                var json = JsonConvert.SerializeObject(new ApiResponse(result));
                context.SetResponse(json);
                context.Response.ContentType = "application/json";
                return true;
            }

            return false;
        }

        private bool Matches(string service, string full, out Dictionary<string, object> param)
        {
            //split the urls into pieces to make it easier to check
            var serviceSplit = service.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
            var fullSplit = full.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            var successCount = 0;

            param = new Dictionary<string, object>();

            //only if the length of the pieces matches, we need to check
            if (serviceSplit.Length == fullSplit.Length)
            {
                for (var i = 0; i < serviceSplit.Length; i++)
                {
                    if (serviceSplit[i] != fullSplit[i])
                    {
                        //when splits are not the same, check for the placeholder i.e {id}
                        if (serviceSplit[i].Matches(@"\{.*?\}"))
                        {
                            //if it does match, add the matches with regex
                            var matches = Regex.Matches(serviceSplit[i], @"\{.*?\}");

                            foreach (Match match in matches)
                            {
                                //remove the brackets from {id} so only id is left
                                var val = match.Value;
                                val = val.Remove(0, 1);
                                val = val.Remove(val.Length - 1, 1);

                                //convert the placeholder to integer, since in this release only int is supported
                                param.Add(val, Convert.ToInt32(fullSplit[i]));
                            }

                            successCount++;
                        }
                    }
                    else
                        successCount++;
                }
            }

            return successCount == fullSplit.Length;
        }

        private string GetRootUrl(HttpListenerContext context)
        {
            var url = context.GetUrl();
            var root =  url.Segments.Length > 1 ? "/" + url.Segments[1] : url.Segments[0];

            return RemoveEndingSlash(root);
        }

        private string GetFullUrl(HttpListenerContext context)
        {
            return context.GetUrl().AbsolutePath;
        }

        private string RemoveEndingSlash(string path)
        {
            if (path.Length > 1 && path.EndsWith("/"))
               path = path.Remove(path.Length - 1, 1);

            return path;
        }
    } 
}
