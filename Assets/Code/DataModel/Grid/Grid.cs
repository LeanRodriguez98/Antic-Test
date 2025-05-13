using CSharpExtensions.GridMatrix;
using System.Collections.Generic;

namespace AnticTest.DataModel.Grid
{
	public class Grid<TCell, TCoordinate>
		where TCell : class, ICell<TCoordinate>, new()
		where TCoordinate : struct, ICoordinate
	{
		private GridMatrix<TCell> map;
		private ICell<TCoordinate> selectedCell;

		public ICell<TCoordinate> SelectedCell { get => selectedCell; }

		public Grid(int sizeX, int sizeY)
		{
			map = new GridMatrix<TCell>(sizeX, sizeY, null);
		}

		public void SetCell(TCell cell)
		{
			map.Set(cell.GetCoordinate().X, cell.GetCoordinate().Y, cell);
		}

		public TCell this[TCoordinate coordinate] { get { return GetCell(coordinate); } }

		public TCell GetCell(TCoordinate coordinate)
		{
			if (!IsValid(coordinate))
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

		public void SetSelectedCell(TCoordinate coordinate)
		{
			SetSelectedCell(GetCell(coordinate));
		}

		public void SetSelectedCell(ICell<TCoordinate> cell)
		{
			selectedCell = cell;
		}

		public bool IsValid(TCoordinate coordinate) 
		{
			return map.IsValid(coordinate.X, coordinate.Y);
		}
	}
}