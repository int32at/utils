using System;

namespace int32.Utils.ServiceHandler.Contracts
{
    public interface IServiceHandler : IDisposable
    {
        void Register(IService service);
        T Service<T>();
        void Initialize();
    }
}
