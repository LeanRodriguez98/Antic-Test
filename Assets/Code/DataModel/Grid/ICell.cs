namespace AnticTest.DataModel.Grid
{
	public interface ICell<TCoordinate>
		where TCoordinate : struct, ICoordinate
	{
		public void Init(TCoordinate coord, CellType type, CellHeight height);
		public void SetCoordinate(TCoordinate coord);
		public void SetType(CellType type);
		public void SetHeight(CellHeight height);
		public CellType GetCellType();
		public CellHeight GetHeight();
		public TCoordinate GetCoordinate();
		public TCoordinate[] GetNeighbors();
	}
}
