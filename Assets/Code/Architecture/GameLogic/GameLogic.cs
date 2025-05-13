using AnticTest.Data.Architecture;
using AnticTest.Data.Blackboard;
using AnticTest.Data.Entities.Factory;
using AnticTest.DataModel.Grid;
using AnticTest.Systems.Events;
using AnticTest.Systems.Provider;
using System;

namespace AnticTest.Architecture.GameLogic
{
	public class GameLogic : IDisposable
	{
		private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
		private EntityRegistry<Cell<Coordinate>, Coordinate> EntityRegistry => 
			ServiceProvider.Instance.GetService<EntityRegistry<Cell<Coordinate>, Coordinate>>();
		private EntitiesLogic<Cell<Coordinate>, Coordinate> EntitiesLogic => 
			ServiceProvider.Instance.GetService<EntitiesLogic<Cell<Coordinate>, Coordinate>>();
		private Map<Cell<Coordinate>, Coordinate> Map => 
			ServiceProvider.Instance.GetService<Map<Cell<Coordinate>, Coordinate>>();

		private MapArchitectureData levelData;
		private WinCondition winCondition;

		public GameLogic(MapArchitectureData levelData)
		{
			this.levelData = levelData;
			ServiceProvider.Instance.AddService<DataBlackboard>(new DataBlackboard());
			ServiceProvider.Instance.AddService<EventBus>(new EventBus());
			ServiceProvider.Instance.AddService<Map<Cell<Coordinate>, Coordinate>>
				(new Map<Cell<Coordinate>, Coordinate>(levelData));
			ServiceProvider.Instance.AddService<EntityRegistry<Cell<Coordinate>, Coordinate>>
				(new EntityRegistry<Cell<Coordinate>, Coordinate>());
			ServiceProvider.Instance.AddService<EntitiesLogic<Cell<Coordinate>, Coordinate>>
				(new EntitiesLogic<Cell<Coordinate>, Coordinate>());
			ServiceProvider.Instance.AddService<EntityFactory>(new EntityFactory());
			winCondition = new WinCondition();
		}

		public void InitSimulation()
		{
			EntityRegistry.SpawnInitialEntities(levelData);
			EntitiesLogic.Init();
		}

		public void Update(float deltaTime) 
		{
			EntitiesLogic.Update(deltaTime);
		}

		public void PostUpdate() 
		{
			EntityRegistry.RemoveDestroyedEntites();
		}

		public void Dispose()
		{
			EntityRegistry.Dispose();
			EntitiesLogic.Dispose();
			Map.Dispose();
			winCondition.Dispose();
			EventBus.ClearAll();
			ServiceProvider.Instance.ClearAllServices();
		}
	}
}
