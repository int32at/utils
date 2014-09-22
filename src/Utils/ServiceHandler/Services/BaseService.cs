using System;
using System.Collections.Generic;
using int32.Utils.ServiceHandler.Contracts;

namespace int32.Utils.ServiceHandler.Services
{
    public abstract class BaseService<T, TParam> : IService, IServiceGet<T>, IServiceGetAll<T>, IServiceGetParam<T, TParam>, IServiceAdd<T>, IServiceDelete<T>, IServiceDeleteParam<TParam>, IServiceGetCount where TParam : IServiceParameter
    {
        public abstract void Initialize();
        public abstract T Add(T item);
        public abstract void Add(IEnumerable<T> items);
        public abstract void Delete(int id);
        public abstract void Delete(T item);
        public abstract void Delete(TParam param);
        public abstract void Delete(Predicate<T> predicate);
        public abstract T Get();
        public abstract T Get(int id);
        public abstract T Get(TParam param);
        public abstract IEnumerable<T> GetAll();
        public abstract IEnumerable<T> GetAll(TParam param);
        public abstract IEnumerable<T> GetAll(Predicate<T> predicate);
        public abstract int GetCount();
        public abstract void Dispose();
    }

    public abstract class BaseService<T> : IService, IServiceGet<T>, IServiceGetAll<T>, IServiceAdd<T>, IServiceDelete<T>, IServiceGetCount
    {
        public abstract void Initialize();
        public abstract T Add(T item);
        public abstract void Add(IEnumerable<T> items);
        public abstract void Delete(int id);
        public abstract void Delete(T item);
        public abstract void Delete(Predicate<T> predicate);
        public abstract T Get();
        public abstract T Get(int id);
        public abstract IEnumerable<T> GetAll();
        public abstract IEnumerable<T> GetAll(Predicate<T> predicate);
        public abstract int GetCount();
        public abstract void Dispose();

    }
}
