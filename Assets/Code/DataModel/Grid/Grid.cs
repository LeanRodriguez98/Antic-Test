namespace AnticTest.DataModel.Map
{
	public class Grid<TCell>
		where TCell : class, ICell, new()
	{
		private TCell[,] map;

		private Coordinate selectedCoordinate;

		public Coordinate SelectedCoordinate { get => selectedCoordinate; }

		public CellEvent OnCellSelected;
		public CellEvent OnCellDeselected;
		public CellEvent OnCellHover;

		public Grid(Coordinate size)
		{
			OnCellSelected += SetSelectedCoordinate;
			map = new TCell[size.x, size.y];
		}

		public void SetCell(TCell cell)
		{
			map[cell.GetCoordinate().x, cell.GetCoordinate().y] = cell;
		}

		public TCell this[Coordinate coordinate] { get { return GetCell(coordinate); } }

		public TCell GetCell(Coordinate coordinate)
		{
			if (coordinate.x < 0 || coordinate.x >= map.GetLength(0) || coordinate.y < 0 || coordinate.y >= map.GetLength(1))
				return null;
			return map[coordinate.x, coordinate.y];
		}

		public Coordinate GetSize()
		{
			return new Coordinate(map.GetLength(0), map.GetLength(1));
		}

		private void SetSelectedCoordinate(ICell cell)
		{
			selectedCoordinate = cell.GetCoordinate();
		}
	}
}