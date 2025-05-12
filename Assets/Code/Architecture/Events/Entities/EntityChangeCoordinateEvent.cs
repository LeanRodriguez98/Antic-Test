using AnticTest.DataModel.Grid;
using AnticTest.Systems.Events;

namespace AnticTest.Architecture.Events
{
	public struct EntityChangeCoordinateEvent : IEvent
	{
		public uint entityId;
		public ICoordinate oldCoordinate;
		public ICoordinate newCoordinate;

		public EntityChangeCoordinateEvent(uint entityId, ICoordinate oldCoordinate, ICoordinate newCoordinate)
		{
			this.entityId = entityId;
			this.oldCoordinate = oldCoordinate;
			this.newCoordinate = newCoordinate;
		}
	}
}
