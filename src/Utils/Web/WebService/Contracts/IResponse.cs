using int32.Utils.Core.Exceptions;

namespace int32.Utils.Web.WebService.Contracts
{
    public interface IResponse<T>
    {
        StatusCode Status { get; set; }
        SerializableException Error { get; set; }
        T Result { get; set; }
    }
}