using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using int32.Utils.Core.Extensions;
using int32.Utils.Core.Generic.Factory;
using int32.Utils.Core.Generic.ViewEngine;
using int32.Utils.Web.WebServer.Controller;

namespace int32.Utils.Web.WebServer
{
    public class WebServer : IDisposable
    {
        private string _url;
        private readonly HttpListener _listener;

        private static readonly AutoResetEvent ListenForNextRequest 
            = new AutoResetEvent(false);

        public DirectoryInfo Root { get; set; }

        public WebServer(string url)
        {
            _url = url;
            _listener = new HttpListener();
            _listener.Prefixes.Add(url + "/");

            if(Root.IsNull())
                Root = new DirectoryInfo(Directory.GetCurrentDirectory());

            Start();
        }

        private void Start()
        {
            _listener.Start();

            ThreadPool.QueueUserWorkItem(Listen);
        }

        private void Listen(object state)
        {
            while (_listener.IsListening)
            {
                _listener.BeginGetContext(ListenerCallback, _listener);
                ListenForNextRequest.WaitOne();
            }
        }

        private void ListenerCallback(IAsyncResult result)
        {
            var listener = result.AsyncState as HttpListener;

            if (listener == null)
                return;

            try
            {
                var context = listener.EndGetContext(result);
                ProccessRequest(context);
            }
            catch { }
            finally
            {
                ListenForNextRequest.Set();
            }
        }

        private void ProccessRequest(HttpListenerContext context)
        { 
            try
            {
                var fileName = context.Request.Url.AbsolutePath;
                fileName = fileName.Substring(1);

                var data = ServeFile(fileName);

                byte[] buf = Encoding.UTF8.GetBytes(data);
                context.Response.ContentLength64 = buf.Length;
                context.Response.OutputStream.Write(buf, 0, buf.Length);
            }
            catch { }

            context.Response.OutputStream.Close();
        }

        private string ServeFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                fileName = "index.html";

            var data = File.ReadAllText(Path.Combine(Root.FullName, fileName));

            var ctrl = FindWebController(fileName);

            if (!ReferenceEquals(null, ctrl) && !ReferenceEquals(null, ctrl.Get))
                return ViewEngine.Instance.Render(data, ctrl.Get(null));

            return data;
        }

        private WebController FindWebController(string fileName)
        {
            var assembly = Assembly.GetEntryAssembly();

            var controllers = assembly.GetTypes().Where(i => i.BaseType == typeof(WebController)).ToList();

            foreach (var controller in controllers)
            {
                var ctrl = (WebController)Activator.CreateInstance(controller);

                if (ctrl != null && ctrl.Path.Equals(fileName))
                    return ctrl;
            }

            return null;
        }

        public void Dispose()
        {
            _listener.Stop();
        }
    }
}