using AnticTest.DataModel.Grid;
using AnticTest.Systems.Events;

namespace AnticTest.Data.Events
{
	public class EntityMovedEvent : IEvent
	{
		public uint entityId;
		public ICoordinate newPosition;
		public EntityMovedEvent(uint entityId, ICoordinate newPosition)
		{
			this.entityId = entityId;
			this.newPosition = newPosition;
		}
	}
}
