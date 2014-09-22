using System.Collections.Generic;
using System.Net;
using int32.Utils.ServiceHandler.Services;
using int32.Utils.Web.Rest.Contracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace int32.Utils.Web.Rest
{
    public abstract class BaseRestService<T> : BaseService<T>
    {
        private WebClient _webClient;

        public IRestConfig Config { get; internal set; }

        protected BaseRestService(IRestConfig config)
        {
            Config = config;
        }

        public override void Initialize()
        {
            _webClient = new WebClient();
        }

        public override void Dispose()
        {
            if (_webClient != null)
                _webClient.Dispose();
        }

        protected TResult MakeGetRequest<TResult>(string url)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            _webClient.Credentials = Config.Credentials;
            _webClient.Headers.Add("Accept", "application/json; odata=verbose");
            var response = _webClient.DownloadString(url);

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter> { new StringEnumConverter { CamelCaseText = true } },
                NullValueHandling = NullValueHandling.Ignore,
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
            };

            return JsonConvert.DeserializeObject<TResult>(response, settings);
        }
    }
}
