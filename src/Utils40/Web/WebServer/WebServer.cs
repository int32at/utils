using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using int32.Utils.Web.WebServer.Processors;
using int32.Utils.Web.WebServer.Processors.Contracts;

namespace int32.Utils.Web.WebServer
{
    public class WebServer
    {
        private readonly HttpListener _listener;

        private static readonly AutoResetEvent ListenForNextRequest
            = new AutoResetEvent(false);

        public string Root { get; set; }

        public AuthenticationSchemes Authentication { get; set; }

        private List<IRequestProcessor> Processors { get; set; }

        public WebServer(string url) : this(url, Environment.CurrentDirectory) { }

        public WebServer(string url, string root)
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add(url + "/");

            Root = root;

            Authentication = AuthenticationSchemes.Anonymous;
        }

        public WebServer Start()
        {
            Processors = new List<IRequestProcessor>()
            {
                new ApiRequestProcessor(),
                new WebRequestProcessor()
            };

            _listener.AuthenticationSchemes = Authentication;
            _listener.Start();

            ThreadPool.QueueUserWorkItem(state =>
            {
                while (_listener.IsListening)
                {
                    _listener.BeginGetContext(result =>
                    {
                        var listener = result.AsyncState as HttpListener;

                        try
                        {
                            if (listener == null)
                                return;

                            var context = listener.EndGetContext(result);

                            ProcessRequest(context);
                        }
                        catch
                        {
                        }
                        finally
                        {
                            ListenForNextRequest.Set();
                        }
                    }, _listener);

                    ListenForNextRequest.WaitOne();
                }
            });

            return this;
        }

        private void ProcessRequest(HttpListenerContext context)
        {
            try
            {
                foreach (var processor in Processors)
                {
                    //when the first processor managed the request
                    //do not proceed with other processors
                    if (processor.Process(context))
                        break;
                }
            }
            finally
            {
                //close the response
                context.Response.OutputStream.Close();
            }
        }

        public void Stop()
        {
            _listener.Stop();
        }
    }
}