using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace int32.Utils.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJSON(this object o)
        {
            o.ThrowIfNull("o");
            return JsonConvert.SerializeObject(o, Constants.JsonSerializerDefaultSettings);
        }

        public static string ToJSON(this IEnumerable<object> objects)
        {
            return JsonConvert.SerializeObject(objects, Constants.JsonSerializerDefaultSettings);
        }

        public static T As<T>(this object o)
        {
            return (T)(o.IsNull() ? o : o.ConvertTo(default(T)));
        }

        public static bool Is<T>(this object o)
        {
            o.ThrowIfNull("o");
            return o is T;
        }

        private static T ConvertTo<T>(this object value, T defaultValue)
        {
            if (value != null)
            {
                var targetType = typeof(T);

                if (value.GetType() == targetType) return (T)value;

                var converter = TypeDescriptor.GetConverter(value);

                if (converter.CanConvertTo(targetType))
                    return (T)converter.ConvertTo(value, targetType);

                converter = TypeDescriptor.GetConverter(targetType);

                if (converter.CanConvertFrom(value.GetType()))
                    return (T)converter.ConvertFrom(value);
            }
            return defaultValue;
        }
    }
}
