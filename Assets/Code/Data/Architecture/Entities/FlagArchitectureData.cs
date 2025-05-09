using AnticTest.Data.Entities.Factory;
using AnticTest.DataModel.Entities;
using AnticTest.DataModel.Grid;
using AnticTest.Services.Provider;
using UnityEngine;

namespace AnticTest.Data.Architecture
{
	[CreateAssetMenu(fileName = "New Flag Architecture Data", menuName = "AnticTest/Data/Architecture/Entities/Flag")]
	public sealed class FlagArchitectureData : EntityArchitectureData<Flag<Cell<Coordinate>, Coordinate>, Cell<Coordinate>, Coordinate>
	{
		public override Flag<Cell<Coordinate>, Coordinate> Get(Coordinate coordinate)
		{
			return Get((object)coordinate);
		}

		protected override Flag<Cell<Coordinate>, Coordinate> Get(params object[] parameters)
		{
			return ServiceProvider.Instance.GetService<EntityFactory>().CreateEntity(this, (Coordinate)parameters[0], GetParameters());
		}

		public override object[] GetParameters()
		{
			return new object[0];
		}
	}
}
