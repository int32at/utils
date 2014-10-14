using System;

namespace int32.Utils.Tests
{
    public static class MakeSure
    {
        internal static Action<object, object> Equal { get; set; }
        internal static Action<object, object> NotEqual { get; set; }
        internal static Action<object, object> Less { get; set; }
        internal static Action<object, object> Greater { get; set; }

        public static void Setup(Action<object, object> equal, Action<object, object> notEqual,
            Action<object, object> less, Action<object, object> greater)
        {
            Equal = equal;
            NotEqual = notEqual;
            Less = less;
            Greater = greater;
        }

        public static That<T> That<T>(T item)
        {
            return new That<T>(item);
        }

        public static ThatAction That(Action action)
        {
            return new ThatAction(action);
        }
    }

    public class ThatAction
    {
        public Action Value { get; set; }

        public ThatAction(Action action)
        {
            Value = action;
        }

        public void Throws<T>()
        {
            const bool expected = true;

            var actual = false;
            try
            {
                Value();
            }
            catch (Exception ex)
            {
                actual = ex.GetType() == typeof(T);
            }

            MakeSure.Equal(expected, actual);
        }

        public void DoesNotThrow()
        {
            const bool expected = true;
            var actual = false;

            try
            {
                Value();
                actual = true;
            }
            catch { }

            MakeSure.Equal(expected, actual);
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

        public void IsGreaterThan(T check)
        {
            MakeSure.Greater(Value, check);
        }

        public void IsLessThan(T check)
        {
            MakeSure.Less(Value, check);
        }
    }
}
