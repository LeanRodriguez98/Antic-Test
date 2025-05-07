using System;
using System.Collections.Generic;

namespace AnticTest.Services.Provider
{
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

		public void AddService<ServiceType>(IService service) where ServiceType : class, IService
		{
			if (!_services.ContainsKey(typeof(ServiceType)))
				_services.Add(typeof(ServiceType), service);
		}

		public bool RemoveService<ServiceType>() where ServiceType : class, IService
		{
			if (!_services.ContainsKey(typeof(ServiceType)))
				throw new KeyNotFoundException();
			return _services.Remove(typeof(ServiceType));
		}

		public object GetService(Type type)
		{
			if (!typeof(IService).IsAssignableFrom(type))
				throw new InvalidCastException();
			IService service;
			_services.TryGetValue(type, out service);
			return service;
		}

		public ServiceType GetService<ServiceType>() where ServiceType : class, IService
		{
			return GetService(typeof(ServiceType)) as ServiceType;
		}
	}
}

