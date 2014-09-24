﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace int32.Utils.Extensions
{
    public static class GenericExtensions
    {
        public static T ThrowIfNull<T>(this T o, string name)
        {
            o.IfNull(() => { throw new ArgumentNullException(name); });
            return o;
        }

        public static T Safe<T>(this T o)
        {
            o.ThrowIfNull("Safe");
            return o;
        }

        public static bool IsNull<T>(this T o)
        {
            return o == null;
        }

        public static T Ensure<T>(this T o)
        {
            return o.ThrowIfNull("o");
        }

        public static T And<T>(this T o)
        {
            return o;
        }

        public static T IfNotNull<T>(this T o, Action action)
        {
            if (!o.IsNull())
                action();

            return o;
        }

        public static T IfNotNull<T>(this T o, Action<T> action)
        {
            if (!o.IsNull())
                action(o);

            return o;
        }

        public static T IfNotNull<T>(this T o, Func<T> action)
        {
            return !o.IsNull() ? action() : o;
        }

        public static T IfNotNull<T>(this T o, Func<T, T> action)
        {
            return !o.IsNull() ? action(o) : o;
        }

        public static T IfNull<T>(this T o, Action action)
        {
            if (o.IsNull())
                action();

            return o;
        }

        public static T IfNull<T>(this T o, Func<T> action)
        {
            return o.IsNull() ? action() : o;
        }

        public static T IfNull<T>(this T o, Func<T, T> action)
        {
            return o.IsNull() ? action(o) : o;
        }

        public static T IfNull<T>(this T o, Action<T> action)
        {
            if (o.IsNull())
                action(o);

            return o;
        }

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

        public static void Remove<T>(this ICollection<T> list, Predicate<T> where)
        {
            list.ThrowIfNull("list");
            where.ThrowIfNull("where");

            var itemsToRemove = list.Where(i => where(i)).ToList();
            foreach (var itemToRemove in itemsToRemove)
                list.Remove(itemToRemove);

        }

        public static void Update<T>(this IEnumerable<T> outer, Action<T> updator)
        {
            foreach (var item in outer)
            {
                updator(item);
            }
        }
    }
}
