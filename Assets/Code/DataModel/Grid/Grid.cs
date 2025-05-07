using AnticTest.Services.Provider;

namespace AnticTest.DataModel.Map
{
	public class Grid<TCell> : IService
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

			System.Random random = new System.Random();

			for (int x = 0; x < size.x; x++)
			{
				for (int y = 0; y < size.y; y++)
				{
					TCell newCell = new TCell();
					
					//Random testing map, delete
					CellType randomType = (CellType)random.Next((int)CellType.Water, (int)CellType.Mountain + 1);
					CellHeight cellHeight = CellHeight.One;
					switch (randomType)
					{
						case CellType.Water:
							cellHeight = CellHeight.Zero;
							break;
						case CellType.Grass:
							cellHeight = (CellHeight)random.Next((int)CellHeight.Zero, (int)CellHeight.One + 1);
							break;
						case CellType.Mountain:
							cellHeight = CellHeight.Two;
							break;
						default:
							break;
					}

					newCell.Init(new Coordinate(x, y), randomType, cellHeight);
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