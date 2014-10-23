﻿using System;

namespace int32.Utils.Web.WebServer.Controller
{
    public abstract class WebController : BaseController
    {
        protected WebController(string viewFile)
        {
            Path = viewFile;
        }
    }

    public class WebResponse
    {
        public string Data { get; set; }
        public Exception Error { get; set; }

        public WebResponse(string data)
        {
            Data = data;
        }

        public WebResponse(Exception ex)
        {
            Error = ex;
        }

        public override string ToString()
        {
            return Error == null ? Data : Error.Message;
        }
    }
}
