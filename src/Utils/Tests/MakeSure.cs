using System;

namespace int32.Utils.Tests
{
    public static class MakeSure
    {
        internal static Action<object, object> Equal { get; set; }
        internal static Action<object, object> NotEqual { get; set; }

        public static void Setup(Action<object, object> equal, Action<object, object> notEqual)
        {
            Equal = equal;
            NotEqual = notEqual;
        }

        public static That<T> That<T>(T item)
        {
            return new That<T>(item);
        } 
    }

    public class That<T>
    {
        public T Value { get; set; }

        public That(T item)
        {
            Value = item;
        }

        public void Is(T check)
        {
            MakeSure.Equal(check, Value);
        }

        public void IsNot(T check)
        {
            MakeSure.NotEqual(check, Value);
        }
    }
}
