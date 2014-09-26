using System;
using System.Collections.Generic;

namespace int32.Utils.Generics.Repository.Contracts
{
    public interface IRepository { }

    public interface IRepository<TEntity, in TKey> : IRepository<TEntity> where TEntity : class, IModel
    {
        TEntity Get(TKey id);
    }

    public interface IRepository<TEntity> : IRepository where TEntity : class
    {
        int Count { get; }
        TEntity Add(TEntity item);
        void Delete(TEntity item);
        void Delete(Func<TEntity, bool> predicate);
        TEntity Get();
        TEntity Get(Func<TEntity, bool> predicate);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAll(Func<TEntity, bool> predicate);
        TEntity Update(TEntity oldItem, TEntity newItem);
    }
}
