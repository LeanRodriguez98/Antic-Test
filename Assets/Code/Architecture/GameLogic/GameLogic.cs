using AnticTest.Data.Architecture;
using AnticTest.Data.Entities.Factory;
using AnticTest.DataModel.Grid;
using AnticTest.Services.Provider;

namespace AnticTest.Architecture.GameLogic
{
	public class GameLogic : IService
	{
		private EntityFactory entityFactory;
		private Map<Cell<Coordinate>, Coordinate> map;

		public GameLogic(MapArchitectureData levelData)
		{
			map = new Map<Cell<Coordinate>, Coordinate>(levelData);
			ServiceProvider.Instance.AddService<Map<Cell<Coordinate>, Coordinate>>(map);
			entityFactory = new EntityFactory();
			ServiceProvider.Instance.AddService<EntityFactory>(entityFactory);
			ServiceProvider.Instance.AddService<GameLogic>(this);
		}

		public void InitSimulation()
		{
			map.SpawnInitialEntities();
		}
	}
}
