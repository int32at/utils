using System.Collections.Generic;
using System.Threading.Tasks;

namespace int32.Utils45.ServiceHandler.Contracts
{
    public interface IServiceAddAsync<T>
    {
        Task<T> Add(T item);
        Task Add(IEnumerable<T> items);
    }
}
