namespace AnticTest.Architecture.Map
{
	public delegate void CellEvent(ICell tile);

	public enum CellType
	{
		UNDEFINED = -1,
		Sea = 0,
		Grass = 1,
	}

	public enum CellHeight
	{
		UNDEFINED = -1,
		One = 1,
		Two = 2,
		Three = 3
	}

	public interface ICell
	{
		public void Init(Coordinate coord, CellType type, CellHeight height);
		public void SetCoordinate(Coordinate coord);
		public void SetType(CellType type);
		public void SetHeight(CellHeight height);
		public Coordinate GetCoordinate();
	}

	public class Cell : ICell
	{
		private Coordinate coordinate;
		private Coordinate[] neighbors = new Coordinate[6];
		private CellType type = CellType.UNDEFINED;
		private CellHeight height = CellHeight.One;

		public Coordinate RightNeighbor => neighbors[0];
		public Coordinate RightDownNeighbor => neighbors[1];
		public Coordinate LeftDownNeighbor => neighbors[2];
		public Coordinate LeftNeighbor => neighbors[3];
		public Coordinate LeftUpNeighbor => neighbors[4];
		public Coordinate RightUpNeighbor => neighbors[5];

		public Cell()
		{
		}

		public void Init(Coordinate coord, CellType type, CellHeight height)
		{
			SetCoordinate(coord);
			SetType(type);
			SetHeight(height);
		}

		public void SetCoordinate(Coordinate coord)
		{
			coordinate = new Coordinate(coord.x, coord.y);
			neighbors[0] = new Coordinate(coord.x + 1, coord.y);
			neighbors[1] = new Coordinate(coord.y % 2 == 0 ? coord.x : coord.x + 1, coord.y - 1);
			neighbors[2] = new Coordinate(coord.y % 2 == 0 ? coord.x - 1 : coord.x, coord.y - 1);
			neighbors[3] = new Coordinate(coord.x - 1, coord.y);
			neighbors[4] = new Coordinate(coord.y % 2 == 0 ? coord.x - 1 : coord.x, coord.y + 1);
			neighbors[5] = new Coordinate(coord.y % 2 == 0 ? coord.x : coord.x + 1, coord.y + 1);
		}

		public void SetType(CellType type)
		{
			this.type = type;
		}

		public void SetHeight(CellHeight height)
		{
			this.height = height;
		}

		public Coordinate[] GetNeighbors()
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

		public Coordinate GetCoordinate()
		{
			return coordinate;
		}
	}
}