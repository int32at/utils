namespace int32.Utils.Web.WebService.Contracts
{
    public interface IResponse<T>
    {
        StatusCode Status { get; set; }
        string Error { get; set; }
        T Result { get; set; }
    }
}