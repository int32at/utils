using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using int32.Utils.ServiceHandler.Contracts;
using int32.Utils45.ServiceHandler.Contracts;

namespace int32.Utils45.ServiceHandler.Services
{
    public abstract class BaseService<T, TParam> : IService, IServiceGetAsync<T>, IServiceGetAllAsync<T>, IServiceGetParamAsync<T, TParam>, IServiceAddAsync<T>, IServiceDeleteAsync<T>, IServiceDeleteParam<TParam>, IServiceGetCountAsync where TParam : IServiceParameter
    {
        public abstract void Initialize();
        public abstract Task<T> Add(T item);
        public abstract Task Add(IEnumerable<T> items);
        public abstract Task Delete(int id);
        public abstract Task Delete(T item);
        public abstract Task Delete(TParam param);
        public abstract Task Delete(Predicate<T> predicate);
        public abstract Task<T> Get();
        public abstract Task<T> Get(int id);
        public abstract Task<T> Get(TParam param);
        public abstract Task<IEnumerable<T>> GetAll();
        public abstract Task<IEnumerable<T>> GetAll(TParam param);
        public abstract Task<IEnumerable<T>> GetAll(Predicate<T> predicate);
        public abstract Task<int> GetCount();
        public abstract void Dispose();
    }
}
