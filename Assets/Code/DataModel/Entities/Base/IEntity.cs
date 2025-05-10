using AnticTest.DataModel.Grid;

namespace AnticTest.DataModel.Entities
{
	public interface IEntity
	{
		public uint GetID();
		public ICoordinate GetCoordinate();
	}
}