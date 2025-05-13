using System;
using System.Collections.Generic;

namespace AnticTest.Systems.Provider
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

		private readonly Dictionary<Type, IService> services = new Dictionary<Type, IService>();

		private ServiceProvider() { }

		public void AddService<ServiceType>(IService service) where ServiceType : class, IService
		{
			if (!services.ContainsKey(typeof(ServiceType)))
				services.Add(typeof(ServiceType), service);
		}

		public bool RemoveService<ServiceType>() where ServiceType : class, IService
		{
			if (!services.ContainsKey(typeof(ServiceType)))
				throw new KeyNotFoundException();
			return services.Remove(typeof(ServiceType));
		}

		public object GetService(Type type)
		{
			if (!typeof(IService).IsAssignableFrom(type))
				throw new InvalidCastException();
			IService service;
			services.TryGetValue(type, out service);
			return service;
		}

		public ServiceType GetService<ServiceType>() where ServiceType : class, IService
		{
			return GetService(typeof(ServiceType)) as ServiceType;
		}

		public void ClearAllServices() 
		{
			services.Clear();
		}
	}
}

