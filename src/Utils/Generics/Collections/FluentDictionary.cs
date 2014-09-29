using System.Collections.Generic;

namespace int32.Utils.Generics.Collections
{
    public class FluentDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public new FluentDictionary<TKey, TValue> Add(TKey key, TValue value)
        {
            base.Add(key, value);
            return this;
        }

        public new FluentDictionary<TKey, TValue> Clear()
        {
            base.Clear();
            return this;
        }

        public new FluentDictionary<TKey, TValue> Remove(TKey key)
        {
            base.Remove(key);
            return this;
        }
    }
}