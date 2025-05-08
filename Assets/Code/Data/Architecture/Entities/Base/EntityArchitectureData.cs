using AnticTest.DataModel.Entities;
using AnticTest.DataModel.Map;

namespace AnticTest.Data.Architecture
{
	public abstract class EntityArchitectureData<EntityType> : ArchitectureData<EntityType> where EntityType : Entity
	{
		public abstract EntityType Get(Coordinate coord);
	}
}
