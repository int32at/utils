using System;
using System.Collections.Generic;
using System.Linq;
using int32.Utils.Core.Extensions;
using int32.Utils.Core.Generic.Repository.Contracts;

namespace int32.Utils.Core.Generic.Data
{
    public class MemoryDatabase<T> : IRepository<T>
    {
        private readonly List<T> _data;

        public MemoryDatabase()
        {
            _data = new List<T>();
        }

        public int Count
        {
            get { return _data.Count; }
        }

        public T Add(T item)
        {
            _data.Add(item);
            return item;
        }

        public void Delete(T item)
        {
            _data.Remove(item);
        }

        public void Delete(Func<T, bool> predicate)
        {
            _data.Remove(predicate);
        }

        public T Get()
        {
            return _data.First();
        }

        public T Get(Func<T, bool> predicate)
        {
            return _data.FirstOrDefault(predicate);
        }

        public IEnumerable<T> GetAll()
        {
            return _data;
        }

        public IEnumerable<T> GetAll(Func<T, bool> predicate)
        {
            return _data.Where(predicate);
        }

        public T Update(T oldItem, T newItem)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}