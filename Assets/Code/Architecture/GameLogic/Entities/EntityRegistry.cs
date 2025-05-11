using AnticTest.Data.Architecture;
using AnticTest.Data.Blackboard;
using AnticTest.DataModel.Entities;
using AnticTest.DataModel.Grid;
using AnticTest.Systems.Provider;
using System;
using System.Collections.Generic;

namespace AnticTest.Architecture.GameLogic
{
	public class EntityRegistry<TCell, TCoordinate> : IService
		where TCell : class, ICell<TCoordinate>, new()
		where TCoordinate : struct, ICoordinate
	{
		private DataBlackboard DataBlackboard => ServiceProvider.Instance.GetService<DataBlackboard>();
		private Map<TCell, TCoordinate> Map => ServiceProvider.Instance.GetService<Map<TCell, TCoordinate>>();


		private List<Ant<TCell, TCoordinate>> ants;
		private List<EnemyBug<TCell, TCoordinate>> enemies;
		private Flag<TCell, TCoordinate> flag;

		private Dictionary<uint, IEntity> entities;

		public EntityRegistry()
		{
			ants = new List<Ant<TCell, TCoordinate>>();
			enemies = new List<EnemyBug<TCell, TCoordinate>>();
			entities = new Dictionary<uint, IEntity>();
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
	}
}
