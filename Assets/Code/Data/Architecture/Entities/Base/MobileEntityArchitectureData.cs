using AnticTest.DataModel.Entities;
using UnityEngine;

namespace AnticTest.Data.Architecture
{
	public abstract class MobileEntityArchitectureData<EntityType> : EntityArchitectureData<EntityType> where EntityType : MobileEntity
	{
		[SerializeField] private int speed;
		[SerializeField] private int health;
	}
}
