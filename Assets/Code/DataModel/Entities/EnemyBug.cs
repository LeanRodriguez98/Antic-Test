using AnticTest.DataModel.Grid;

namespace AnticTest.DataModel.Entities
{
	public sealed class EnemyBug<TCell, TCoordinate> : MobileEntity<TCell, TCoordinate>
		where TCell : class, ICell<TCoordinate>, new()
		where TCoordinate : struct, ICoordinate
	{
		public EnemyBug(TCoordinate coordinate, uint ID) : base(coordinate, ID) { }

		protected override void SetParameters(params object[] parameters)
		{
			base.SetParameters(parameters);
		}
	}
}
