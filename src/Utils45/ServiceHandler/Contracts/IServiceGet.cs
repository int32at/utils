using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using int32.Utils.ServiceHandler.Contracts;

namespace int32.Utils45.ServiceHandler.Contracts
{
    public interface IServiceGetAsync<T>
    {
        Task<T> Get();
        Task<T> Get(int id);
    }

    public interface IServiceGetAllAsync<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAll(Predicate<T> predicate);
    }

    public interface IServiceGetParamAsync<T, in TParam> where TParam : IServiceParameter
    {
        Task<T> Get(TParam param);
        Task<IEnumerable<T>> GetAll(TParam param);
    }
}
