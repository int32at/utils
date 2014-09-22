using System.Collections.Generic;
using System.Threading.Tasks;

namespace int32.Utils.ServiceHandler.Contracts
{
    public interface IServiceAdd<T>
    {
        T Add(T item);
        void Add(IEnumerable<T> items);
    }

    public interface IServiceAddAsync<T>
    {
        Task<T> AddAsync(T item);
        Task AddAsync(IEnumerable<T> items);
    }
}
