using System.Collections.Generic;

namespace AnticTest.DataModel.Grid
{
	public class Grid<TCell, TCoordinate>
		where TCell : class, ICell<TCoordinate>, new()
		where TCoordinate : struct, ICoordinate
	{
		private GridMatrix<TCell> map;

		private TCoordinate selectedCoordinate;

		public TCoordinate SelectedCoordinate { get => selectedCoordinate; }

		public CellEvent<TCoordinate> OnCellSelected;
		public CellEvent<TCoordinate> OnCellDeselected;
		public CellEvent<TCoordinate> OnCellHover;

		public Grid(int sizeX, int sizeY)
		{
			OnCellSelected += SetSelectedCoordinate;
			map = new GridMatrix<TCell>(sizeX, sizeY, null);
		}

		public void SetCell(TCell cell)
		{
			map.Set(cell.GetCoordinate().X, cell.GetCoordinate().Y, cell);
		}

		public TCell this[TCoordinate coordinate] { get { return GetCell(coordinate); } }

		public TCell GetCell(TCoordinate coordinate)
		{
			if (!map.IsValid(coordinate.X, coordinate.Y))
				return null;
			return map[coordinate.X, coordinate.Y];
		}

		public (int x, int y) GetSize()
		{
			return map.Lenght;
		}

		public IEnumerable<TCell> GetGraph()
		{
			return map;
		}

		private void SetSelectedCoordinate(ICell<TCoordinate> cell)
		{
			selectedCoordinate = cell.GetCoordinate();
		}
	}
}