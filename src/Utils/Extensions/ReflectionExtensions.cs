using System;
using System.Reflection;

namespace int32.Utils.Extensions
{
    public static class ReflectionExtensions
    {
        public static void Set<T>(this object o, string name, T value)
        {
            o.ThrowIfNull("o");
            value.ThrowIfNull("value");

            var type = o.GetType();

            var prop = GetProperty(type, name);

            if (prop != null)
                prop.SetValue(o, value, null);
            else
            {
                var field = GetField(type, name);

                if (field != null)
                    field.SetValue(o, value);
            }
        }

        public static T Get<T>(this object o, string name)
        {
            o.ThrowIfNull("o");

            var type = o.GetType();

            var prop = GetProperty(type, name);

            if (prop != null)
                return prop.GetValue(o, null).As<T>();

            var field = GetField(type, name);

            return field != null ? field.GetValue(o).As<T>() : default(T);
        }

        private static PropertyInfo GetProperty(Type type, string name)
        {
            return type.GetProperty(name) ??
                                type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty);
        }

        private static FieldInfo GetField(Type type, string name)
        {
            return type.GetField(name) ??
                                type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);
        }
    }
}
