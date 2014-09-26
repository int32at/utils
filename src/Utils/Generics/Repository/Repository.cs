﻿using System;
using System.Collections.Generic;
using int32.Utils.Generics.Repository.Contracts;

namespace int32.Utils.Generics.Repository
{
    public abstract class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class, IModel
    {
        public abstract int Count { get; }
        public abstract TEntity Add(TEntity item);
        public abstract void Delete(TEntity item);
        public abstract void Delete(Func<TEntity, bool> predicate);
        public abstract TEntity Get();
        public abstract TEntity Get(TKey id);
        public abstract TEntity Get(Func<TEntity, bool> predicate);
        public abstract IEnumerable<TEntity> GetAll();
        public abstract IEnumerable<TEntity> GetAll(Func<TEntity, bool> predicate);
        public abstract TEntity Update(TEntity oldItem, TEntity newItem);
    }

    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IModel
    {
        public abstract int Count { get; }
        public abstract TEntity Add(TEntity item);
        public abstract void Delete(TEntity item);
        public abstract void Delete(Func<TEntity, bool> predicate);
        public abstract TEntity Get();
        public abstract TEntity Get(Func<TEntity, bool> predicate);
        public abstract IEnumerable<TEntity> GetAll();
        public abstract IEnumerable<TEntity> GetAll(Func<TEntity, bool> predicate);
        public abstract TEntity Update(TEntity oldItem, TEntity newItem);
    }
}
