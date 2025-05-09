using AnticTest.Data.Entities.Factory;
using AnticTest.DataModel.Entities;
using AnticTest.DataModel.Grid;
using AnticTest.Services.Provider;
using UnityEngine;

namespace AnticTest.Data.Architecture
{
	[CreateAssetMenu(fileName = "New Enemy Bug Architecture Data", menuName = "AnticTest/Data/Architecture/Entities/Enemy Bug")]
	public sealed class EnemyBugArchitectureData : MobileEntityArchitectureData<EnemyBug<Cell<Coordinate>, Coordinate>, Cell<Coordinate>, Coordinate>
	{
		public override EnemyBug<Cell<Coordinate>, Coordinate> Get(Coordinate coordinate)
		{
			return Get((object)coordinate);
		}

		protected override EnemyBug<Cell<Coordinate>, Coordinate> Get(params object[] parameters)
		{
			return ServiceProvider.Instance.GetService<EntityFactory>().CreateEntity(this, (Coordinate)parameters[0], GetParameters()); ;
		}
	}
}
