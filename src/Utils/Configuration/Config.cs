using System;
using System.Collections.Generic;
using System.Linq;
using int32.Utils.Extensions;

namespace int32.Utils.Configuration
{
    public class Config
    {
        private readonly List<ConfigEntry> _entries;

        public Config()
        {
            _entries = new List<ConfigEntry>();
        }

        public void Set(ConfigEntry entry)
        {
            _entries.Add(entry);
        }

        public T Get<T>(string key)
        {
            var entry = _entries.SingleOrDefault(i => i.Key.Equals(key));
            entry.ThrowIfNull(key);

            // ReSharper disable once PossibleNullReferenceException
            return entry.Value.As<T>();
        }

        public void Remove(string key)
        {
            Remove(i => i.Key.Equals(key));
        }

        public void Remove(Predicate<ConfigEntry> where)
        {
            for (var i = 0; i < _entries.Count; i++)
                if (where(_entries[i]))
                    _entries.RemoveAt(i);
        }
    }
}
