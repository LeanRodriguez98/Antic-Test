namespace AnticTest.Architecture.Map
{
	public class Grid<TCell> where TCell : class, ICell, new()
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

			for (int x = 0; x < size.x; x++)
			{
				for (int y = 0; y < size.y; y++)
				{
					TCell newCell = new TCell();
					newCell.Init(new Coordinate(x, y), CellType.Grass, CellHeight.One);
					SetCell(newCell);
				}
			}
		}

		public void SetCell(TCell cell)
		{
			map[cell.GetCoordinate().x, cell.GetCoordinate().y] = cell;
		}

		public TCell this[Coordinate coord] { get { return GetCell(coord); } }

		public TCell GetCell(Coordinate coord)
		{
			if (coord.x < 0 || coord.x >= map.GetLength(0) || coord.y < 0 || coord.y >= map.GetLength(1))
				return null;
			return map[coord.x, coord.y];
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