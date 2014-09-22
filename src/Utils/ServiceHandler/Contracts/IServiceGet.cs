using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace int32.Utils.ServiceHandler.Contracts
{
    public interface IServiceGet<T>
    {
        T Get();
        T Get(int id);
    }

    public interface IServiceGetAsync<T>
    {
        Task<T> GetAsync();
        Task<T> GetAsync(int id);
    }

    public interface IServiceGetAll<T>
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(Predicate<T> predicate);
    }
    public interface IServiceGetAllAsync<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(Predicate<T> predicate);
    }

    public interface IServiceGetParam<out T, in TParam> where TParam : IServiceParameter
    {
        T Get(TParam param);
        IEnumerable<T> GetAll(TParam param);
    }
    public interface IServiceGetParamAsync<T, in TParam> where TParam : IServiceParameter
    {
        Task<T> GetAsync(TParam param);
        Task<IEnumerable<T>> GetAllAsync(TParam param);
    }
}
