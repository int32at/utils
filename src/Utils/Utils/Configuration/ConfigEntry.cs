﻿namespace int32.Utils.Configuration
{
    public class ConfigEntry
    {
        public string Key { get; set; }
        public object Value { get; set; }

        public ConfigEntry(string key, object o)
        {
            Key = key;
            Value = o;
        }
    }
}
