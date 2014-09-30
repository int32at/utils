using int32.Utils.Core.Exceptions;
using int32.Utils.Web.WebService.Contracts;

namespace int32.Utils.Web.WebService
{
    public enum StatusCode
    {
        Ok,
        Error
    }

    public class Response<T> : IResponse<T>
    {
        public StatusCode Status { get; set; }
        public SerializableException Error { get; set; }
        public T Result { get; set; }
    }
}