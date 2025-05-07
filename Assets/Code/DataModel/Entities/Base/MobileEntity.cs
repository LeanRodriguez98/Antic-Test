using AnticTest.DataModel.Map;

namespace AnticTest.DataModel.Entities
{
	public abstract class MobileEntity : Entity
	{
		public static EntityEvent OnEntityMoved;
		public CoordinateEvent OnThisEntityMoved;

		public MobileEntity(Coordinate coordinate, uint ID) : base(coordinate, ID) { }

		public override void SetCoordinate(Coordinate coordinate)
		{
			base.SetCoordinate(coordinate);
			OnEntityMoved?.Invoke(this);
			OnThisEntityMoved?.Invoke(this.coordinate);
		}
	}
}
