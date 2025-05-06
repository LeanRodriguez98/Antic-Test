using AnticTest.Architecture.Map;

namespace AnticTest.Architecture.Entities
{
	public abstract class MobileEntity : Entity
	{
		public static EntityEvent OnEntityMoved;
		public CoordinateEvent OnThisEntityMoved;

		public MobileEntity(Coordinate coordinate) : base(coordinate) { }

		public override void SetCoordinate(Coordinate coordinate)
		{
			base.SetCoordinate(coordinate);
			OnEntityMoved?.Invoke(this);
			OnThisEntityMoved?.Invoke(this.coordinate);
		}
	}
}
