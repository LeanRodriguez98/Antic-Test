using AnticTest.Architecture.Events;
using AnticTest.DataModel.Entities;
using AnticTest.DataModel.Grid;
using AnticTest.Systems.Events;
using AnticTest.Systems.Provider;
using System;
using System.Collections.Generic;

namespace AnticTest.Architecture.GameLogic
{
	class CombatFinder<TCell, TCoordinate> : IDisposable
		where TCell : class, ICell<TCoordinate>, new()
		where TCoordinate : struct, ICoordinate
	{
		private EntityRegistry<TCell, TCoordinate> EntityRegistry => ServiceProvider.Instance.GetService<EntityRegistry<TCell, TCoordinate>>();
		private Map<TCell, TCoordinate> Map => ServiceProvider.Instance.GetService<Map<TCell, TCoordinate>>();
		private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

		public CombatFinder()
		{
			EventBus.Subscribe<EntityChangeCoordinateEvent>(UpdateCurrentOponents);
			EventBus.Subscribe<EntityDestroyEvent>(UpdateCurrentOponents);
		}

		private void UpdateCurrentOponents(EntityChangeCoordinateEvent entityChangeCoordinateEvent)
		{
			UpdateOponentsIn((TCoordinate)entityChangeCoordinateEvent.oldCoordinate);
			UpdateOponentsIn((TCoordinate)entityChangeCoordinateEvent.newCoordinate);
		}

		private void UpdateCurrentOponents(EntityDestroyEvent entityDestroyEvent)
		{
			UpdateOponentsIn((TCoordinate)entityDestroyEvent.entity.GetCoordinate());
		}

		private void UpdateOponentsIn(TCoordinate coordinate)
		{
			foreach (KeyValuePair<Type, List<uint>> allEntiiesInCoordinate in Map.GetAllEntitiesIn(coordinate))
			{
				foreach (uint entityID in allEntiiesInCoordinate.Value)
				{
					SetCurrentOponentsOf(EntityRegistry[entityID]);
				}
			}
		}

		private void SetCurrentOponentsOf(IEntity entity)
		{
			if (!(entity is ICombatant))
				return;

			ICombatant combatant = (ICombatant)entity;
			combatant.ClearCurrentOponents();

			foreach (KeyValuePair<Type, List<uint>> allEntiiesInCoordinate in Map.GetAllEntitiesIn((TCoordinate)entity.GetCoordinate()))
			{
				foreach (uint otherEntityID in allEntiiesInCoordinate.Value)
				{
					if (!(EntityRegistry[otherEntityID] is ICombatant))
						continue;

					ICombatant otherCombatant = (ICombatant)EntityRegistry[otherEntityID];
					if (AreOponents(combatant, otherCombatant))
						combatant.AddOponent(otherCombatant);
				}
			}
		}

		private bool AreOponents(ICombatant combatantA, ICombatant combatantB)
		{
			return (combatantA is Ant<TCell, TCoordinate> && combatantB is EnemyBug<TCell, TCoordinate>) ||
				   (combatantB is Ant<TCell, TCoordinate> && combatantA is EnemyBug<TCell, TCoordinate>);
		}

		public void Dispose()
		{
			EventBus.Unsubscribe<EntityChangeCoordinateEvent>(UpdateCurrentOponents);
			EventBus.Unsubscribe<EntityDestroyEvent>(UpdateCurrentOponents);
		}
	}
}
