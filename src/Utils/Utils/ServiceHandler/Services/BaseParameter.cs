using int32.Utils.ServiceHandler.Contracts;

namespace int32.Utils.ServiceHandler.Services
{
    public class BaseParameter<T> : IServiceParameter<T>
    {
        public string Key { get; set; }
        public T Value { get; set; }
    }
}
