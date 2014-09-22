using System.Collections.Generic;

namespace int32.Utils.ServiceHandler.Contracts
{
    public interface IServiceAdd<T>
    {
        T Add(T item);
        void Add(IEnumerable<T> items);
    }
}
