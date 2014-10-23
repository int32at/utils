using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using int32.Utils.Web.WebServer.Processors;
using int32.Utils.Web.WebServer.Processors.Contracts;

namespace int32.Utils.Web.WebServer
{
    public class WebServer : IDisposable
    {
        private readonly HttpListener _listener;

        private static readonly AutoResetEvent ListenForNextRequest
            = new AutoResetEvent(false);

        private List<IRequestProcessor> Processors { get; set; }

        public WebServer(string url)
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add(url + "/");

            Processors = new List<IRequestProcessor>()
            {
                new ApiRequstProcessor(),
                new WebRequestProcessor()
            };

            Start();
        }

        private void Start()
        {
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

        public void Dispose()
        {
            _listener.Stop();
        }
    }
}
