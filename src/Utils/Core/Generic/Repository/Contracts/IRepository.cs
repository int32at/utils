﻿using System;
using System.Collections.Generic;

namespace int32.Utils.Core.Generic.Repository.Contracts
{
    public interface IRepository { }

    public interface IRepository<TEntity, in TKey> : IRepository<TEntity> where TEntity : class
    {
        TEntity Get(TKey id);
    }

    public interface IRepository<TEntity> : IRepository
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
        void SaveChanges();
    }
}
