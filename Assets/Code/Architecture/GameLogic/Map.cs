using AnticTest.Data.Architecture;
using AnticTest.Data.Blackboard;
using AnticTest.DataModel.Entities;
using AnticTest.DataModel.Map;
using AnticTest.Services.Provider;
using System.Collections.Generic;

namespace AnticTest.Architecture.GameLogic
{
	public class Map<TCell> : IService
		where TCell : class, ICell, new()
	{
		DataBlackboard DataBlackboard => ServiceProvider.Instance.GetService<DataBlackboard>();

		private Grid<TCell> grid;

		private List<Ant> ants;
		private List<EnemyBug> enemies;
		private Flag flag;

		private MapArchitectureData levelData;

		public Coordinate Size => grid.GetSize();

		public Map(MapArchitectureData levelData)
		{
			this.levelData = levelData;
			grid = new Grid<TCell>(new Coordinate(levelData.Map.width, levelData.Map.height));
			ants = new List<Ant>();
			enemies = new List<EnemyBug>();
			CreateTerrain();
		}

		public void CreateTerrain()
		{
			for (int y = 0; y < levelData.Map.height; y++)
			{
				for (int x = 0; x < levelData.Map.width; x++)
				{
					TCell newCell = new TCell();
					newCell.Init(new Coordinate(x, y),
						levelData.Map[x, y].cellArchitectureData.cellType,
						levelData.Map[x, y].cellArchitectureData.cellHeight);
					grid.SetCell(newCell);
				}
			}
		}

		public void SpawnInitialEntities()
		{
			for (int y = 0; y < levelData.Map.height; y++)
			{
				for (int x = 0; x < levelData.Map.width; x++)
				{
					ArchitectureData entityArchitectureData = levelData.Map[x, y].entityArchitectureData;
					if (levelData.Map[x, y].entityArchitectureData == null)
						continue;

					if (entityArchitectureData is FlagArchitectureData)
						flag = DataBlackboard.GetArchitectureData<FlagArchitectureData>(entityArchitectureData.name)?.Get(new Coordinate(x, y));
					else if (entityArchitectureData is AntArchitectureData)
						ants.Add(DataBlackboard.GetArchitectureData<AntArchitectureData>(entityArchitectureData.name)?.Get(new Coordinate(x, y)));
					else if (entityArchitectureData is EnemyBugArchitectureData)
						enemies.Add(DataBlackboard.GetArchitectureData<EnemyBugArchitectureData>(entityArchitectureData.name)?.Get(new Coordinate(x, y)));
				}
			}
		}

		public TCell this[Coordinate coordinate] { get { return GetCell(coordinate); } }

		public TCell GetCell(Coordinate coordinate)
		{
			return grid.GetCell(coordinate);
		}
	}
}
