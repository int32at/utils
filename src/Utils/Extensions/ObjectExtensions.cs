using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace int32.Utils.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJSON(this object o)
        {
            o.ThrowIfNull("o");
            return JsonConvert.SerializeObject(o);
        }

        public static string ToJSON(this IEnumerable<object> objects)
        {
            return JsonConvert.SerializeObject(objects);
        }

        public static T As<T>(this object o)
        {
            o.ThrowIfNull("o");
            return o.ConvertTo(default(T));
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
