using System;
using System.Collections;
using System.Collections.Generic;
using int32.Utils.Extensions;

namespace int32.Utils.Generics.Collections
{
    public class FluentList<T> : List<T>
    {
        public new FluentList<T> Add(T obj)
        {
            base.Add(obj);
            return this;
        }

        public new FluentList<T> AddRange(IEnumerable<T> collection)
        {
            base.AddRange(collection);
            return this;
        }

        public new FluentList<T> Clear()
        {
            base.Clear();
            return this;
        }

        public new FluentList<T> ForEach(Action<T> action)
        {
            base.ForEach(action);
            return this;
        }

        public new FluentList<T> Insert(int index, T item)
        {
            base.Insert(index, item);
            return this;
        }

        public new FluentList<T> InsertRange(int index, IEnumerable<T> collection)
        {
            base.InsertRange(index, collection);
            return this;
        }

        public new FluentList<T> Remove(T item)
        {
            base.Remove(item);
            return this;
        }

        public new FluentList<T> RemoveAt(int index)
        {
            base.RemoveAt(index);
            return this;
        }

        public new FluentList<T> RemoveRange(int index, int count)
        {
            base.RemoveRange(index, count);
            return this;
        }

        public new FluentList<T> Reverse()
        {
            base.Reverse();
            return this;
        }

        public new FluentList<T> Sort()
        {
            base.Sort();
            return this;
        }
    }
}