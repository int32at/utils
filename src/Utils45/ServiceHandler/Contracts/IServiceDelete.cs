using System;
using System.Threading.Tasks;
using int32.Utils.ServiceHandler.Contracts;

namespace int32.Utils45.ServiceHandler.Contracts
{
    public interface IServiceDeleteAsync<T>
    {
        Task Delete(int id);
        Task Delete(T item);
        Task Delete(Predicate<T> predicate);
    }

    public interface IServiceDeleteParam<T> where T : IServiceParameter
    {
        Task Delete(T param);
    }
}
