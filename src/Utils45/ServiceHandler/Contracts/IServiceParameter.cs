namespace int32.Utils45.ServiceHandler.Contracts
{
    public interface IServiceParameter<T> : IServiceParameter
    {
        string Key { get; set; }
        T Value { get; set; }
    }

    public interface IServiceParameter { }
}
