using AnticTest.Architecture.Pathfinding;
using AnticTest.Data.Architecture;
using AnticTest.Data.Blackboard;
using AnticTest.DataModel.Entities;
using AnticTest.DataModel.Grid;
using AnticTest.Systems.Provider;
using System;
using System.Collections.Generic;

namespace AnticTest.Architecture.GameLogic
{
	public class Map<TCell, TCoordinate> : IService
		where TCell : class, ICell<TCoordinate>, new()
		where TCoordinate : struct, ICoordinate
	{
		DataBlackboard DataBlackboard => ServiceProvider.Instance.GetService<DataBlackboard>();

		private Grid<TCell, TCoordinate> grid;

		private List<Ant<TCell, TCoordinate>> ants;
		private List<EnemyBug<TCell, TCoordinate>> enemies;
		private Flag<TCell, TCoordinate> flag;

		private MapArchitectureData levelData;

		public (int x, int y) Size => grid.GetSize();

		public Map(MapArchitectureData levelData)
		{
			this.levelData = levelData;
			grid = new Grid<TCell, TCoordinate>(levelData.Map.width, levelData.Map.height);
			ants = new List<Ant<TCell, TCoordinate>>();
			enemies = new List<EnemyBug<TCell, TCoordinate>>();
			CreateTerrain();
		}

		public void CreateTerrain()
		{
			for (int y = 0; y < levelData.Map.height; y++)
			{
				for (int x = 0; x < levelData.Map.width; x++)
				{
					TCell newCell = new TCell();
					TCoordinate newCellCordinate = new TCoordinate();
					newCellCordinate.Set(x, y);
					newCell.Init(newCellCordinate,
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
						flag = (DataBlackboard.GetArchitectureData<FlagArchitectureData>(entityArchitectureData.name)?.Get(new Coordinate(x, y)) as Flag<TCell, TCoordinate>);
					else if (entityArchitectureData is AntArchitectureData)
						ants.Add(DataBlackboard.GetArchitectureData<AntArchitectureData>(entityArchitectureData.name)?.Get(new Coordinate(x, y)) as Ant<TCell, TCoordinate>);
					else if (entityArchitectureData is EnemyBugArchitectureData)
						enemies.Add(DataBlackboard.GetArchitectureData<EnemyBugArchitectureData>(entityArchitectureData.name)?.Get(new Coordinate(x, y)) as EnemyBug<TCell, TCoordinate>);
				}
			}

			foreach (Ant<TCell, TCoordinate> ant in ants)
			{
				ant.SetPathfinderSystem(() => grid, new EntityPathfinding<TCell, TCoordinate>());
			}

			foreach (EnemyBug<TCell, TCoordinate> enemyBug in enemies)
			{
				enemyBug.SetPathfinderSystem(() => grid, new EntityPathfinding<TCell, TCoordinate>());
				enemyBug.GoTo(grid[(TCoordinate)flag.GetCoordinate()]);
			}
		}

		public TCell this[TCoordinate coordinate] { get { return GetCell(coordinate); } }

		public TCell GetCell(TCoordinate coordinate)
		{
			return grid.GetCell(coordinate);
		}

		public TCell this[ICoordinate coordinate] { get { return GetCell(coordinate); } }

		public TCell GetCell(ICoordinate coordinate)
		{
			if (coordinate is TCoordinate)
				return grid.GetCell((TCoordinate)coordinate);
			throw new InvalidCastException();
		}
	}
}
