using System;
using int32.Utils.Core.Extensions;

namespace int32.Utils.Tests
{
    public static class MakeSure
    {
        public static That<T> That<T>(T item)
        {
            return new That<T>(item);
        }

        public static ThatAction That(Action action)
        {
            return new ThatAction(action);
        }

        internal static bool Equal(object a, object b)
        {
            return Equals(a, b);
        }

        internal static bool Greater(IComparable a, IComparable b)
        {
            return a.CompareTo(b) > 0;
        }

        internal static bool Less(IComparable a, IComparable b)
        {
            return b.CompareTo(a) > 0;
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

            if (!MakeSure.Equal(expected, actual))
                throw new AssertFailedException(expected, actual, "Throws");
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

            if (!MakeSure.Equal(expected, actual))
                throw new AssertFailedException(expected, actual, "DoesNotThrow");
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
            if (!MakeSure.Equal(check, Value))
                throw new AssertFailedException(check, Value, "Is");
        }

        public void IsNot(T check)
        {
            if (MakeSure.Equal(check, Value))
                throw new AssertFailedException(check, Value, "IsNot");
        }

        public void IsGreaterThan(T check)
        {
            if(!MakeSure.Greater((IComparable)Value, (IComparable)check))
                throw new AssertFailedException(check, Value, "IsGreaterThan");
        }

        public void IsLessThan(T check)
        {
            if(!MakeSure.Less((IComparable)Value, (IComparable)check))
                throw new AssertFailedException(check, Value, "IsLessThan");
        }
    }

    public class AssertFailedException : Exception
    {
        private readonly object _a;
        private readonly object _b;
        private readonly string _operation;

        public AssertFailedException(object a, object b, string operation)
        {
            _a = a;
            _b = b;
            _operation = operation;
        }

        private string GetMessage()
        {
            return "'{0}' {1} '{2}'".With(GetName(_b), _operation, GetName(_a));
        }

        private string GetName(object o)
        {
            return o.IsNull() ? "null" : o.ToString();
        }

        public override string Message
        {
            get { return GetMessage(); }
        }
    }
}
