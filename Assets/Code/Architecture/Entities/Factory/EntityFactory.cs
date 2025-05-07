using AnticTest.Architecture.Map;
using AnticTest.Architecture.Services;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AnticTest.Architecture.Entities.Factory
{
	public sealed class EntityFactory : IService
	{
		public EntityEvent OnEntityCreated;
		public EntityEvent OnEntityDestroyed;
		private uint lastEntityID;

		private Dictionary<Type, ConstructorInfo> entityConstructors;

		public EntityFactory()
		{
			LoadEntityConstructors();
			ServiceProvider.Instance.AddService<EntityFactory>(this);
		}

		private void LoadEntityConstructors()
		{
			entityConstructors = new Dictionary<Type, ConstructorInfo>();
			foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
			{
				if (type.IsClass && !type.IsAbstract &&
					typeof(Entity).IsAssignableFrom(type) && type != typeof(Entity))
				{
					foreach (ConstructorInfo constructor in type.GetConstructors())
					{
						ParameterInfo[] parameters = constructor.GetParameters();
						if (parameters.Length == 2 && parameters[0].ParameterType == typeof(Coordinate) &&
							parameters[1].ParameterType == typeof(uint))
						{
							entityConstructors.Add(type, constructor);
							break;
						}
					}
				}
			}
		}

		public EntityType CreateEntity<EntityType>(Coordinate coordinate) where EntityType : Entity
		{
			EntityType newEntity = null;
			if (entityConstructors.ContainsKey(typeof(EntityType)))
			{
				newEntity = entityConstructors[typeof(EntityType)].
					Invoke(new object[] { coordinate, lastEntityID }) as EntityType;
				lastEntityID++;
				OnEntityCreated?.Invoke(newEntity);
			}
			return newEntity;
		}
	}
}
