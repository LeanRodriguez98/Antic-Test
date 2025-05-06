using AnticTest.Architecture.Entities;
using AnticTest.Architecture.Entities.Factory;
using AnticTest.Architecture.Map;
using AnticTest.Architecture.Services;
using AnticTest.Architecture.Utils;
using System.Collections.Generic;

namespace AnticTest.Architecture.GameLogic
{
	public class GameLogic : IService
	{
		private Grid<Cell> map;

		private List<Ant> ants;
		private List<EnemyBug> enemies;
		private Flag flag;

		private EntityFactory entityFactory;

		public GameLogic(Coordinate mapSize)
		{
			map = new Grid<Cell>(mapSize);
			entityFactory = new EntityFactory();

			Coordinate flagCoordinate = mapSize / 2;
			flag = entityFactory.CreateEntity<Flag>(flagCoordinate);

			ants = new List<Ant>();
			foreach (Coordinate coordinate in CoordinateUtils.GetCoordinatesInRadius(flagCoordinate, 2))
			{
				ants.Add(entityFactory.CreateEntity<Ant>(coordinate));
			}

			enemies = new List<EnemyBug>();
			for (int y = 0; y < mapSize.y; y++)
			{
				enemies.Add(entityFactory.CreateEntity<EnemyBug>(new Coordinate(0, y)));
			}

			ServiceProvider.Instance.AddService(typeof(GameLogic), this);
		}
	}
}
