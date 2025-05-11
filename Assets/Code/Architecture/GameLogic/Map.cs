using AnticTest.Architecture.Events;
using AnticTest.Data.Architecture;
using AnticTest.DataModel.Grid;
using AnticTest.Systems.Events;
using AnticTest.Systems.Provider;
using System;

namespace AnticTest.Architecture.GameLogic
{
	public class Map<TCell, TCoordinate> : IService
		where TCell : class, ICell<TCoordinate>, new()
		where TCoordinate : struct, ICoordinate
	{
		private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

		private Grid<TCell, TCoordinate> grid;
		private float distanceBetweenCells;

		public (int x, int y) Size => grid.GetSize();
		public float DistanceBetweenCells => distanceBetweenCells;
		public Grid<TCell, TCoordinate> Grid => grid;

		public Map(MapArchitectureData levelData)
		{
			grid = new Grid<TCell, TCoordinate>(levelData.Map.width, levelData.Map.height);
			this.distanceBetweenCells = levelData.LogicalDistanceBetweenCells;
			CreateTerrain(levelData);
			EventBus.Subscribe<CellSelectedEvent<TCoordinate>>(OnCellSelected);
			EventBus.Subscribe<CellDeselectedEvent>(OnCellDeselected);
		}

		private void OnCellSelected(CellSelectedEvent<TCoordinate> cellSelectedEvent)
		{
			grid.SetSelectedCell(cellSelectedEvent.selectedCoordinate);
		}

		private void OnCellDeselected(CellDeselectedEvent cellDeselectedEvent)
		{
			grid.SetSelectedCell(null);
		}

		public void CreateTerrain(MapArchitectureData levelData)
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

		public TCell this[TCoordinate coordinate] => GetCell(coordinate);

		public TCell GetCell(TCoordinate coordinate)
		{
			return grid.GetCell(coordinate);
		}

		public TCell this[ICoordinate coordinate] => GetCell(coordinate);

		public TCell GetCell(ICoordinate coordinate)
		{
			if (coordinate is TCoordinate)
				return grid.GetCell((TCoordinate)coordinate);
			throw new InvalidCastException();
		}

		public bool IsInBorders(TCoordinate coordinate)
		{
			return grid.IsValid(coordinate);
		}
	}
}
