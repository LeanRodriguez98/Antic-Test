using AnticTest.Data.Architecture;
using AnticTest.Data.Blackboard;
using AnticTest.Data.Entities.Factory;
using AnticTest.DataModel.Map;
using AnticTest.Services.Provider;

namespace AnticTest.Architecture.GameLogic
{
	public class GameLogic : IService
	{
		private EntityFactory entityFactory;
		private Map<Cell> map;

		public GameLogic(MapArchitectureData levelData)
		{
			map = new Map<Cell>(levelData);
			ServiceProvider.Instance.AddService<Map<Cell>>(map);
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
