using System;

namespace int32.Utils45.ServiceHandler.Contracts
{
    public interface IService : IDisposable
    {
        void Initialize(IInitializeParameter param);
    }
}
