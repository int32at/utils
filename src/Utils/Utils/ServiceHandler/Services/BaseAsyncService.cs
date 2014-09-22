using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using int32.Utils.ServiceHandler.Contracts;

namespace int32.Utils.ServiceHandler.Services
{
    public abstract class BaseAsyncService<T, TParam> : IService, IServiceGetAsync<T>, IServiceGetAllAsync<T>, IServiceGetParamAsync<T, TParam>, IServiceAddAsync<T>, IServiceDeleteAsync<T>, IServiceDeleteParamAsync<TParam>, IServiceGetCountAsync where TParam : IServiceParameter
    {
        public abstract void Initialize();
        public abstract Task<T> AddAsync(T item);
        public abstract Task AddAsync(IEnumerable<T> items);
        public abstract Task DeleteAsync(int id);
        public abstract Task DeleteAsync(T item);
        public abstract Task DeleteAsync(TParam param);
        public abstract Task DeleteAsync(Predicate<T> predicate);
        public abstract Task<T> GetAsync();
        public abstract Task<T> GetAsync(int id);
        public abstract Task<T> GetAsync(TParam param);
        public abstract Task<IEnumerable<T>> GetAllAsync();
        public abstract Task<IEnumerable<T>> GetAllAsync(TParam param);
        public abstract Task<IEnumerable<T>> GetAllAsync(Predicate<T> predicate);
        public abstract Task<int> GetCountAsync();
        public abstract void Dispose();
    }
}
