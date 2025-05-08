using AnticTest.Data.Entities.Factory;
using AnticTest.DataModel.Entities;
using AnticTest.DataModel.Map;
using AnticTest.Services.Provider;
using UnityEngine;

namespace AnticTest.Data.Architecture
{
	[CreateAssetMenu(fileName = "New Ant Architecture Data", menuName = "AnticTest/Data/Architecture/Entities/Ant")]
	public sealed class AntArchitectureData : MobileEntityArchitectureData<Ant>
	{
		public override Ant Get(Coordinate coordinate)
		{
			return Get((object)coordinate);
		}

		protected override Ant Get(params object[] parameters)
		{
			return ServiceProvider.Instance.GetService<EntityFactory>().CreateEntity<Ant>(this, (Coordinate)parameters[0]);
		}
	}
}
