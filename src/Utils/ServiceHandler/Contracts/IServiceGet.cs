using System;
using System.Collections.Generic;

namespace int32.Utils.ServiceHandler.Contracts
{
    public interface IServiceGet<T>
    {
        T Get();
        T Get(int id);
    }

    public interface IServiceGetAll<T>
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(Predicate<T> predicate);
    }

    public interface IServiceGetParam<T, in TParam> where TParam : IServiceParameter
    {
        T Get(TParam param);
        IEnumerable<T> GetAll(TParam param);
    }
}
