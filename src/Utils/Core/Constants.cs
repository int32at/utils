using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace int32.Utils.Core
{
    public static class Constants
    {
        private static JsonSerializerSettings _jsonSerializerSettings;

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
    }
}
