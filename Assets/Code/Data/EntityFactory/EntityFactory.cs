using AnticTest.Data.Architecture;
using AnticTest.DataModel.Entities;
using AnticTest.DataModel.Map;
using AnticTest.Services.Provider;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AnticTest.Data.Entities.Factory
{
	public delegate void EntityFactoryEvent(ArchitectureData entityArchitectureData, Entity entity);

	public sealed class EntityFactory : IService
	{
		private const string DATA_MODEL_ASSEMBLY_NAME = "DataModelAssembly";

		public EntityFactoryEvent OnEntityCreated;
		public EntityEvent OnEntityDestroyed;
		private uint lastEntityID;

		private Dictionary<Type, ConstructorInfo> entityConstructors;

		public EntityFactory()
		{
			LoadEntityConstructors();
		}

		private void LoadEntityConstructors()
		{
			entityConstructors = new Dictionary<Type, ConstructorInfo>();
			foreach (Type type in Assembly.Load(new AssemblyName(DATA_MODEL_ASSEMBLY_NAME)).GetTypes())
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

		public EntityType CreateEntity<EntityType>(EntityArchitectureData<EntityType> architectureData, Coordinate coordinate) where EntityType : Entity
		{
			EntityType newEntity = null;
			if (entityConstructors.ContainsKey(typeof(EntityType)))
			{
				newEntity = entityConstructors[typeof(EntityType)].
					Invoke(new object[] { coordinate, lastEntityID }) as EntityType;
				lastEntityID++;
				OnEntityCreated?.Invoke(architectureData, newEntity);
			}
			return newEntity;
		}
	}
}
