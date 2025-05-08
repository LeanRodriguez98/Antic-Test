using AnticTest.DataModel.Entities;
using UnityEngine;

namespace AnticTest.Data.Gameplay
{
	public class EntityGameplayData<EntityType> : GameplayData<EntityType> where EntityType : Entity
	{
		public GameObject prefab;
	}
}
