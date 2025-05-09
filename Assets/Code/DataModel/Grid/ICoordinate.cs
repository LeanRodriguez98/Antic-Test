namespace AnticTest.DataModel.Grid
{
	public interface ICoordinate
	{
		public int X { get; }
		public int Y { get; }
		public (int x, int y) GetCoordinate();
		public void Set(int x, int y);
		public void Set((int x, int y) coordinate);
		public void Set(ICoordinate coordinate);
		public int Distance(ICoordinate other);
	}
}