using System;
using System.Threading.Tasks;

namespace int32.Utils.ServiceHandler.Contracts
{
    public interface IServiceDelete<T>
    {
        void Delete(int id);
        void Delete(T item);
        void Delete(Predicate<T> predicate);
    }

    public interface IServiceDeleteAsync<T>
    {
        Task DeleteAsync(int id);
        Task DeleteAsync(T item);
        Task DeleteAsync(Predicate<T> predicate);
    }

    public interface IServiceDeleteParam<T> where T : IServiceParameter
    {
        void Delete(T param);
    }

    public interface IServiceDeleteParamAsync<T> where T : IServiceParameter
    {
        Task DeleteAsync(T param);
    }
}
