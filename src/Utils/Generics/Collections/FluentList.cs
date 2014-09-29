using System;
using System.Collections;
using System.Collections.Generic;
using int32.Utils.Extensions;

namespace int32.Utils.Generics.Collections
{
    public class FluentList<T> : IEnumerable<T>
    {
        private readonly List<T> _data;

        public int Count { get { return _data.Count; } }

        public T this[int index]
        {
            get { return _data[index]; }
            set { _data[index] = value; }
        }

        public FluentList()
        {
            _data = new List<T>();
        }

        public FluentList<T> Add(T item)
        {
            _data.Add(item);
            return this;
        }

        public FluentList<T> Clear()
        {
            _data.Clear();
            return this;
        }

        public bool Contains(T item)
        {
            return _data.Contains(item);
        }

        public FluentList<T> CopyTo(T[] array, int arrayIndex)
        {
            _data.CopyTo(array, arrayIndex);
            return this;
        }

        public int IndexOf(T item)
        {
            return _data.IndexOf(item);
        }

        public FluentList<T> Insert(int index, T item)
        {
            _data.Insert(index, item);
            return this;
        }

        public FluentList<T> Remove(T item)
        {
            _data.Remove(item);
            return this;
        }

        public FluentList<T> Remove(Func<T, bool> predicate)
        {
            _data.Remove(predicate);
            return this;
        }

        public FluentList<T> RemoveAt(int index)
        {
            _data.RemoveAt(index);
            return this;
        }

        public FluentList<T> Update(Action<T> action)
        {
            _data.Update(action);
            return this;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}