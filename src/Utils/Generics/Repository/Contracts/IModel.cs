using System;
using System.Collections.Generic;

namespace int32.Utils.Generics.Repository.Contracts
{
    public interface IModel
    {
        string Return();
    }

    public static class ModelExtensions
    {
        public static string Return<T>(this IEnumerable<T> models, Func<IEnumerable<T>, string> action) where T : IModel
        {
            return action(models);
        }
    }
}
