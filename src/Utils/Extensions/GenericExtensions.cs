using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace int32.Utils.Extensions
{
    public static class GenericExtensions
    {
        public static bool In<T>(this T source, params T[] list)
        {
            source.ThrowIfNull("source");
            return list.Contains(source);
        }

        public static bool In<T>(this T[] source, params T[] list)
        {
            source.ThrowIfNull("source");
            return source.ToList().All(i => i.In(list));
        }

        public static bool Between<T>(this T actual, T lower, T upper) where T : IComparable<T>
        {
            return actual.CompareTo(lower) >= 0 && actual.CompareTo(upper) < 0;
        }

        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            list.ThrowIfNull("list");
            action.ThrowIfNull("action");

            foreach (var item in list) action(item);
        }
    }
}
