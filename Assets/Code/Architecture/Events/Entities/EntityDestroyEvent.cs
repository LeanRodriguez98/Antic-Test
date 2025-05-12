using AnticTest.DataModel.Entities;
using AnticTest.Systems.Events;

namespace AnticTest.Architecture.Events
{
	public struct EntityDestroyEvent : IEvent
	{
		public IEntity entity;

		public EntityDestroyEvent(IEntity entity)
		{
			this.entity = entity;
		}
	}
}
