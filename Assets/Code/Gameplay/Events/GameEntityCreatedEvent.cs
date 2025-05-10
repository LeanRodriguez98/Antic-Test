using AnticTest.Systems.Events;
using UnityEngine;

namespace AnticTest.Gameplay.Events
{
	public struct GameEntityCreatedEvent : IEvent
	{
		public uint entityID;
		public GameObject objectInstance;

		public GameEntityCreatedEvent(uint entityID, GameObject objectInstance) 
		{
			this.entityID = entityID;
			this.objectInstance = objectInstance;
		}
	}
}
