﻿using System;
using System.Reflection;

namespace int32.Utils.Core.Extensions
{
    public static class ReflectionExtensions
    {
        public static void Set<T>(this object o, string name, T value)
        {
            o.ThrowIfNull("o");
            value.ThrowIfNull("value");

            var type = o.GetType();

            GetProperty(type, name)
                .IfNull(() => GetField(type, name).IfNotNull(field => field.SetValue(o, value)))
                .IfNotNull(prop => prop.SetValue(o, value, null));
        }

        public static T Get<T>(this object o, string name)
        {
            o.ThrowIfNull("o");

            var type = o.GetType();
            T returnValue = default(T);

            GetProperty(type, name)
                .IfNull(() => GetField(type, name).IfNotNull(field => returnValue = field.GetValue(o).As<T>()))
                .IfNotNull(prop => returnValue = prop.GetValue(o, null).As<T>());

            return returnValue;
        }

        public static object Get(this object o, string name)
        {
            o.ThrowIfNull("o");

            var type = o.GetType();
            object returnValue = null;

            GetProperty(type, name)
                .IfNull(() => GetField(type, name).IfNotNull(field => returnValue = field.GetValue(o)))
                .IfNotNull(prop => returnValue = prop.GetValue(o, null));

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
