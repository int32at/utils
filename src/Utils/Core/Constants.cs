using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace int32.Utils.Core
{
    public static class Constants
    {
        private static JsonSerializerSettings _jsonSerializerSettings;

        private static JsonSerializerSettings _jsonSerializerAllSettings;

        public static JsonSerializerSettings JsonSerializerDefaultSettings
        {
            get
            {
                if (_jsonSerializerSettings == null)
                {
                    _jsonSerializerSettings = new JsonSerializerSettings();
                    _jsonSerializerSettings.Converters.Add(new StringEnumConverter());
                }

                return _jsonSerializerSettings;
            }
        }

        public static JsonSerializerSettings JsonSerializerAllSettings
        {
            get
            {
                if (_jsonSerializerAllSettings == null)
                {
                    _jsonSerializerAllSettings = new JsonSerializerSettings();
                    _jsonSerializerAllSettings.ContractResolver = new AllPropertiesResolver();
                }

                return _jsonSerializerAllSettings;
            }
        }
    }

    public class AllPropertiesResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                            .Select(p => base.CreateProperty(p, memberSerialization))
                        .Union(type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                   .Select(f => base.CreateProperty(f, memberSerialization)))
                        .ToList();
            props.ForEach(p => { p.Writable = true; p.Readable = true; });
            return props;
        }
    }
}
