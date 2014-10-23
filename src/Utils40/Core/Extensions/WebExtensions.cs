using System.Net;
using System.Text;

namespace int32.Utils.Core.Extensions
{
    public static class WebExtensions
    {
        public static string GetUrl(this HttpListenerContext context)
        {
            return context.Request.Url.AbsolutePath.Substring(1);
        }

        public static void SetResponse(this HttpListenerContext context, string data)
        {
            var buf = Encoding.UTF8.GetBytes(data);
            context.Response.ContentLength64 = buf.Length;
            context.Response.OutputStream.Write(buf, 0, buf.Length);
        }
    }
}
