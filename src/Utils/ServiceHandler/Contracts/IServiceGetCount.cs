using System.Threading.Tasks;

namespace int32.Utils.ServiceHandler.Contracts
{
    public interface IServiceGetCount
    {
        int GetCount();
    }

    public interface IServiceGetCountAsync
    {
        Task<int> GetCountAsync();
    }
}
