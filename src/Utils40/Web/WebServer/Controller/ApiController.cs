using System;
using Newtonsoft.Json;

namespace int32.Utils.Web.WebServer.Controller
{
    public class ApiController : BaseController
    {
        protected ApiController(string path) : base(path) { }
    }

    public class ApiResponse
    {
        public object Data { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ApiResponseError Error { get; set; }

        public ApiResponse(object data)
        {
            Data = data;
        }

        public ApiResponse(Exception ex)
        {
            Data = new object[] { };
            Error = new ApiResponseError(ex, 500);
        }

        public ApiResponse(Exception ex, int code)
        {
            Data = new object[] { };
            Error = new ApiResponseError(ex, code);
        }
    }

    public class ApiResponseError
    {
        public int Code { get; set; }
        public string Message { get; set; }

        public ApiResponseError(Exception ex, int code)
        {
            Code = code;
            Message = ex.Message;
        }
    }
}
