using AnticTest.Architecture.Events;
using AnticTest.Architecture.Exceptions;
using AnticTest.Data.Architecture;
using AnticTest.Data.Blackboard;
using AnticTest.DataModel.Entities;
using AnticTest.DataModel.Grid;
using AnticTest.Systems.Events;
using AnticTest.Systems.Provider;
using System;
using System.Collections.Generic;

namespace AnticTest.Architecture.GameLogic
{
	public class EntityRegistry<TCell, TCoordinate> : IService, IDisposable
		where TCell : class, ICell<TCoordinate>, new()
		where TCoordinate : struct, ICoordinate
	{
		private DataBlackboard DataBlackboard => ServiceProvider.Instance.GetService<DataBlackboard>();
		private Map<TCell, TCoordinate> Map => ServiceProvider.Instance.GetService<Map<TCell, TCoordinate>>();
		private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

		private List<Ant<TCell, TCoordinate>> ants;
		private List<EnemyBug<TCell, TCoordinate>> enemies;
		private Flag<TCell, TCoordinate> flag;

		private Dictionary<uint, IEntity> entities;
		private List<IEntity> toDestroy;

		public EntityRegistry()
		{
			ants = new List<Ant<TCell, TCoordinate>>();
			enemies = new List<EnemyBug<TCell, TCoordinate>>();
			entities = new Dictionary<uint, IEntity>();
			toDestroy = new List<IEntity>();

			EventBus.Subscribe<EntityDestroyEvent>(RemoveEntityFromRegistry);
		}

		public IEntity this[uint ID] => entities[ID];
		public IEnumerable<IEntity> Entities => entities.Values;
		public IEnumerable<MobileEntity<TCell, TCoordinate>> MobileEntites =>
			EnumerableExtensions.Concat<MobileEntity<TCell, TCoordinate>>(ants, enemies);
		public IEnumerable<Ant<TCell, TCoordinate>> Ants => ants;
		public IEnumerable<EnemyBug<TCell, TCoordinate>> Enemies => enemies;
		public Flag<TCell, TCoordinate> Flag => flag;

		public void SpawnInitialEntities(MapArchitectureData levelData)
		{
			for (int y = 0; y < levelData.Map.height; y++)
			{
				for (int x = 0; x < levelData.Map.width; x++)
				{
					ArchitectureData entityArchitectureData = levelData.Map[x, y].entityArchitectureData;
					if (levelData.Map[x, y].entityArchitectureData == null)
						continue;

					SpawnEntity(entityArchitectureData, x, y);
				}
			}

			if (flag == null)
				throw new BrokenGameRuleException("There can't be no flag on the map.");

			if (ants.Count == 0)
				throw new BrokenGameRuleException("There can't be no ants on the map.");

			if (enemies.Count == 0)
				throw new BrokenGameRuleException("There can't be no enemies on the map.");
		}

		public void SpawnEntity(ArchitectureData entityArchitectureData, int x, int y)
		{
			if (!entityArchitectureData.GetType().InheritsFromRawGeneric(typeof(EntityArchitectureData<,,>)))
				throw new InvalidCastException();

			Coordinate spawnCoordinate = new Coordinate(x, y);

			switch (entityArchitectureData)
			{
				case FlagArchitectureData:
					{
						if (flag != null)
							throw new BrokenGameRuleException("There cannot be more than one flag on the map.");

						flag = DataBlackboard
							.GetArchitectureData<FlagArchitectureData>(entityArchitectureData.name)?
							.Get(spawnCoordinate) as Flag<TCell, TCoordinate>;
						entities.Add(flag.GetID(), flag);
						break;
					}
				case AntArchitectureData:
					{
						Ant<TCell, TCoordinate> newAnt = DataBlackboard.GetArchitectureData<AntArchitectureData>(entityArchitectureData.name)?
							.Get(spawnCoordinate) as Ant<TCell, TCoordinate>;
						entities.Add(newAnt.GetID(), newAnt);
						ants.Add(newAnt);
						break;
					}
				case EnemyBugArchitectureData:
					{
						EnemyBug<TCell, TCoordinate> newEnemyBug = DataBlackboard.GetArchitectureData<EnemyBugArchitectureData>(entityArchitectureData.name)?
							.Get(spawnCoordinate) as EnemyBug<TCell, TCoordinate>;
						entities.Add(newEnemyBug.GetID(), newEnemyBug);
						enemies.Add(newEnemyBug);
						break;
					}
			}
		}

		private void RemoveEntityFromRegistry(EntityDestroyEvent entityDestroyEvent)
		{
			toDestroy.Add(entityDestroyEvent.entity);
		}

		public void RemoveDestroyedEntites()
		{
			foreach (IEntity entity in toDestroy)
			{
				if (entity is Ant<TCell, TCoordinate>)
					ants.Remove((Ant<TCell, TCoordinate>)entity);
				if (entity is EnemyBug<TCell, TCoordinate>)
					enemies.Remove((EnemyBug<TCell, TCoordinate>)entity);
				if (flag == entity)
					flag = null;

				if (entities.ContainsKey(entity.GetID()))
					entities.Remove(entity.GetID());
			}
			toDestroy.Clear();

			if (enemies.Count == 0)
				EventBus.Raise(new AllEnemyEntitiesDestroyedEvent());
		}

		public void Dispose()
		{
			EventBus.Unsubscribe<EntityDestroyEvent>(RemoveEntityFromRegistry);
		}
	}
}
