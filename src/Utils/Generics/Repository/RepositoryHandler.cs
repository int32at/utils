using System.Collections.Generic;
using int32.Utils.Extensions;
using int32.Utils.Generics.Repository.Contracts;
using int32.Utils.Generics.Singleton;

namespace int32.Utils.Generics.Repository
{
    public class RepositoryHandler : Singleton<RepositoryHandler>
    {
        private readonly List<IRepository> _repositories;

        public RepositoryHandler()
        {
            _repositories = new List<IRepository>();
        }

        public void Register(IRepository repository)
        {
            if (!_repositories.Contains(repository))
                _repositories.Add(repository);
        }

        public void Register(params IRepository[] repositories)
        {
            _repositories.ForEach(Register);
        }

        public T Repository<T>()
        {
            return _repositories.Find(i => i.GetType() == typeof(T)).As<T>();
        }
    }
}
