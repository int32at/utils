namespace int32.Utils.Generics
{
    public class Property<T>
    {
        public T Value { get; set; }

        public static implicit operator T(Property<T> value)
        {
            return value.Value;
        }

        public static implicit operator Property<T>(T value)
        {
            return new Property<T> { Value = value };
        }
    }
}
