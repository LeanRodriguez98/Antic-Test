using AnticTest.Data.Architecture;
using AnticTest.Data.Blackboard;
using AnticTest.Data.Entities.Factory;
using AnticTest.DataModel.Entities;
using AnticTest.DataModel.Grid;
using AnticTest.Gameplay.Utils;
using AnticTest.Services.Provider;
using UnityEngine;

namespace AnticTest.Gameplay.Entities.Factory
{
	public class GameEntityFactory : MonoBehaviour
	{
		DataBlackboard DataBlackboard => ServiceProvider.Instance.GetService<DataBlackboard>();
		private GameObject entityContainer;

		public void Init()
		{
			ServiceProvider.Instance.GetService<EntityFactory>().OnEntityCreated += SpawnGameplayEntity;
			entityContainer = new GameObject("EntityContainer");
			entityContainer.transform.parent = transform;
		}

		private void SpawnGameplayEntity(ArchitectureData entityArchitectureData, IEntity entity)
		{
			GameObject entityPrefab = DataBlackboard.GetPrefabFromData(entityArchitectureData);
			Instantiate(entityPrefab, GridUtils.CoordinateToWorld(new Coordinate(entity.GetCoordinate())), Quaternion.identity, entityContainer.transform);
		}
	}
}
