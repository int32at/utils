using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using int32.Utils.Core.Generic.Types;

namespace int32.Utils.Core.Extensions
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
            return o.ThrowIfNull("Safe");
        }

        public static bool IsNull<T>(this T o)
        {
            return o == null;
        }

        public static bool IsNotNull<T>(this T o)
        {
            return !o.IsNull();
        }

        public static T And<T>(this T o)
        {
            return o;
        }

        public static T And<T>(this T o, Func<T, T> action)
        {
            return action(o);
        }

        public static T And<T>(this T o, Action<T> action)
        {
            action(o);
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

        public static void Remove<T>(this ICollection<T> list, Func<T, bool> where)
        {
            list.ThrowIfNull("list");
            where.ThrowIfNull("where");

            var itemsToRemove = list.Where(where).ToList();
            foreach (var itemToRemove in itemsToRemove)
                list.Remove(itemToRemove);

        }

        public static ICollection<T> Update<T>(this ICollection<T> outer, Action<T> action)
        {
            foreach (var item in outer)
            {
                action(item);
            }

            return outer;
        }

        public static Switch<T> Switch<T>(this T o)
        {
            return new Switch<T>(o);
        }

        public static string MemberName<T, TResult>(this T o, Expression<Func<T, TResult>> expression) where T : class
        {
            return MemberName(expression);
        }

        public static string MemberName<T, TReturn>(Expression<Func<T, TReturn>> expression) where T : class
        {
            return ((MemberExpression)expression.Body).Member.Name;
        }
    }
}