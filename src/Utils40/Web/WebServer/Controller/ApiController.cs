using System;
using Newtonsoft.Json;

namespace int32.Utils.Web.WebServer.Controller
{
    public abstract class ApiController : BaseController
    {
        protected ApiController(string path) : this()
        {
            Path = Path + path;
        }

        private ApiController()
        {
            Path = "api";
        }
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
            Data = new object[] {};
            Error = new ApiResponseError(ex);
        }
    }

    public class ApiResponseError
    {
        public int Code { get; set; }
        public string Message { get; set; }

        public ApiResponseError(Exception ex)
        {
            Code = 500;
            Message = ex.Message;
        }
    }
}
