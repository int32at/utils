using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Dynamic;
using System.Net;
using int32.Utils.Core.Extensions;
using int32.Utils.Core.Generic.Types;
using int32.Utils.Web.WebServer.Controller.Contracts;

namespace int32.Utils.Web.WebServer
{
    internal static class WebServerHelper
    {
        internal static TypeScanner Scanner = new TypeScanner();

        internal static T FindController<T>(string root) where T : IController
        {
            try
            {
                var controllers = new List<Type>();
                controllers.AddRange(Scanner.Scan<T>());

                foreach (var controller in controllers)
                {
                    var ctrl = (T) Activator.CreateInstance(controller);

                    if (ctrl != null && ctrl.Path.Equals(root))
                        return ctrl;
                }
            }
            catch
            {
            }

            return ObjectExtensions.As<T>(null);
        }

        internal static dynamic GetQueryString(HttpListenerContext context)
        {
            return BuildDynamicKeyValueObject(context.Request.QueryString);
        }

        internal static dynamic BuildDynamicKeyValueObject(NameValueCollection query)
        {
            var dict = new Collection<KeyValuePair<string, object>>();

            foreach (var k in query.AllKeys)
                dict.Add(new KeyValuePair<string, object>(k, query[k]));

            var dyn = new ExpandoObject();
            var coll = (ICollection<KeyValuePair<string, object>>) dyn;

            foreach (var kvp in dict)
            {
                coll.Add(kvp);
            }

            return dyn;
        }

        internal static dynamic Merge(dynamic query, Dictionary<string, object> param)
        {
            var coll = (ICollection<KeyValuePair<string, object>>)query;

            foreach (var key in param)
            {
                coll.Add(new KeyValuePair<string, object>(key.Key, key.Value));
            }

            return query;
        }
    }
}