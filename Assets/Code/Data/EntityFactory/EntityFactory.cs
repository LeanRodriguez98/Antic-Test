using AnticTest.Data.Architecture;
using AnticTest.DataModel.Entities;
using AnticTest.DataModel.Grid;
using AnticTest.Services.Provider;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AnticTest.Data.Entities.Factory
{
	public delegate void EntityFactoryEvent(ArchitectureData entityArchitectureData, IEntity entity);

	public sealed class EntityFactory : IService
	{
		private const string SET_PARAMETERS_METHOD_NAME = "SetParameters";

		public EntityFactoryEvent OnEntityCreated;
		private uint lastEntityID;

		private Dictionary<Type, ConstructorInfo> entityConstructors;
		private Dictionary<Type, MethodInfo> entityParametersSetters;

		public EntityFactory()
		{
			entityConstructors = new Dictionary<Type, ConstructorInfo>();
			entityParametersSetters = new Dictionary<Type, MethodInfo>();
		}

		public TEntity CreateEntity<TEntity, TCell, TCoordinate>(EntityArchitectureData<TEntity, TCell, TCoordinate> architectureData, TCoordinate coordinate, object[] parammeters)
			where TEntity : Entity<TCell, TCoordinate>
			where TCell : class, ICell<TCoordinate>, new()
			where TCoordinate : struct, ICoordinate
		{
			TEntity newEntity = null;
			Type entityType = typeof(TEntity);

			if (!entityConstructors.ContainsKey(entityType))
				RegisterEntiyMethods(entityType);

			newEntity = entityConstructors[entityType].
				Invoke(new object[] { coordinate, lastEntityID }) as TEntity;

			entityParametersSetters[entityType].Invoke(newEntity, new object[] { parammeters });

			lastEntityID++;
			OnEntityCreated?.Invoke(architectureData, newEntity);
			return newEntity;
		}

		private void RegisterEntiyMethods(Type entityType) 
		{
			if (entityType.IsClass && !entityType.IsAbstract && entityType.InheritsFromRawGeneric(typeof(Entity<,>)))
			{
				foreach (ConstructorInfo constructor in entityType.GetConstructors())
				{
					ParameterInfo[] parameters = constructor.GetParameters();
					if (parameters.Length == 2 &&
						typeof(ICoordinate).IsAssignableFrom(parameters[0].ParameterType) &&
						parameters[1].ParameterType == typeof(uint))
					{
						entityConstructors.Add(entityType, constructor);
						break;
					}
				}

				MethodInfo parametersSetter = entityType.GetMethod(SET_PARAMETERS_METHOD_NAME, BindingFlags.NonPublic | BindingFlags.Instance);
				if (parametersSetter != null)
					entityParametersSetters.Add(entityType, parametersSetter);
			}
		}
	}
}
