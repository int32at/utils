using System;

namespace int32.Utils.ServiceHandler.Contracts
{
    public interface IService : IDisposable
    {
        void Initialize();
    }
}
