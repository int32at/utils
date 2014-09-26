using System;
using System.Collections.Generic;

namespace int32.Utils.ServiceHandler.Contracts
{
    public interface IServiceUpdate<T>
    {
        T Update(T item, Func<T, T> predicate);
    }
}
