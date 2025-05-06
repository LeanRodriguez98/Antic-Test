using System;
using System.Collections.Generic;

namespace AnticTest.Architecture.Services
{
    public interface IService
    {

    }

    public class ServiceProvider : IServiceProvider
    {
        private static ServiceProvider instance = null;
		public static ServiceProvider Instance 
        {
            get
            {
				if (instance == null)
                    instance = new ServiceProvider();
                return instance;
            } 
            private set => instance = value;
        }

        private readonly Dictionary<Type, IService> _services = new Dictionary<Type, IService>();

        private ServiceProvider() { }

		public void AddService(Type type, IService service)
        {
            if (!typeof(IService).IsAssignableFrom(type))
                throw new InvalidCastException();
            _services.Add(type, service);
        }

        public bool RemoveService(Type type)
        {
            if (!typeof(IService).IsAssignableFrom(type))
                throw new InvalidCastException();
            return _services.Remove(type);
        }

        public object GetService(Type type)
        {
            if (!typeof(IService).IsAssignableFrom(type))
                throw new InvalidCastException();
            IService service;
            _services.TryGetValue(type, out service);
            return service;
        }
    }
}

