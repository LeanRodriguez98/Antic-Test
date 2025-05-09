using AnticTest.DataModel.Grid;

namespace AnticTest.DataModel.Entities
{
	public sealed class Flag<TCell, TCoordinate> : Entity<TCell, TCoordinate>
		where TCell : class, ICell<TCoordinate>, new()
		where TCoordinate : struct, ICoordinate
	{
		public Flag(TCoordinate coordinate, uint ID) : base(coordinate, ID) { }

		protected override void SetParameters(params object[] parameters)
		{
		}
	}
}
