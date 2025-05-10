using AnticTest.Data.Blackboard;
using AnticTest.Data.Events;
using AnticTest.Gameplay.Events;
using AnticTest.Gameplay.Utils;
using AnticTest.Systems.Events;
using AnticTest.Systems.Provider;
using UnityEngine;

namespace AnticTest.Gameplay.Components
{ 
	public class GameEntityFactory : GameComponent
	{
		private GameObject entityContainer;

		public override void Init()
		{
			EventBus.Subscribe<EntityCreatedEvent>(SpawnGameplayEntity);
			entityContainer = new GameObject("EntityContainer");
			entityContainer.transform.parent = transform;
		}

		public override void Dispose()
		{
			EventBus.Unsubscribe<EntityCreatedEvent>(SpawnGameplayEntity);
		}

		private void SpawnGameplayEntity(EntityCreatedEvent entityCreatedEvent)
		{
			GameObject newEntityGameObject = Instantiate(DataBlackboard.GetPrefabFromData(entityCreatedEvent.entityArchitectureData),
														 GridUtils.CoordinateToWorld(entityCreatedEvent.entity.GetCoordinate()),
														 Quaternion.identity, entityContainer.transform);
			EventBus.Raise(new GameEntityCreatedEvent(entityCreatedEvent.entity.GetID(), newEntityGameObject));
		}
	}
}
