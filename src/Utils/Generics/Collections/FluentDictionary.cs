using System.Collections;
using System.Collections.Generic;

namespace int32.Utils.Generics.Collections
{
    public class FluentDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private readonly Dictionary<TKey, TValue> _data;

        public TValue this[TKey key]
        {
            get { return _data[key]; }
            set { _data[key] = value; }
        }

        public ICollection<TKey> Keys { get { return _data.Keys; } }
        public ICollection<TValue> Values { get { return _data.Values; } }
        public int Count { get { return _data.Count; } }

        public FluentDictionary()
        {
            _data = new Dictionary<TKey, TValue>();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public FluentDictionary<TKey, TValue> Add(TKey key, TValue value)
        {
            _data.Add(key, value);
            return this;
        }

        public FluentDictionary<TKey, TValue> Add(KeyValuePair<TKey, TValue> item)
        {
            return Add(item.Key, item.Value);
        }

        public FluentDictionary<TKey, TValue> Clear()
        {
            _data.Clear();
            return this;
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _data.ContainsKey(item.Key);
        }

        public bool ContainsKey(TKey key)
        {
            return _data.ContainsKey(key);
        }

        public FluentDictionary<TKey, TValue> Remove(TKey key)
        {
            _data.Remove(key);
            return this;
        }

        public FluentDictionary<TKey, TValue> Remove(KeyValuePair<TKey, TValue> item)
        {
            _data.Remove(item.Key);
            return this;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _data.TryGetValue(key, out value);
        }
    }
}