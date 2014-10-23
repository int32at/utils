using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Reflection;
using int32.Utils.Core.Extensions;
using int32.Utils.Web.WebServer.Controller.Contracts;

namespace int32.Utils.Web.WebServer
{
    internal static class WebServerHelper
    {
        internal static T FindController<T>(string fileName) where T : IController
        {
            var assembly = Assembly.GetEntryAssembly();

            var controllers = assembly.GetTypes().Where(i => i.BaseType == typeof(T)).ToList();

            foreach (var controller in controllers)
            {
                var ctrl = (T)Activator.CreateInstance(controller);

                if (fileName.EndsWith("/"))
                    fileName = ctrl.Path.TrimEnd('/');

                if (ctrl != null && ctrl.Path.Equals(fileName))
                    return ctrl;
            }

            return ObjectExtensions.As<T>(null);
        }

        internal static dynamic GetQueryString(HttpListenerContext context)
        {
            return BuildQueryStringObject(context.Request.QueryString);
        }

        private static dynamic BuildQueryStringObject(NameValueCollection query)
        {
            var dict = new Collection<KeyValuePair<string, object>>();

            foreach (var k in query.AllKeys)
                dict.Add(new KeyValuePair<string, object>(k, query[k]));

            var dyn = new ExpandoObject();
            var coll = (ICollection<KeyValuePair<string, object>>)dyn;

            foreach (var kvp in dict)
            {
                coll.Add(kvp);
            }

            return dyn;
        }
    }
}
