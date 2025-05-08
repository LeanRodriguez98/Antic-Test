using AnticTest.Architecture.Utils;
using AnticTest.Data.Architecture;
using AnticTest.Data.Blackboard;
using AnticTest.Data.Entities.Factory;
using AnticTest.Data.Names;
using AnticTest.DataModel.Entities;
using AnticTest.DataModel.Map;
using AnticTest.Services.Provider;
using System.Collections.Generic;

namespace AnticTest.Architecture.GameLogic
{
	public class GameLogic : IService
	{
		DataBlackboard DataBlackboard => ServiceProvider.Instance.GetService<DataBlackboard>();

		private Grid<Cell> map;

		private List<Ant> ants;
		private List<EnemyBug> enemies;
		private Flag flag;

		private EntityFactory entityFactory;

		public GameLogic(Coordinate mapSize)
		{
			map = new Grid<Cell>(mapSize);
			ServiceProvider.Instance.AddService<Grid<Cell>>(map);
			entityFactory = new EntityFactory();
			ServiceProvider.Instance.AddService<EntityFactory>(entityFactory);
			ServiceProvider.Instance.AddService<GameLogic>(this);
		}

		public void InitSimulation() 
		{
			Coordinate flagCoordinate = map.GetSize() / 2;
			flag = DataBlackboard.GetArchitectureData<FlagArchitectureData>(EntityNames.FLAG_DATA_NAME)?.Get(flagCoordinate);

			ants = new List<Ant>();
			foreach (Coordinate coordinate in CoordinateUtils.GetCoordinatesInRadius(flagCoordinate, 2))
			{
				ants.Add(DataBlackboard.GetArchitectureData<AntArchitectureData>(EntityNames.ANT_DATA_NAME)?.Get(coordinate));
			}

			enemies = new List<EnemyBug>();
			for (int y = 0; y < map.GetSize().y; y++)
			{
				string enemyToSpawn = string.Empty;

				if (y % 3 == 0)
					enemyToSpawn = EntityNames.BEETLE_DATA_NAME;
				else if (y % 3 == 1)
					enemyToSpawn = EntityNames.APHID_DATA_NAME;
				else if (y % 3 == 2)
					enemyToSpawn = EntityNames.LADYBUG_DATA_NAME;

				enemies.Add(DataBlackboard.GetArchitectureData<EnemyBugArchitectureData>(enemyToSpawn)?.Get(new Coordinate(0, y)));
			}
		}
	}
}
