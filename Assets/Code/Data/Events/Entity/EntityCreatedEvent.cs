using AnticTest.Data.Architecture;
using AnticTest.DataModel.Entities;
using AnticTest.Systems.Events;

namespace AnticTest.Data.Events
{
	public struct EntityCreatedEvent : IEvent
	{
		public ArchitectureData entityArchitectureData;
		public IEntity entity;

		public EntityCreatedEvent(ArchitectureData entityArchitectureData, IEntity entity)
		{
			this.entityArchitectureData = entityArchitectureData;
			this.entity = entity;
		}
	}
}
