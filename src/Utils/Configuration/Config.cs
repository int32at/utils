﻿using System;
using System.Collections.Generic;
using System.Linq;
using int32.Utils.Extensions;

namespace int32.Utils.Configuration
{
    public class Config
    {
        private readonly List<ConfigEntry> _entries;

        public object this[string key]
        {
            get { return GetConfigEntry(key).Value; }
        }

        public Config()
        {
            _entries = new List<ConfigEntry>();
        }

        public void Set(ConfigEntry entry)
        {
            _entries.Add(entry);
        }

        public void Set(string key, object value)
        {
            _entries.Add(new ConfigEntry(key, value));
        }

        public T Get<T>(string key)
        {
            return GetConfigEntry(key).Value.As<T>();
        }

        public void Remove(string key)
        {
            Remove(i => i.Key.Equals(key));
        }

        public void Remove(Func<ConfigEntry, bool> where)
        {
            for (var i = 0; i < _entries.Count; i++)
                if (where(_entries[i]))
                    _entries.RemoveAt(i);
        }

        private ConfigEntry GetConfigEntry(string key)
        {
            return _entries.SingleOrDefault(i => i.Key.Equals(key)).ThrowIfNull(key);
        }
    }
}
