namespace AnticTest.DataModel.Grid
{
	public enum CellType
	{
		UNDEFINED = -1,
		Water = 0,
		Grass = 1,
		Mountain = 2
	}

	public enum CellHeight
	{
		Zero = 0,
		One = 1,
		Two = 2,
	}

	public delegate void CellEvent<TCoordinate>(ICell<TCoordinate> cell) 
		where TCoordinate : struct, ICoordinate;

	public class Cell<TCoordinate> : ICell<TCoordinate>
		where TCoordinate : struct, ICoordinate
	{
		private TCoordinate coordinate;
		private TCoordinate[] neighbors = new TCoordinate[6];
		private CellType type = CellType.UNDEFINED;
		private CellHeight height = CellHeight.Zero;

		public TCoordinate RightNeighbor => neighbors[0];
		public TCoordinate RightDownNeighbor => neighbors[1];
		public TCoordinate LeftDownNeighbor => neighbors[2];
		public TCoordinate LeftNeighbor => neighbors[3];
		public TCoordinate LeftUpNeighbor => neighbors[4];
		public TCoordinate RightUpNeighbor => neighbors[5];

		public TCoordinate InvalidCoordinate
		{
			get
			{
				TCoordinate invalidCoordinate = new TCoordinate();
				invalidCoordinate.Set(int.MinValue, int.MinValue);
				return invalidCoordinate;
			}
		}

		public Cell()
		{
			for (int i = 0; i < neighbors.Length; i++)
			{
				neighbors[i] = InvalidCoordinate;
			}
		}

		public void Init(TCoordinate coordinate, CellType type, CellHeight height)
		{
			SetCoordinate(coordinate);
			SetType(type);
			SetHeight(height);
		}

		public void SetCoordinate(TCoordinate coordinate)
		{
			this.coordinate = coordinate;
			neighbors[0].Set(coordinate.X + 1, coordinate.Y);
			neighbors[1].Set(coordinate.Y % 2 == 0 ? coordinate.X : coordinate.X + 1, coordinate.Y - 1);
			neighbors[2].Set(coordinate.Y % 2 == 0 ? coordinate.X - 1 : coordinate.X, coordinate.Y - 1);
			neighbors[3].Set(coordinate.X - 1, coordinate.Y);
			neighbors[4].Set(coordinate.Y % 2 == 0 ? coordinate.X - 1 : coordinate.X, coordinate.Y + 1);
			neighbors[5].Set(coordinate.Y % 2 == 0 ? coordinate.X : coordinate.X + 1, coordinate.Y + 1);
		}

		public void SetType(CellType type)
		{
			this.type = type;
		}

		public void SetHeight(CellHeight height)
		{
			this.height = height;
		}

		public TCoordinate[] GetNeighbors()
		{
			return neighbors;
		}

		public CellHeight GetHeight()
		{
			return height;
		}

		public CellType GetCellType()
		{
			return type;
		}

		public TCoordinate GetCoordinate()
		{
			return coordinate;
		}
	}
}