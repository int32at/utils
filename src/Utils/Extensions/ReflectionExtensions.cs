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

            GetProperty(type, name)
                .IfNull
                    (prop => GetField(type, name).IfNotNull(field => field.SetValue(o, value)),
                prop => prop.SetValue(o, value, null));
        }

        public static T Get<T>(this object o, string name)
        {
            o.ThrowIfNull("o");

            var type = o.GetType();
            T returnValue = default(T);

            GetProperty(type, name).IfNotNull(prop =>
            {
                returnValue = prop.GetValue(o, null).As<T>();
            }, prop => GetField(type, name).IfNotNull(field => { returnValue = field.GetValue(o).As<T>(); }));

            return returnValue;
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
