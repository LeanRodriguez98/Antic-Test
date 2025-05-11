using AnticTest.Data.Events;
using AnticTest.Gameplay.Events;
using AnticTest.Gameplay.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace AnticTest.Gameplay.Components
{
	public class GameEntityRegistry : GameComponent
	{
		private Dictionary<uint, GameObject> gameEntities;

		public override void Init()
		{
			gameEntities = new Dictionary<uint, GameObject>();
			EventBus.Subscribe<GameEntityCreatedEvent>(OnGameEntityCreated);
			EventBus.Subscribe<EntityMovedEvent>(OnEntityMoved);
		}

		public override void Dispose()
		{
			EventBus.Unsubscribe<GameEntityCreatedEvent>(OnGameEntityCreated);
			EventBus.Unsubscribe<EntityMovedEvent>(OnEntityMoved);
		}

		private void OnEntityMoved(EntityMovedEvent entityMovedEvent)
		{
			gameEntities[entityMovedEvent.entityId].transform.position =
				GridUtils.MovementToWorld(entityMovedEvent.originPosition,
				entityMovedEvent.targetPosition, entityMovedEvent.traveledDistance);

			Vector3 direction = GridUtils.CoordinateToWorld(entityMovedEvent.targetPosition) -
				GridUtils.CoordinateToWorld(entityMovedEvent.originPosition);
			direction.y = 0;
			gameEntities[entityMovedEvent.entityId].transform.rotation =
				Quaternion.LookRotation(direction, Vector3.up);
		}

		public void OnGameEntityCreated(GameEntityCreatedEvent gameEntityCreatedEvent)
		{
			gameEntities.Add(gameEntityCreatedEvent.entityID, gameEntityCreatedEvent.objectInstance);
		}
	}
}
