using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace int32.Utils.Core.Extensions
{
    public static class WebExtensions
    {
        public static Uri GetUrl(this HttpListenerContext context)
        {
            return context.Request.Url;
        }

        public static void SetResponse(this HttpListenerContext context, string data)
        {
            var buf = Encoding.UTF8.GetBytes(data);
            context.Response.ContentLength64 = buf.Length;
            context.Response.OutputStream.Write(buf, 0, buf.Length);
        }

        public static dynamic GetPostData(this HttpListenerContext context)
        {
            dynamic postParams = new ExpandoObject();

            try
            {
                string rawData;

                using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
                    rawData = reader.ReadToEnd();

                if (rawData.IsNullOrEmpty())
                    return postParams;

                if (rawData.IsJson())
                    postParams = rawData.FromJSON<dynamic>();
                else
                {
                    var rawParams = rawData.Split('&');

                    var expandoObject = new ExpandoObject() as IDictionary<string, object>;

                    foreach (var param in rawParams)
                    {
                        var kvPair = param.Split('=');
                        var key = kvPair[0];
                        var value = HttpUtility.UrlDecode(kvPair[1]);
                        expandoObject.Add(key, value);
                    }

                    postParams = expandoObject;
                }
            }
            catch { }

            return postParams;
        }
    }
}