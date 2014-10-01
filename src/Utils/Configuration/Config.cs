using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using int32.Utils.Core.Extensions;

namespace int32.Utils.Configuration
{
    public class Config
    {
        private List<ConfigEntry> _entries;

        public object this[string key]
        {
            get { return GetConfigEntry(key).Value; }
            set { Set(key, value); }
        }

        public int Count { get { return _entries.Count; } }

        public Config()
        {
            _entries = new List<ConfigEntry>();
        }

        public void Set(ConfigEntry entry)
        {
            entry.ThrowIfNull("entry");

            if (_entries.Contains(entry))
                throw new ArgumentException(string.Format("Entry with key {0} already exists", entry.Key));

            _entries.Add(entry);
        }

        public void Set(string key, object value)
        {
            Set(new ConfigEntry(key, value));
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

        /// <summary>
        /// Loads the app.config file automatically and parses the appSettings 
        /// </summary>
        /// <returns></returns>
        public static Config Create()
        {
            return Load(ConfigurationManager.AppSettings);
        }

        public Config Load()
        {
            var cfg = Create();
            _entries = cfg._entries;
            return this;
        }

        private static Config Load(NameValueCollection values)
        {
            var temp = new Config();

            values.IfNotNull(config =>
            {
                foreach (var key in config.AllKeys)
                    temp.Set(key, config[key]);
            });

            return temp;
        }

        private ConfigEntry GetConfigEntry(string key)
        {
            return _entries.SingleOrDefault(i => i.Key.Equals(key)).ThrowIfNull(key);
        }
    }
}
