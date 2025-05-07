using AnticTest.DataModel.Map;

namespace AnticTest.DataModel.Entities
{
	public sealed class Ant : MobileEntity
	{
		public Ant(Coordinate coordinate, uint ID) : base(coordinate, ID) { }
	}
}
