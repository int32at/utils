using System;
using System.Collections.Generic;
using int32.Utils.Core.Extensions;

namespace int32.Utils.Core.Generic.Collections
{
    public class EventList<T> : List<T>
    {
        public Action<T> ItemAdded;
        public Action<T> ItemRemoved;

        public new void Add(T item)
        {
            base.Add(item);
            ItemAdded.IfNotNull(i => i(item));
        }

        public new void AddRange(IEnumerable<T> collection)
        {
            collection.ForEach(Add);
        }

        public new void Remove(T item)
        {
            base.Remove(item);
            ItemRemoved.IfNotNull(i => i(item));
        }

        public new void RemoveAt(int index)
        {
            base.RemoveAt(index);
            ItemRemoved.IfNotNull(i => i(base[index]));
        }
    }
}