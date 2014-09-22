using System.Net;

namespace int32.Utils.Web.Rest.Contracts
{
    public interface IRestConfig
    {
        string BaseUrl { get; set; }
        ICredentials Credentials { get; set; }
    }
}
