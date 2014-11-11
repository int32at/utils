using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using int32.Utils.Core.Extensions;
using int32.Utils.Core.Generic.Factory;
using int32.Utils.Web.WebServer.Processors;
using int32.Utils.Web.WebServer.Processors.Contracts;

namespace int32.Utils.Web.WebServer
{
    public class WebServer 
    {
        private HttpListener _listener;
        private List<IRequestProcessor> _processors;
        private readonly AutoResetEvent _listenForNextRequest = new AutoResetEvent(false);

        public Bootstrapper Config { get; internal set; }

        #region events 

        public Action OnStarted { get; set; }
        public Action OnStopped { get; set; }
        public Action<IRequestProcessor, HttpListenerContext> OnRequestProcessed { get; set; }
        public Action<HttpListenerContext> OnRequestReceived { get; set; } 

        #endregion

        public WebServer Start<T>() where T : Bootstrapper
        {
            Config = Factory<T>.Create().ThrowIfNull("bootstrapper");
            Setup();
            return this;
        }

        public void Stop()
        {
            _listener.Stop();
            OnStopped.Execute();
        }

        private void Setup()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add(Config.Url + "/");
            _listener.AuthenticationSchemes = Config.Authentication;

            //set the default processors
            _processors = new List<IRequestProcessor>()
            {
                new ApiRequestProcessor(),
                new WebRequestProcessor()
            };

            //add additional ones from the config
            _processors.AddRange(Config.Processors);

            RunServer();
        }

        private void RunServer()
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
                            _listenForNextRequest.Set();
                        }
                    }, _listener);

                    _listenForNextRequest.WaitOne();
                }
            });

            OnStarted.Execute();
        }

        private void ProcessRequest(HttpListenerContext context)
        {
            try
            {
                OnRequestReceived.Execute(context);

                foreach (var processor in _processors)
                {
                    //when the first processor managed the request
                    //do not proceed with other processors
                    if (processor.IsNotNull() && processor.Process(context))
                    {
                        OnRequestProcessed.Execute(processor, context);
                        break;
                    }
                }
            }
            finally
            {
                //close the response
                context.Response.OutputStream.Close();
            }
        }
    }
}