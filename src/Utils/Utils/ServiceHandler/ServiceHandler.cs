using System;
using System.Collections.Generic;
using int32.Utils.Extensions;
using int32.Utils.ServiceHandler.Contracts;

namespace int32.Utils.ServiceHandler
{
    public class ServiceHandler : IDisposable
    {
        private readonly List<IService> _services;

        public ServiceHandler()
        {
            _services = new List<IService>();
        }

        public void Register(IService service)
        {
            if (!_services.Contains(service))
                _services.Add(service);
        }

        public void Register(params IService[] services)
        {
            services.ForEach(Register);
        }

        public T Service<T>()
        {
            return _services.Find(i => i.GetType() == typeof(T)).As<T>();
        }
        public void Initialize()
        {
            _services.ForEach(i => i.Initialize());
        }

        public void Dispose()
        {
            _services.ForEach(i => i.Dispose());
        }
    }
}