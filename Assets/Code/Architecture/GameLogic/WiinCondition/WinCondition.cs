using AnticTest.Architecture.Events;
using AnticTest.DataModel.Grid;
using AnticTest.Systems.Events;
using AnticTest.Systems.Provider;
using System;

namespace AnticTest.Architecture.GameLogic
{
	public class WinCondition : IDisposable
	{
		private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
		private EntityRegistry<Cell<Coordinate>, Coordinate> EntityRegistry =>
			ServiceProvider.Instance.GetService<EntityRegistry<Cell<Coordinate>, Coordinate>>();

		public WinCondition()
		{
			EventBus.Subscribe<AllEnemyEntitiesDestroyedEvent>(OnAllEnemiesDestroyed);
			EventBus.Subscribe<FlagCapturedEvent>(OnFlagCaptured);
		}

		private void OnAllEnemiesDestroyed(AllEnemyEntitiesDestroyedEvent _)
		{
			EventBus.Raise(new WinEvent());
		}

		private void OnFlagCaptured(FlagCapturedEvent _)
		{
			EventBus.Raise(new LoseEvent());
		}

		public void Dispose()
		{
			EventBus.Unsubscribe<AllEnemyEntitiesDestroyedEvent>(OnAllEnemiesDestroyed);
			EventBus.Unsubscribe<FlagCapturedEvent>(OnFlagCaptured);
		}
	}
}
