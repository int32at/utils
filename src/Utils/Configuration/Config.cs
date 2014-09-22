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
            var entry = _entries.SingleOrDefault(i => i.Key.Equals(key));
            entry.ThrowIfNull(key);
            _entries.Remove(entry);
        }
    }
}
