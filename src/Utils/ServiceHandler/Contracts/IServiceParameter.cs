namespace int32.Utils.ServiceHandler.Contracts
{
    public interface IServiceParameter<T> : IServiceParameter
    {
        string Key { get; set; }
        T Value { get; set; }
    }

    public interface IServiceParameter { }
}
