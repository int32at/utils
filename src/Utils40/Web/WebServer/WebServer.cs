using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using int32.Utils.Core.Extensions;
using int32.Utils.Core.Generic.Factory;
using int32.Utils.Web.WebServer.Processors;
using int32.Utils.Web.WebServer.Processors.Contracts;

namespace int32.Utils.Web.WebServer
{
    public static class WebServer 
    {
        private static HttpListener _listener;
        private static List<IRequestProcessor> _processors;
        private static readonly AutoResetEvent ListenForNextRequest = new AutoResetEvent(false);

        public static Bootstrapper Config { get; internal set; }

        #region events 

        public static Action OnStarted { get; set; }
        public static Action OnStopped { get; set; }
        public static Action<IRequestProcessor, HttpListenerContext> OnRequestProcessed { get; set; }
        public static Action<HttpListenerContext> OnRequestReceived { get; set; } 

        #endregion

        public static void Start<T>() where T : Bootstrapper
        {
            Config = Factory<T>.Create().ThrowIfNull("bootstrapper");
            Setup();
        }

        public static void Stop()
        {
            _listener.Stop();
            OnStopped.Execute();
        }

        private static void Setup()
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

        private static void RunServer()
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

            OnStarted.Execute();
        }

        private static void ProcessRequest(HttpListenerContext context)
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
                        using (var memory = new MemoryStream())
                        {
                            context.Response.OutputStream.CopyTo(memory);
                        }
                       
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