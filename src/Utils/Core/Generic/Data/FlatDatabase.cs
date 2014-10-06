using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using int32.Utils.Core.Extensions;
using int32.Utils.Core.Generic.Repository.Contracts;

namespace int32.Utils.Core.Generic.Data
{
    public class FlatDatabase<T> : IRepository<T>
    {
        protected FileInfo File;
        private List<T> _data;

        public FlatDatabase(string path) : this(new FileInfo(path)) { }

        public FlatDatabase(FileInfo file)
        {
            File = file.ThrowIfNull("dir").Ensure();
            _data = new List<T>();
        }

        public int Count { get { return _data.Count; } }

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

        public FlatDatabase<T> Load()
        {
            var data = File.ReadAlltext();
            _data = data.FromJSON<List<T>>();
            if (_data.IsNull()) _data = new List<T>();
            return this;
        }

        public void SaveChanges()
        {
            var json = _data.ToJSON();
            File.WriteAllText(json);
        }
    }
}
