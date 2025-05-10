using AnticTest.DataModel.Grid;
using AnticTest.Systems.Events;

namespace AnticTest.Data.Events
{
	public struct EntityMovedEvent : IEvent
	{
		public uint entityId;
		public ICoordinate originPosition;
		public ICoordinate targetPosition;
		public float traveledDistance;

		public EntityMovedEvent(uint entityId, ICoordinate originPosition, ICoordinate targetPosition, float traveledDistance)
		{
			this.entityId = entityId;
			this.originPosition = originPosition;
			this.targetPosition = targetPosition;
			this.traveledDistance = traveledDistance;
		}
	}
}
