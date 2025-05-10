using AnticTest.Data.Architecture;
using AnticTest.Data.Blackboard;
using AnticTest.Data.Entities.Factory;
using AnticTest.DataModel.Grid;
using AnticTest.Systems.Events;
using AnticTest.Systems.Provider;

namespace AnticTest.Architecture.GameLogic
{
	public class GameLogic : IService
	{
		private Map<Cell<Coordinate>, Coordinate> Map => ServiceProvider.Instance.GetService<Map<Cell<Coordinate>, Coordinate>>();

		public GameLogic(MapArchitectureData levelData)
		{
			ServiceProvider.Instance.AddService<DataBlackboard>(new DataBlackboard());
			ServiceProvider.Instance.AddService<EventBus>(new EventBus());
			ServiceProvider.Instance.AddService<Map<Cell<Coordinate>, Coordinate>>(new Map<Cell<Coordinate>, Coordinate>(levelData));
			ServiceProvider.Instance.AddService<EntityFactory>(new EntityFactory());
			ServiceProvider.Instance.AddService<GameLogic>(this);
		}

		public void InitSimulation()
		{
			Map.SpawnInitialEntities();
		}
	}
}
