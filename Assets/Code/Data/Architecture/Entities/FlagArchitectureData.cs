using AnticTest.Data.Entities.Factory;
using AnticTest.DataModel.Entities;
using AnticTest.DataModel.Map;
using AnticTest.Services.Provider;
using UnityEngine;

namespace AnticTest.Data.Architecture
{
	[CreateAssetMenu(fileName = "New Flag Architecture Data", menuName = "AnticTest/Data/Architecture/Entities/Flag")]
	public sealed class FlagArchitectureData : EntityArchitectureData<Flag>
	{
		public override Flag Get(Coordinate coordinate)
		{
			return Get((object)coordinate);
		}

		protected override Flag Get(params object[] parameters)
		{
			return ServiceProvider.Instance.GetService<EntityFactory>().CreateEntity<Flag>(this, (Coordinate)parameters[0]);
		}
	}
}
