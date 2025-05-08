using AnticTest.Data.Entities.Factory;
using AnticTest.DataModel.Entities;
using AnticTest.DataModel.Map;
using AnticTest.Services.Provider;
using UnityEngine;

namespace AnticTest.Data.Architecture
{
	[CreateAssetMenu(fileName = "New Enemy Bug Architecture Data", menuName = "AnticTest/Data/Architecture/Entities/Enemy Bug")]
	public sealed class EnemyBugArchitectureData : MobileEntityArchitectureData<EnemyBug>
	{
		public override EnemyBug Get(Coordinate coordinate)
		{
			return Get((object)coordinate);
		}

		protected override EnemyBug Get(params object[] parameters)
		{
			return ServiceProvider.Instance.GetService<EntityFactory>().CreateEntity<EnemyBug>(this, (Coordinate)parameters[0]);
		}
	}
}
