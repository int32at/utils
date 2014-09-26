using System.Collections.Generic;
using int32.Utils.Extensions;
using int32.Utils.Generics.Repository.Contracts;

namespace int32.Utils.Generics.Repository
{
    public class RepositoryHandler
    {
        private static readonly List<IRepository> Repositories = new List<IRepository>();

        public static void Register(IRepository repository)
        {
            if (!Repositories.Contains(repository))
                Repositories.Add(repository);
        }

        public static void Register(params IRepository[] repositories)
        {
            Repositories.ForEach(Register);
        }

        public static T Repository<T>()
        {
            return Repositories.Find(i => i.GetType() == typeof(T)).As<T>();
        }
    }
}
