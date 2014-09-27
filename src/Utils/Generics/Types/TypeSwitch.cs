using System;

namespace int32.Utils.Generics.Types
{
    public class Switch<T>
    {
        private readonly T _value;
        private bool _handled;

        public Switch(T value)
        {
            _value = value;
        }

        public Switch<T> Case<TTarget>(Action action) where TTarget : T
        {
            return Case<TTarget>(obj => action());
        }

        public Switch<T> Case<TTarget>(Action<T> action) where TTarget : T
        {
            if (_handled) return this;
            var sourceType = _value.GetType();
            var targetType = typeof(TTarget);

            if (sourceType != targetType) return this;
            action(_value);
            _handled = true;
            return this;
        }

        public void Default(Action action)
        {
            Default(obj => action());
        }

        public void Default(Action<T> action)
        {
            if (!_handled)
                action(_value);
        }
    }
}