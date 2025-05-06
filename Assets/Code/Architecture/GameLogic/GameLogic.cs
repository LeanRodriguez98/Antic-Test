using AnticTest.Architecture.Entities;
using AnticTest.Architecture.Map;
using AnticTest.Architecture.Utils;
using System.Collections.Generic;

namespace AnticTest.Architecture.GameLogic
{
	public class GameLogic
	{
		private Grid<Cell> map;

		private List<Ant> ants;
		private List<EnemyBug> enemies;
		private Flag flag;

		public GameLogic(Coordinate mapSize)
		{
			map = new Grid<Cell>(mapSize);

			Coordinate flagCoordinate = mapSize / 2;
			flag = new Flag(flagCoordinate);

			ants = new List<Ant>();
			foreach (Coordinate coordinate in CoordinateUtils.GetCoordinatesInRadius(flagCoordinate, 2))
			{
				ants.Add(new Ant(coordinate));
			}

			enemies = new List<EnemyBug>();
			for (int y = 0; y < mapSize.y; y++)
			{
				enemies.Add(new EnemyBug(new Coordinate(0, y)));
			}
		}
	}
}
